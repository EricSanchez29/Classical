using System;
namespace Maths.LinearAlgebra;

public static class LinearSystemSolver
{
    private static readonly int _precision = Global.Precision;

    // An implementation of Cramer's rule
    //
    //      this requires the determinant of the input matrix to be non zero
    //
    // O((n+1)*2.2n + n(2n)) ~ O(n^2)
    public static double[] FindLinearSystemSolution_Exact(Matrix A, double[] b)
    {
        // a11X1 + a12X2 + ... + a1nXn = b1
        // a21x1 + a22X2 + ... + a2nXn = b2
        // ..
        // an1X1 + an2X2 + ... + annXn = bn
        // Ax = b

        // where the A matrix is this.matrix

        // Xj = Dj/D
        // D = det A

        //          a11 a12 ... a1j-1 b1 a1j+1 a1n
        // Dj = det a21 a22 ... a2j-1 b2 a2j+1 a2n
        //          a31 a32 ... a3j-1 b3 a3j+1 a3n
        //          ...

        // This implementation of the Determinant algo (Laplace Expansion)
        // is modified to run at O(n) as opposed to O(n!)
        //      is this true?
        //
        // Changine Algorithm to save any relevant computed submatrix
        // aka only saving submatrices that I am guaranteed to encounter
        //
        // the object A has a dictionary which retains values between calculations of GetDeterminant
        // if and only if IsCramerRule == true, for all of those calucalations
        var determinant = A.GetDeterminant(true);

        // Am about to divide by the original determinant of matrix A
        // making sure I don't divide by zero
        if (determinant == double.NaN || determinant == 0.0)
        {
            return null;
        }

        var result = new double[A.Dimensions.Column];

        for (int j = 0; j < A.Dimensions.Column; j++)
        {
            var originalColumn = new double[A.Dimensions.Column];

            // substitute the b column vector for this(j) column
            for (int i = 0; i < A.Dimensions.Row; i++)
            {
                originalColumn[i] = A.matrix[i][j];
                A.matrix[i][j] = b[i];
            }

            // calculate Xj = Dj/D
            // need to add 1 because of zero index
            var Dj = A.GetDeterminant(true, j + 1);

            result[j] = Math.Round(Dj / determinant, _precision);

            // replace original column vector
            for (int i = 0; i < A.Dimensions.Row; i++)
            {
                A.matrix[i][j] = originalColumn[i];
            }
        }

        return result;
    }

    public static double[] FindLinearSystemSolution_Approx(Matrix A, double[] b, double[] x = null, double[] _x = null)
    {
        if ((A?.Dimensions.Row ?? int.MinValue) == int.MinValue)
        {
            return null;
        }

        if (((A?.matrix?[0][0] ?? 0.0) == 0.0)
            || ((b?.Length ?? 0) == 0))
        {
            return null;
        }

        // Two initial guesses for x
        x ??= new double[b.Length];
        _x ??= new double[b.Length];
        for (int i = 0; i < b.Length; i++)
            _x[i] = 1;

        // Picking the correct algorithnm to approximate the solution,
        // based on if the input matrix m is hermitian or not
        //
        // Since I am not using complex values, only need to do the transpose
        // in order to determine if a matrix is hermitian
        // (as opposed to a the conjugate transpose)
        var conjugateTranpose = A.Transpose();
        if (!A.Equals(conjugateTranpose))
        {
            // Should never run this algorithm with a Herimitian matrix
            // Runs twice as slow with this method vs conjugate gradient
            return biconjugateGradient(A, b, x, _x);
        }
        else
        {
            return conjugateGradientMethod(A, b, x);
        }
    }

    // Only for real Hermitian matricies
    // A = A^t = (A^T)* = A^T
    // aka when the transpose of the matrix is equal to itself
    // O(nlog(n)) [theoretical best case for sparse matrix]
    private static double[] conjugateGradientMethod(Matrix A, double[] b, double[] x)
    {
        // residual vector r0
        // r0 = b - Ax0
        var r = VectorAlgebra.Subtract(b, Matrix.Multiply(A, x));

        // if intial guess is close enough, return
        if (isSuffcientlySmall(r))
        {
            return VectorAlgebra.RoundVector(x);
        }

        // search direction vector p0 = r0
        // for the first iteration
        var p = r;

        // for now doing a predetermined number of iterations
        // later may switch to exit while loop when x is suffciently small
        for (int k = 1; k < 10000; k++)
        {
            // scalar a0
            // a0 = (r0 * r0) / (x0 * A * p0)
            var a_denominator = VectorAlgebra.DotProduct(p, Matrix.Multiply(A, p));
            if (a_denominator == 0.0)
            {
                // bad x0 guess?
                return null;
            }

            var a = VectorAlgebra.DotProduct(r, r) / a_denominator;

            // compute x1
            // x1 = x0 + a0p0
            x = VectorAlgebra.Add(x, VectorAlgebra.Multiply(a, p));

            // compute residual vector r1
            // r_next = r - aAp
            var r_next = VectorAlgebra.Subtract(r, VectorAlgebra.Multiply(a, Matrix.Multiply(A, p)));
            if (isSuffcientlySmall(r_next))
            {
                Console.WriteLine(@"Iterations: {0}", k);
                return VectorAlgebra.RoundVector(x);
            }

            // compute scalar bk
            // beta = r1 * r1 / r0 * r0
            var beta = VectorAlgebra.DotProduct(r_next, r_next) / VectorAlgebra.DotProduct(r, r);

            // compute direction vector p1
            // p1 = r1 + beta*p0
            p = VectorAlgebra.Add(r_next, VectorAlgebra.Multiply(beta, p));

            r = r_next;
        }

        Console.WriteLine("Broke out of loop =(");

        return VectorAlgebra.RoundVector(x);
    }

    // Used to help determine when I have a viable solution 
    public static bool isSuffcientlySmall(double[] vector)
    {
        // ||v|| = v * v < error than can be considered a zero vector
        if (VectorAlgebra.DotProduct(vector, vector) < 0.001)
        {
            return true;
        }

        return false;
    }

    // Can handle any real valued matrix (A)
    // real valued vectors b, x and & _x
    private static double[] biconjugateGradient(Matrix A, double[] b, double[] x, double[] _x)
    {
        // residual vector r0
        // r0 = b - Ax0
        var r = VectorAlgebra.Subtract(b, Matrix.Multiply(A, x));
        var _r = VectorAlgebra.Subtract(Matrix.Multiply(A, _x), b);

        // if intial guess is close enough, return
        if (isSuffcientlySmall(r))
        {
            return VectorAlgebra.RoundVector(x);
        }

        if(isSuffcientlySmall(_r))
        {
            return VectorAlgebra.RoundVector(_x);
        }

        // search direction vector p0 = r0
        // for the first iteration
        var p = r;
        var _p = _r;

        // for now doing a predetermined total number of iterations
        // exit loop when x is suffciently small
        for (int k = 1; k < 10000; k++)
        {
            // scalar ak
            // ak = (_rk * rk) / (_pk * A * pk)
            var a_denominator = VectorAlgebra.DotProduct(_p, Matrix.Multiply(A, p));
            if (a_denominator == 0.0)
            {
                // bad x0 guess?
                return null;
            }

            var a = VectorAlgebra.DotProduct(_r, r) / a_denominator;

            // compute x1
            // x1 = x0 + a0p0
            x = VectorAlgebra.Add(x, VectorAlgebra.Multiply(a, p));
            _x = VectorAlgebra.Add(_x, VectorAlgebra.Multiply(a, _p));

            // compute residual vector r1
            // r_next = r - aAp
            var r_next = VectorAlgebra.Subtract(r, VectorAlgebra.Multiply(a, Matrix.Multiply(A, p)));
            if (isSuffcientlySmall(r_next))
            {
                Console.WriteLine(@"Iterations: {0}", k);
                return VectorAlgebra.RoundVector(x);
            }

            var _r_next = VectorAlgebra.Subtract(_r, VectorAlgebra.Multiply(a, Matrix.Multiply(A, _p, true)));
            if (isSuffcientlySmall(_r_next))
            {
                Console.WriteLine(@"Iterations: {0}", k);
                return VectorAlgebra.RoundVector(_x);
            }

            // compute scalar beta
            // beta = _rk+1 * rk+1 / _rk * rk
            var beta = VectorAlgebra.DotProduct(_r_next, r_next) / VectorAlgebra.DotProduct(_r, r);

            // compute direction vector p1
            // pk = rk + beta*pk
            p = VectorAlgebra.Add(r_next, VectorAlgebra.Multiply(beta, p));
            _p = VectorAlgebra.Add(_r_next, VectorAlgebra.Multiply(beta, _p));

            r = r_next;
            _r = _r_next;
        }

        Console.WriteLine("Broke out of loop =(");

        // default case if I break out of the loop without a proper solution
        return VectorAlgebra.RoundVector(x);
    }
}

namespace Maths.LinearAlgebra;

public static class LinearSystemAlgorithms
{
    private static readonly int _precision = Global.Precision;

    /// Cramer's Rule Modified
    #region


    // An implementation of Cramer's rule
    //
    //      this requires the determinant of the input matrix to be non zero
    //
    // O((n+1)*2.2n + n(2n)) ~ O(n^2)
    public static double[] CramersRuleModified(Matrix A, double[] b)
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
        if (double.IsNaN(determinant) || determinant == 0.0)
        {
            throw new Exception();
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
    #endregion

    /// Conjugate Transpose Method
    #region
    public static double[] ConjugateTransposeMethod(Matrix A, double[] b, double[]? x = null, double[]? _x = null)
    {
        if ((A == null) || (b == null) || (b.Length == 0))
        {
            throw new ArgumentException();
        }

        if (A.Dimensions.Row != A.Dimensions.Column)
        {
            throw new ArgumentException();
        }

        // Two initial guesses for x
        x ??= new double[b.Length];

        // Picking the correct algorithnm to approximate the solution,
        // based on if the input matrix m is hermitian or not
        //
        // Since I am not using complex values, only need to do the transpose
        // in order to determine if a matrix is hermitian
        // (as opposed to a the conjugate transpose)
        var conjugateTranpose = A.Transpose();

        if (!A.Equals(conjugateTranpose))
        {
            _x ??= new double[b.Length];
            for (int i = 0; i < b.Length; i++)
                _x[i] = 1;

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
            return VectorAlgebra.Round(x);
        }

        // search direction vector p0 = r0
        // (only for the first iteration)
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
                return new double[0];
            }

            var a = VectorAlgebra.DotProduct(r, r) / a_denominator;

            // compute xk
            // xk = x_k-1 + a_k * p_k-1
            x = VectorAlgebra.Add(x, VectorAlgebra.Multiply(a, p));

            // compute residual vector r1
            // r_next = r - aAp
            var r_next = VectorAlgebra.Subtract(r, VectorAlgebra.Multiply(a, Matrix.Multiply(A, p)));
            if (isSuffcientlySmall(r_next))
            {
                Console.WriteLine(@"Iterations: {0}", k);
                return VectorAlgebra.Round(x);
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

        return VectorAlgebra.Round(x);
    }

    // Used to help determine when I have a viable solution
    // for the to conjugate grad methods
    public static bool isSuffcientlySmall(double[] vector)
    {
        // ||v|| = v * v < error than can be considered a zero vector
        if (VectorAlgebra.DotProduct(vector, vector) < 0.0001)
        {
            return true;
        }

        return false;
    } 

    // Can handle any real valued matrix (A),
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
            return VectorAlgebra.Round(x);
        }

        if(isSuffcientlySmall(_r))
        {
            return VectorAlgebra.Round(_x);
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
                return Array.Empty<double>();
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
                return VectorAlgebra.Round(x);
            }

            var _r_next = VectorAlgebra.Subtract(_r, VectorAlgebra.Multiply(a, Matrix.Multiply(A, _p, true)));
            if (isSuffcientlySmall(_r_next))
            {
                Console.WriteLine(@"Iterations: {0}", k);
                return VectorAlgebra.Round(_x);
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
        return VectorAlgebra.Round(x);
    }
    #endregion

    /// Chios Extended Condensation Method
    /// (Depends on Cramer's Rule Modified)
    #region
    // adapted from Habgood & Arel (2011)
    public static double[] ChiosExtendedCondensationMethod(Matrix A, double[] b)
    {
        // For now only handle square matricies
        if (!A.IsSquare || A.Dimensions.Row != b.Length)
        {
            throw new ArgumentException("Not square, or # rows in A doesn't equal length of b");
        }

// these inputs might not be right
        return ChiosExtendedCondensationMethodHelper(A, b, b.Length, b.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="A"></param>
    /// <param name="b"></param>
    /// <param name="currentSize"></param>
    /// <param name="mirrorSize"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static double[] ChiosExtendedCondensationMethodHelper(Matrix A, double[] b, int currentSize, int mirrorSize)
    { 
        var result = new double[mirrorSize];

        // Condensation step size
        int m = 2;

        // current matrix's size
        int n = currentSize;            // unnecesary assignment?

        // reusable matrix must be the same size as input A
        // as it contains all minor dets for every position in A
        double[][] reusableminor = new double[n][];
        for (int i = 0; i < n; i++)
        {
            reusableminor[i] = new double[n];
        }

        // unknowns for this matrix to solve for
        int mirrorsize = n;

        while ((n - m) > mirrorsize)
        {
            var leadDeterminant = A.CalculateMinor(m + 1, m + 1);
            var leadMinor = new double[m];

            if (leadDeterminant == 0)
                throw new Exception("Can't divide by zero =(");

            // divide lead column by minor of at A(1,1)
            for (int i = 0; i < n - 1; i++)
            {
                A.matrix[i][1] = A.matrix[i][1] / leadDeterminant;
            }

            // calculate the minors that are common
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    // each minor will exclude one row & col from this matrix A
                    reusableminor[i][j] = A.CalculateMinor(i, j);
                }
            }

            for (int row = m + 1; row <= n + 1; row++)
            {
                // find the lead minors for this row
                for (int i = 1; i <= m; i++)
                {
                    leadMinor[i] = 0;

                    for (int j = 1; j <= m; j++)
                    {
                        if (j % 2 != 0)
                            leadMinor[i] = leadMinor[i] + (A.matrix[row][j] * reusableminor[i][j]);
                        else
                            leadMinor[i] = leadMinor[i] - (A.matrix[row][j] * reusableminor[i][j]);
                    }
                }

                // Core Loop: find the m x m determinant for each elmement in a
                for (int col = m + 1; col <= n + 1; col++)
                {
                    for (int i = 0; i <= m; i++)
                    {
                        // calculate m x m determinant
                        if (i % 2 == 0)
                            A.matrix[row][col] = A.matrix[row][col] + (leadMinor[i] * A.matrix[i][col]);
                        else
                            A.matrix[row][col] = A.matrix[row][col] - (leadMinor[i] * A.matrix[i][col]);
                    }
                }
            }

            // reduce matrix size by condensation step size
            n -= m;
        }
        // End condensation, solve matrix
        if (n == 6)
        {
            // solve for the subset of unknowns assigned
            // x[] = cramersrule[b]

            // Should I reuse my old code?
            // Maybe this becomes more worth while at n == 5 or 6,
            // which means less condesation steps?




            return CramersRuleModified(A, b);
        }
        // Continue condensation steps
        else
        {
            var mirrorA = Matrix.Mirror(A);

            ChiosExtendedCondensationMethodHelper(A, b, currentSize / 2, 1);

            ChiosExtendedCondensationMethodHelper(mirrorA, b, currentSize/2, 1);
        }

        return result;
    }
    #endregion

}

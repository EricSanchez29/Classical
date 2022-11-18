﻿using Maths.LinearAlgebra;

namespace SystemLinearEquations.LinearSystemAlgorithms
{
    public class ConjugateTransposeMethods : ILinearSystemMethod
    {
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

            if (isSuffcientlySmall(_r))
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

        public double[] Solve(Matrix A, double[] b)
        {
            throw new NotImplementedException();
        }
    }
}
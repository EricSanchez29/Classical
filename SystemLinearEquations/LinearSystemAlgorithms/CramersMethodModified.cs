using Maths;
using Maths.LinearAlgebra;

namespace SystemLinearEquations.LinearSystemAlgorithms
{
    public class CramersMethodModified : ILinearSystemMethod
    {
        // An implementation of Cramer's rule
        //
        //      this requires the determinant of the input matrix to be non zero
        //
        // O((n+1)*2.2n + n(2n)) ~ O(n^2)
        public double[] Solve(Matrix A, double[] b)
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

                result[j] = Math.Round(Dj / determinant, Global.Precision); ;

                // replace original column vector
                for (int i = 0; i < A.Dimensions.Row; i++)
                {
                    A.matrix[i][j] = originalColumn[i];
                }
            }

            return result;
        }
    }
}

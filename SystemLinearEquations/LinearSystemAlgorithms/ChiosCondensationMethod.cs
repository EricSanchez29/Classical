using Maths;
using Maths.LinearAlgebra;

namespace SystemLinearEquations.LinearSystemAlgorithms
{
    public class ChiosCondensationMethod : ILinearSystemMethod
    {
        /// Chios Extended Condensation Method
        /// (Depends on Cramer's Rule Modified)
        /// adapted from Habgood & Arel (2011)


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
            const int sizeLimit = 6;

            if (A.Dimensions.Column <= sizeLimit)
            {
                return CramersMethod(A, b);
            }

            var result = new double[mirrorSize];

            // Condensation step size
            int m = 2;

            // reusable matrix must be the same size as input A
            // as it contains all minor dets for every position in A
            double[][] reusableminor = new double[currentSize][];
            for (int i = 0;  i < currentSize; i++)
            {
                reusableminor[i] = new double[currentSize];
            }

            // unknowns for this matrix to solve for
            int mirrorsize = currentSize;

            while ((currentSize - m) > mirrorsize)
            {
                var leadDeterminant = A.CalculateMinor(m + 1, m + 1);
                var leadMinor = new double[m];

                if (leadDeterminant == 0)
                    throw new Exception("Can't divide by zero =(");

                // divide lead column by minor of at A(1,1)
                for (int i = 0; i < currentSize - 1; i++)
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

                for (int row = m + 1; row <= currentSize + 1; row++)
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
                    for (int col = m + 1; col <= currentSize + 1; col++)
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
                currentSize -= m;
            }
            // End condensation, solve matrix
            if (currentSize == sizeLimit)
            {
                return CramersMethod(A, b);
            }
            // Continue condensation steps
            else
            {
                var mirrorA = A.Mirror();

                var answer1 = ChiosExtendedCondensationMethodHelper(A, b, currentSize / 2, 1);

                var answer2 = ChiosExtendedCondensationMethodHelper(mirrorA, b, currentSize / 2, 1);

                if (answer1.Length + answer2.Length != result.Length)
                {
                    throw new Exception();
                }

                for (int i = 0; i < answer1.Length; i++)
                {
                    result[i] = answer1[i];
                }

                for (int i = answer1.Length; i < answer2.Length; i++)
                {
                    result[i] = answer2[i];
                }
            }

            return result;
        }

        public double[] Solve(Matrix A, double[] b)
        {
            //For now only handle square matricies
            if (!A.IsSquare)
            {
                throw new ArgumentException("Matrix A must be a square");
            }

            if (A.Dimensions.Row != b.Length)
            {
                throw new ArgumentException("Number of rows in A doesn't equal length of b");
            }


            // these inputs might not be right
            return ChiosExtendedCondensationMethodHelper(A, b, b.Length, b.Length);
        }

        // this is reused code, bad
        public static double[] CramersMethod(Matrix A, double[] b)
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
            var determinant = A.GetDeterminant(false);

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
                var Dj = A.GetDeterminant(false);

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

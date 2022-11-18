using Maths.LinearAlgebra;

namespace SystemLinearEquations.LinearSystemAlgorithms
{
    public class ChiosCondensationMethod : ILinearSystemMethod
    {

        /// Chios Extended Condensation Method
        /// (Depends on Cramer's Rule Modified)
        #region
        // adapted from Habgood & Arel (2011)
        public static double[] ChiosExtendedCondensationMethod(Matrix A, double[] b)
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

            //



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

            // reusable matrix must be the same size as input A
            // as it contains all minor dets for every position in A
            double[][] reusableminor = new double[currentSize][];
            for (int i = 0; i < currentSize; i++)
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
            if (currentSize == 6)
            {
                // solve for the subset of unknowns assigned
                // x[] = cramersrule[b]

                // Should I reuse my old code?
                // Maybe this becomes more worth while at n == 5 or 6,
                // which means less condesation steps?




                return null;// CramersRuleModified(A, b);
            }
            // Continue condensation steps
            else
            {
                var mirrorA = A.Mirror();

                ChiosExtendedCondensationMethodHelper(A, b, currentSize / 2, 1);

                ChiosExtendedCondensationMethodHelper(mirrorA, b, currentSize / 2, 1);
            }

            return result;
        }

        public double[] Solve(Matrix A, double[] b)
        {
            throw new NotImplementedException();
        }
    }
}

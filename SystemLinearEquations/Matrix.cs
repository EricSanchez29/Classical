using System.Text;

namespace Maths.LinearAlgebra;

// This class doesn't allow you to change the dimensions of a matrix (even for Transpose())
// Need to create a new instance instead 
public class Matrix : MatrixBase
{
    // this might slow down 
    private Dictionary<string, double> _originalDeterminants = new Dictionary<string, double>();

    // used for benchmarking
    //private int _2x2_Count = 0;
    //private int _dictionaryCalls = 0;

    // Matrix elements are usually indexed from 1
    // however arrays are 0 indexed, deal with it
    public double[][] matrix;

    // fix this!
    public override bool Equals(object obj)
    {
        // could also write
        // (this is null && obj is null
        if (this == null && obj == null)
            return true;

        else if (this == null || obj == null)
            return false;

        if (obj is not Matrix m2)
            return false;

        if (this.Dimensions != m2.Dimensions)
            return false;

        for (int i = 0; i < m2.Dimensions.Row; i++)
            for (int j = 0; j < m2.Dimensions.Column; j++)
                if (this.matrix[i][j] != m2.matrix[i][j])
                    return false;

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), matrix);
    }

    // Using math convention:
    // Matrices are referred to by row by column (y, x) order
    // same with matrix elements Ayx  (A11 as the first element in the top left corner)
    public Matrix(int row, int column) : base(row, column)
    {
        this.matrix = new double[row][];

        for (int i = 0; i < row; i++)
            matrix[i] = new double[column];
    }

    // Don't currently check that the inputMatrix has consistently sized arrays
    // double[i][].Length == double[i+1][].Length
    // (aka is not a jagged array)
    public Matrix(double[][] inputMatrix) : base(inputMatrix?.Length ?? 0, inputMatrix?[0].Length ?? 0)
    {
        if (inputMatrix == null)
        {
            throw new ArgumentNullException(nameof(inputMatrix));
        }

        this.matrix = new double[inputMatrix.Length][];

        for (int i = 0; i < inputMatrix.Length; i++)
            matrix[i] = new double[inputMatrix[0].Length];

        for (int i = 0; i < inputMatrix.Length; i++)
            Array.Copy(inputMatrix[i], this.matrix[i], inputMatrix[i].Length);
    }

    public static Matrix Copy(Matrix input)
    {
        var output = new Matrix(input.Dimensions.Row, input.Dimensions.Column);

        for (int i = 0; i < input.Dimensions.Row; i++)
            for (int j = 0; j < input.Dimensions.Column; j++)
                output.matrix[i][j] = input.matrix[i][j];

        return output;
    }

    public Matrix Copy()
    {
        return Copy(this);
    }

    public static Matrix GetIdentityMatrix(int size)
    {
        Matrix result = new Matrix(size, size);

        for (int i = 0; i < size; i++)
            result.matrix[i][i] = 1;

        return result;
    }

    // I only save the resulting dictonary to this object when
    //  isCramersRule == true && cramersRuleIteration == 0
    // 
    // I only lookup this._originalDeterminants when 
    //  isCramersRule == true && cramersRuleIteration != 0
    public double GetDeterminant(bool isCramersRule = false, int cramersRuleIteration = 0)
    {
        if (this.matrix is null || !this.IsSquare)
            return double.NaN;

        if (this.Dimensions.Column == 1)
            return this.matrix[0][0];

        if (this.Dimensions.Column == 2)
            return Math.Round(this.matrix[0][0] * this.matrix[1][1], Global.Precision) - Math.Round(this.matrix[0][1] * this.matrix[1][0], Global.Precision);

        // This is an implementation of the Laplace Expansion method of calculating a determinant
        // aka det(A) = Sum {(-1)^i+j * Aij * Mij}
        //
        // In this implementation I store the values of Mij the first time they are computed
        // (Due to the nature of the laplace expansion, many of the smaller cofactor matrices are repeatedly used)
        // 
        // Laplace Expansion = O(n!)
        // Modified Laplace ~ O(2.2n) with Aux(2^n)
        if (cramersRuleIteration < 0)
        {
            throw new ArgumentException("cramersRuleIteration cannot be less than 0");
        }

        if (cramersRuleIteration > 0 && isCramersRule == false)
        {
            // iteration number should be zero if isCramersRule == false
            throw new ArgumentException("Impossible state, don't need positive cramersRuleIteration, if not executing Cramer's Rule ");
        }

        double determinant = 0;

        // dictionary is growing at 2^n for first pass
        // then 2^(n-1) for each subsequent GetDeterminant() call
        //
        // total 2^(n+1) for entire Cramers algorithm

        var power = Dimensions.Column;

        if (cramersRuleIteration > 0)
            power--;

        var dictionarySizeBound = Math.Pow(2, power);

        Dictionary<string, double> calculatedDeterminants = new Dictionary<string, double>(Convert.ToInt32(dictionarySizeBound));

        var allowed = new List<int>();
        for (int c = 1; c <= Dimensions.Column; c++)
            allowed.Add(c);

        // using regular matrix coordinate notation
        for (int i = 1; i <= Dimensions.Column; i++)
        {
            double minorDet;

            if (isCramersRule)
            {
                if (cramersRuleIteration == 0)
                {
                    // populate this.dictionary (store all calculations from determinant of orignal A)
                    // ie before subsequent calls with modified A 
                    minorDet = getDeterminantHelper(1, i, calculatedDeterminants, allowed);
                    _originalDeterminants = calculatedDeterminants;
                }
                // only once per Dj.GetDeterminant in CramersRule
                // the function getSubMatrix() will find a repeat calculation from the intitial getDet()
                else
                {
                    minorDet = getDeterminantHelper(1, i, calculatedDeterminants, allowed, cramersRuleIteration);
                }
            }
            else
            {
                // ignore dictionary associated with this object
                // find answer using local dictionary which is not persisted
                minorDet = getDeterminantHelper(1, i, calculatedDeterminants, allowed);
            }

            double cofactorDet = matrix[0][i - 1] * minorDet;

            if (i % 2 != 0)
                determinant += cofactorDet;
            else
                determinant -= cofactorDet;
        }

        //Console.WriteLine("n: " + this.Dimensions.Column);
        //Console.WriteLine("2x2: " + _2x2_Count);
        //_2x2_Count = 0;
        //long count = 1;
        //Console.WriteLine("DictionaryCalls(both): " + _dictionaryCalls);
        //Console.WriteLine("DictionarySize(bigger): " + _minorDeterminants.Count);
        //// Remove this?
        //// This is the number of function calls to get det(2x2)  would be made by
        //// a normal laplace expansion implementation (n!)
        ////
        //// I stop at i < 2 because that is the simple 2x2 det calculation which is not recursive
        //for (int i = this.Dimensions.Column; i > 2; i--)
        //{
        //    count = i * count;
        //}

        //Console.WriteLine("Theoretical ca
        //ls for Laplace expansion:" + count);

        return determinant;
    }

    // two modes
    // one without a special column (specialColumn == 0)
    // one with a special column (0 < specialColumn <= n)
    //      this means I check the common dictionary this._originalDeterminants
    private double getDeterminantHelper(int row, int column, Dictionary<string, double> calculations, List<int> allowedColumns, int modfiedColumn = 0)
    {
        // Is it worth using a list?
        // In this function I use:
        //  -   Contains(int)
        //  -   Remove(int)
        //  -   Add(int)
        //  -   Sort()

        if ((row <= 0) || (column <= 0) || (row > Dimensions.Row) || (column > this.Dimensions.Column))
            throw new Exception("Something went wrong");

        if (modfiedColumn < 0)
            throw new Exception("Unexpected input");

        if (row < matrix.Length - 2)
        {
            allowedColumns.Remove(column);
            double determinant = 0;

            int length = allowedColumns.Count;

            byte[] keyArray = new byte[length];

            for (int i = 0; i < length; i++)
            {
                keyArray[i] = Convert.ToByte(allowedColumns[i]);
            }

            string key = Encoding.Default.GetString(keyArray);

            bool containsModifiedColumn = allowedColumns.Contains(modfiedColumn);

            // only check if this is not the first det calculation in Cramers
            if (((modfiedColumn == column) || !containsModifiedColumn) && (modfiedColumn != 0))
            {
                //_dictionaryCalls++;
                determinant = _originalDeterminants[key];
            }
            else if (!calculations.ContainsKey(key))
            {
                for (int i = 0; i < allowedColumns.Count; i++)
                {
                    var mult = this.matrix[row][allowedColumns[i] - 1] * getDeterminantHelper(row + 1, allowedColumns[i], calculations, allowedColumns, modfiedColumn);

                    if ((i) % 2 == 0)
                        determinant += mult;
                    else
                        determinant -= mult;
                }

                calculations.Add(key, determinant);
            }
            else
            {
                //_dictionaryCalls++;
                determinant = calculations[key];
            }

            allowedColumns.Add(column);
            allowedColumns.Sort();

            return determinant;
        }

        // calculate the case: det(2x2)
        // there is no smaller cofactor
        // so, no smaller minor to calculate
        // Therefore, calculate this.minor
        else if (row == matrix.Length - 2)
        {
            allowedColumns.Remove(column);

            // at this point, only 2 columns should be allowed
            if (allowedColumns.Count != 2)
                throw new Exception("Something went terribly wrong");

            int leftColumn = allowedColumns[0];
            int rightColumn = allowedColumns[1];

            var keyArray = new byte[2];

            keyArray[0] = Convert.ToByte(leftColumn);
            keyArray[1] = Convert.ToByte(rightColumn);

            string key = Encoding.Default.GetString(keyArray);

            //bool noModifiedColumn = allowedColumns.Contains(modfiedColumn);
            // this should be more efficent than above?
            bool containsModifiedColumn = ((allowedColumns[0] == modfiedColumn) || (allowedColumns[1] == modfiedColumn));

            allowedColumns.Add(column);
            allowedColumns.Sort();

            if (((modfiedColumn == column) || !containsModifiedColumn) && (modfiedColumn != 0))
            {
                //_dictionaryCalls++; ;

                return _originalDeterminants[key];
            }

            else if (!calculations.ContainsKey(key))
            {
                // Used to benchmark vs no dictionary implementation
                //_2x2_Count++;

                var det = (this.matrix[this.Dimensions.Row - 2][leftColumn - 1] * this.matrix[this.Dimensions.Row - 1][rightColumn - 1]) - (this.matrix[this.Dimensions.Row - 1][leftColumn - 1] * this.matrix[this.Dimensions.Row - 2][rightColumn - 1]);
                calculations.Add(key, det);

                return det;
            }
            else
            {
                //_dictionaryCalls++;

                return calculations[key];
            }
        }
        else
        {
            // This is only ever reached if the input row is too large
            // or too small for the size of the matrix, which is currently never
            return double.NaN;
        }
    }

    // Finds the determinant of a submatrix of this.matrix
    // created by ignoring the row and column selected.
    // Will use my old method of creating a dictionary to help with calculations
    public double CalculateMinor(int row, int column)
    {
        if ((row < 1) || (row > Dimensions.Row) || (column < 1) || (column > Dimensions.Column))
        {
            throw new ArgumentOutOfRangeException("Cannot compute minor " + row + "x" + column);
        }

        // should i even by creating a (1x1) matrix before calling this function?
        if ((Dimensions.Column == 2) && (Dimensions.Row == 2))
        {
            // | a11 a12 |
            // | a21 a22 |
            //
            // no determinant to calculate
            return matrix[0][0];
        }

        //should do this outside of this function, this was in the paper
        if ((row == 1 + Dimensions.Row) && (column == 1 + Dimensions.Column))
        {
            // get the determinant of this matrix, don't exclude a row/column
            // using lists and dictionaries O(2^n)
            return GetDeterminant();
        }


        // maybe should optimize this for matrices which don't benefit much from a dictionary (n~4)
        // (definitely not worth it at n~3)
        // 

        var power = Dimensions.Column;

        var dictionarySizeBound = Math.Pow(2, power);

        Dictionary<string, double> calculatedDeterminants = new Dictionary<string, double>(Convert.ToInt32(dictionarySizeBound));

        var allowedColumns = new List<int>(Dimensions.Column - 1);
        for (int c = 1; c <= Dimensions.Column; c++)
        {
            if (c == column)
                continue;

            allowedColumns.Add(c);
        }

        var allowedRows = new List<int>(Dimensions.Row - 1);
        for (int c = 1; c <= Dimensions.Row; c++)
        {
            // don't include this column
            if (c == row)
                continue;

            allowedRows.Add(c);
        }

        double determinant = getMinorDetHelper(calculatedDeterminants, allowedColumns, allowedRows);

        // not based on dimensions of the bigger matrix
        //for (int i = 0; i < Dimensions.Column - 1; i++)
        //{
        //    int thisColumn = column;
        //    //allowedColumns.Remove(thisColumn);

        //    double cofactorDet = matrix[row - 1][thisColumn - 1] * getMinorDetHelper(calculatedDeterminants, allowedColumns, allowedRows);

        //    if (getSign(row, thisColumn))
        //        determinant += cofactorDet;
        //    else
        //        determinant -= cofactorDet;

        //    allowedColumns.Add(thisColumn);

        //    allowedColumns.Sort();
        //}
        //


        // why? (because
        if (getSign(row, column))
            determinant *= 1;
        else
            determinant *= -1;

        return determinant;
    }

    // + true
    // - odd
    private static bool getSign(int row, int column)
    {
        if ((isEven(row) && isEven(column)) || (!isEven(row)) && (!isEven(column)))
        {
            return true;
        }

        return false;
    }

    private static bool isEven(int num)
    {
        return num % 2 == 0;
    }

    // Scenarios (I [shouldn't?]won't ever call out of bounds to begin with, aka calling the ignored column or coordinates which don't exist in the matrix)
    //
    // 1st: row < 2 + end of matrix
    //      and the ignored row is the last row in meatrix
    // 2nd: this.row = 2 + end matrix
    //      and the ignored row is the last row
    // 3rd: this.row = 2 + end matrix
    //      and the ignored row is the next row (2nd to last row)
    // 

    // need to add ignore row list
    // consider making the allowedcolumns and allowedRows dictionaries
    private double getMinorDetHelper(Dictionary<string, double> calculations, List<int> allowedColumns, List<int> allowedRows)
    {
        if (allowedRows.Count != allowedColumns.Count)
        {
            throw new Exception("Oops");
        }

        //1st: this.row is more than two rows less(higher on the matrix, earlier call) than the end of the matrix(aka row == Dimensions.Row)
        //      and the ignored row is the last row
        // 
        //                              extra careful for now
        if (allowedColumns.Count > 2 && allowedRows.Count > 2)
        {
            double determinant = 0;

            int length = allowedColumns.Count;

            byte[] keyArray = new byte[length * 2];

            for (int i = 0; i < length; i++)
            {
                keyArray[i] = Convert.ToByte(allowedColumns[i]);
            }

            for (int i = 0; i < length; i++)
            {
                keyArray[i + length] = Convert.ToByte(allowedRows[i]);
            }

            string key = Encoding.Default.GetString(keyArray);

            

            if (!calculations.ContainsKey(key))
            {
                int removeRow = allowedRows[0];

                int removeColumn;
                allowedRows.Remove(removeRow);

                for (int i = 0; i < length; i++)
                {
                    removeColumn = allowedColumns[i];

                    allowedColumns.Remove(removeColumn);

                    var mult = this.matrix[removeRow - 1][removeColumn - 1];

                    mult = mult * getMinorDetHelper(calculations, allowedColumns, allowedRows);


                    if (getSign(removeRow, removeColumn))
                        determinant += mult;
                    else
                        determinant -= mult;


                    allowedColumns.Add(removeColumn);
                    allowedColumns.Sort();
                }

                calculations.Add(key, determinant);
                allowedRows.Sort();
                allowedRows.Add(removeRow);
            }
            else
            {
                //_dictionaryCalls++;
                determinant = calculations[key];
            }

            return determinant;
        }

        // this needs different logic
        else if (allowedColumns.Count == 2)
        {
            /*
             *  3 ways to reach the final 2x2 determinant calculation
             * 
             *   |   ...    | Not Allowed
             *   |   ...    | Allowed
             *   |   ...    | Allowed
             * 
             *   |   ...    | Allowed
             *   |   ...    | Not Allowed
             *   |   ...    | Allowed
             * 
             *   |   ...    | Allowed
             *   |   ...    | Allowed
             *   |   ...    | Not Allowed
             * 
             * 
             * 
             * 
             * 
             */

            // calculate the case: det(2x2)
            // there is no smaller cofactor
            // so, no smaller minor to calculate
            // Therefore, calculate this.minor


            int leftColumn = allowedColumns[0];
            int rightColumn = allowedColumns[1];
            int topRow = allowedRows[0];
            int bottomRow = allowedRows[1];

            // could see a big performance impact on this algorithm
            // need to do this to accomodate ignoring rows
            var keyArray = new byte[4];

            keyArray[0] = Convert.ToByte(leftColumn);
            keyArray[1] = Convert.ToByte(rightColumn);
            keyArray[2] = Convert.ToByte(topRow);
            keyArray[3] = Convert.ToByte(bottomRow);
            string key = Encoding.Default.GetString(keyArray);

            //allowedColumns.Add(col);
            //allowedColumns.Sort();

            if (!calculations.ContainsKey(key))
            {
                // Used to benchmark vs no dictionary implementation
                //_2x2_Count++;

                var det = (this.matrix[topRow - 1][leftColumn - 1] * this.matrix[bottomRow - 1][rightColumn - 1]) - (this.matrix[bottomRow - 1][leftColumn - 1] * this.matrix[topRow - 1][rightColumn - 1]);
                calculations.Add(key, det);

                return det;
            }
            else
            {
                //_dictionaryCalls++;

                return calculations[key];
            }
        }

        else
        {
            // This is only ever reached if the input row is too large
            // or too small for the size of the matrix, which is currently never?
            return double.NaN;
        }
    }

    // flips the order of the columns
    // one of the flipped columns should be negated
    // though apparently this is not necessary,
    // (revisit this later)
    //
    // should this return a simple 2d double array?
    public Matrix Mirror()
    {
        int rowLength = Dimensions.Row;

        var result = new double[Dimensions.Row][];

        int columnLength = Dimensions.Column;

        for (int i = 0; i < rowLength; i++)
        {
            // empty column, will add content bellow
            result[i] = new double[columnLength];

            // | ai1 ai2 ... ain-1 ain |
            //
            // becomes
            //
            // | ain ain-1 ... -ai2 -ai1 |
            //
            // (see Habgood (2011) for better description)

            // this is the pivot for the "mirroring" 
            int midPoint;

            int destColumn = columnLength - 1;

            bool skipColumn;
            // if odd number of columns, skip swapping the middle one
            if (columnLength % 2 != 0)
            {
                skipColumn = true;
                midPoint = destColumn / 2; // odd numbers round down
            }
            // if even number of columns, swap all
            else
            {
                skipColumn = false;
                midPoint = (columnLength / 2);
            }

            int sourceColumn;

            for (sourceColumn = 0; sourceColumn < midPoint; sourceColumn++)
            {
                result[i][destColumn] = -matrix[i][sourceColumn];

                destColumn--;
            }

            if (skipColumn)
            {
                // don't swap just copy
                result[i][destColumn] = matrix[i][sourceColumn];
                destColumn--;
                sourceColumn++;
            }

            for (int k = sourceColumn; k < columnLength; k++)
            {
                result[i][destColumn] = matrix[i][k];
                destColumn--;
            }
        }

        return new Matrix(result);
    }

    // add test?
    public static Matrix GetRandomHermitianMatrix(int n)
    {
        var result = new Matrix(n, n);

        var rand = new Random();

        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                var random = rand.NextDouble();
                result.matrix[i][j] = random;

                // Don't want to rewrite with the same random number
                if (i == j)
                    continue;

                // the opposite matrix element, across the diagonal
                // must be the same for a hermitian matrix
                result.matrix[j][i] = random;
            }
        }

        return result;
    }

    // Need to test !
    //
    // Returns an (n x n) Matrix 
    public static Matrix GetRandomSquareMatrix(int n)
    {
        var result = new Matrix(n, n);

        var rand = new Random();

        int loops = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                loops++;
                result.matrix[i][j] = rand.NextDouble();
            }
        }

        return result;
    }

    public bool Add(Matrix rightOperand)
    {
        if (Dimensions != rightOperand.Dimensions)
            return false;

        for (int r = 0; r < Dimensions.Row; r++)
            for (int c = 0; c < Dimensions.Column; c++)

                matrix[r][c] += rightOperand.matrix[r][c];

        return true;
    }

    // left and right don't matter here, commutative
    public static Matrix Add(Matrix leftOperand, Matrix rightOperand)
    {
        if (leftOperand is null || rightOperand is null || (leftOperand.Dimensions != rightOperand.Dimensions))
            throw new ArgumentException("Null or mismatched matricies as parameters");

        var result = Copy(leftOperand);

        result.Add(rightOperand);

        return result;
    }

    /*
        (2,3) * (3,2)
    
        1 3 2    1 2
        2 5 6    0 3
                 7 9

        1*1 + 3*0 + 2*7,  2*1 + 3*3 + 2*9
        2*1 + 5*0 + 6*7,  2*2 + 5*3 + 6*9

        15  29
        44  73
    */
    // left and right matter, not commutative
    public static Matrix? Multiply(Matrix leftOperand, Matrix rightOperand)
    {
        if (leftOperand.Dimensions.Column != rightOperand.Dimensions.Row)
            return null;

        var newMatrix = new Matrix(leftOperand.Dimensions.Row, rightOperand.Dimensions.Column);

        for (int newMatrixRow = 0; newMatrixRow < newMatrix.Dimensions.Row; newMatrixRow++)
        {
            for (int newMatrixColumn = 0; newMatrixColumn < newMatrix.Dimensions.Column; newMatrixColumn++)
            {
                double newElement = 0;

                for (int i = 0; i < leftOperand.Dimensions.Column; i++)
                    newElement += Math.Round(leftOperand.matrix[newMatrixRow][i] * rightOperand.matrix[i][newMatrixColumn], Global.Precision);


                newMatrix.matrix[newMatrixRow][newMatrixColumn] = newElement;
            }
        }

        return newMatrix;
    }

    public static double[] Multiply(Matrix leftOperand, double[] rightOperand, bool swapOperands = false)
    {
        if (swapOperands)
        {
            if (leftOperand.Dimensions.Row != rightOperand.Length)
                return Array.Empty<double>();

            var result = new double[leftOperand.Dimensions.Column];

            for (int i = 0; i < result.Length; i++)
            {
                var sum = 0.0;

                for (int j = 0; j < leftOperand.Dimensions.Row; j++)
                    sum += Math.Round(rightOperand[j] * leftOperand.matrix[j][i], Global.Precision);

                result[i] = sum;
            }

            return result;
        }
        else
        {
            if (leftOperand.Dimensions.Column != rightOperand.Length)
                return Array.Empty<double>();

            var result = new double[leftOperand.Dimensions.Row];

            for (int i = 0; i < result.Length; i++)
            {
                var sum = 0.0;

                for (int j = 0; j < rightOperand.Length; j++)
                    sum += Math.Round(leftOperand.matrix[i][j] * rightOperand[j], Global.Precision);

                result[i] = sum;
            }

            return result;
        }
    }

    /*
        2 1        2 3 5
        3 4        1 4 6
        5 6
    */
    public static Matrix Transpose(Matrix input)
    {
        var result = new Matrix(input.Dimensions.Column, input.Dimensions.Row);

        for (int i = 0; i < input.Dimensions.Row; i++)
            for (int j = 0; j < input.Dimensions.Column; j++)
                result.matrix[j][i] = input.matrix[i][j];

        return result;
    }

    public Matrix Transpose()
    {
        return Transpose(this);
    }

    public string[] ConvertToStringArray()
    {
        var stringArray = new string[matrix.Length + 1];

        var sb = new StringBuilder();

        for (int i = 0; i < this.Dimensions.Row; i++)
        {
            for (int j = 0; j < this.Dimensions.Column; j++)
            {
                sb.Append(this.matrix[i][j]);
                sb.Append(' ');
            }

            stringArray[i] = sb.ToString();
            sb.Clear();
        }

        return stringArray;
    }
}
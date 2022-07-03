using System;
using System.Collections.Generic;
using System.Text;

namespace  Maths.LinearAlgebra;

// This class doesn't allow you to change the dimensions of matrix
// Need to create a new instance to do that
public class Matrix : MatrixBase
{
    public int Precision
    {
        get { return _precision; }
    }
    private static readonly int _precision = Global.Precision;

    // this is
    private Dictionary<string, double> _originalDeterminants;
    
    
    // used for benchmarking
    //private int _2x2_Count = 0;
    //private int _dictionaryCalls = 0;

    // Matrix elements are usually indexed from 1
    // however arrays are 0 indexed, deal with it
    public double[][] matrix;

    // has some bug in it?
    public override bool Equals(object obj)
    {
        if (this == null && obj == null)
            return true;

        else if (this == null || obj == null)
            return false;

        var m2 = obj as Matrix;
        if (m2 == null)
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
    // Matrices are referred to in this row by column (y, x) order
    // same with matrix elements Ayx with A11 as the first element in the top left corner
    public Matrix(int row, int column) : base(row, column)
    {
        this.matrix = new double[row][];

        for (int i = 0; i < row; i++)
            matrix[i] = new double[column];
    }

    // Don't currently check that the inputMatrix has consistently sized arrays
    // double[i][].Length == double[i+1][].Length
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
    // isCramersRule == true && cramersRuleIteration == 0
    // 
    // I only lookup this.Dictionary when 
    // isCramersRule == true && cramersRuleIteration != 0
    public double GetDeterminant(bool isCramersRule = false, int cramersRuleIteration = 0)
    {
        if (this.matrix == null || !this.IsSquare)
            return double.NaN;

        if(this.Dimensions.Column == 1)
           return this.matrix[0][0];

        if(this.Dimensions.Column == 2)
           return  Math.Round(this.matrix[0][0]*this.matrix[1][1], _precision) - Math.Round(this.matrix[0][1]*this.matrix[1][0], _precision);
     
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

        //_dictionaryCalls = 0;


        // dictionary is growing at 2^n for first pass
        // then 2^(n-1) for each subsequent GetDeterminant() call
        //
        // total 2^(n+1) for entire Cramers algorithm

        var power = this.Dimensions.Column;

        if (cramersRuleIteration > 0)
            power--;

        var dictionarySizeBound = Math.Pow(2, power);

        Dictionary<string, double> calculatedDeterminants = new Dictionary<string, double>(Convert.ToInt32(dictionarySizeBound));
        
        var allowed = new List<int>();
        for(int c = 1; c <= this.Dimensions.Column; c++)
            allowed.Add(c);

        // using regular matrix coordinate notation
        for (int i = 1; i <= this.Dimensions.Column; i++)
        {
            double minorDet;

            if (isCramersRule)
            {
                if (cramersRuleIteration == 0)
                {
                    // populate this.dictionary (store all calculations from determinant of orignal A)
                    // ie before subsequent calls with modified A 
                    minorDet = getMinorDeterminant(1, i, calculatedDeterminants, allowed);
                    _originalDeterminants = calculatedDeterminants;
                }
                // only once per Dj.GetDeterminant in CramersRule
                // the function getSubMatrix() will find a repeat calculation from the intitial getDet()
                else
                {
                    minorDet = getMinorDeterminant(1, i, calculatedDeterminants, allowed, cramersRuleIteration);
                }
            }
            else
            {
                // ignore dictionary associated with this object
                // find answer using local dictionary which is not persisted
                minorDet = getMinorDeterminant(1, i, calculatedDeterminants, allowed);
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

        //Console.WriteLine("Theoretical calls for Laplace expansion:" + count);

        return determinant;
    }

    // two modes
    // one without a special column (specialColumn == 0)
    // one with a special column (0 < specialColumn <= n)
    //      this means I check the common dictionary this.Dictionary
    private double getMinorDeterminant(int row, int column, Dictionary<string, double> calculations, List<int> allowedColumns, int modfiedColumn = 0)
    {
        // Is it worth using a list?
        // In this function I use:
        //  -   Contains(int)
        //  -   Remove(int)
        //  -   Add(int)
        //  -   Sort()

        if ((row <= 0) || (column <= 0) || (row > this.Dimensions.Row) || (column > this.Dimensions.Column))
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
                    var mult = this.matrix[row][allowedColumns[i] - 1] * getMinorDeterminant(row + 1, allowedColumns[i], calculations, allowedColumns, modfiedColumn);

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
        else if (row == this.matrix.Length - 2)
        {
            allowedColumns.Remove(column);

            // at this point, only 2 columns should be allowed
            if (allowedColumns.Count !=  2)
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
            for (int j = i; j < n; j++)
            {
                loops++;
                result.matrix[i][j] = rand.NextDouble();

                // Don't want to overwrite with another random number
                if (i == j)
                {
                    continue;
                }

                result.matrix[j][i] = rand.NextDouble();
            }
        }
        
        return result;
    }

    public bool Add(Matrix rightOperand)
    {
        if (this.Dimensions != rightOperand.Dimensions)
            return false;

        for (int r = 0; r < this.Dimensions.Row; r++)
            for (int c = 0; c < this.Dimensions.Column; c++)

                this.matrix[r][c] += rightOperand.matrix[r][c];

        return true;
    }

    // left and right don't matter here, commutative
    public static Matrix Add(Matrix leftOperand, Matrix rightOperand)
    {
        if (leftOperand == null || rightOperand == null || (leftOperand.Dimensions != rightOperand.Dimensions) )
            return null;

        var result = Matrix.Copy(leftOperand);

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
    public static Matrix Multiply(Matrix leftOperand, Matrix rightOperand)
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
                    newElement = newElement + Math.Round(leftOperand.matrix[newMatrixRow][i]*rightOperand.matrix[i][newMatrixColumn], _precision);


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
                return null;

            var result = new double[leftOperand.Dimensions.Column];

            for (int i = 0; i < result.Length; i++)
            {
                var sum = 0.0;

                for (int j = 0; j < leftOperand.Dimensions.Row; j++)
                    sum = sum + Math.Round(rightOperand[j]*leftOperand.matrix[j][i], _precision);

                result[i] = sum;
            }

            return result;
        }
        else
        {
            if (leftOperand.Dimensions.Column != rightOperand.Length)
                return null;

            var result = new double[leftOperand.Dimensions.Row];

            for (int i = 0; i < result.Length; i++)
            {
                var sum = 0.0;

                for (int j = 0; j < rightOperand.Length; j++)
                    sum = sum + Math.Round(leftOperand.matrix[i][j]*rightOperand[j], _precision);

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
                sb.Append(" ");
            }

            stringArray[i] = sb.ToString();
            sb.Clear();
        }

        return stringArray;
    }
}
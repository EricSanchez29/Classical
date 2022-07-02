using System;

namespace Maths.LinearAlgebra;

public class MatrixBase
{
    public bool IsSquare
    {
        get { return _isSquare; }
    }

    // should only be set in constructor
    private readonly bool _isSquare;

    // Example call: double number = obj.matrix[i][j];
    // I can't guarantee that one array, i, will not be accidentally resized;
    // so I need a reference number of columns in order to determine errors
    public (int Row, int Column) Dimensions
    {
        get { return _dimensions; }
    }

    // Matrices are referred to as, "row by column" or (r x c)
    private readonly (int row, int column) _dimensions;

    public MatrixBase(int row, int column)
    {
        if (row <= 0)
        {
            throw new ArgumentException(nameof(row), "Matrix size cannot be less than 1");
        }

        if (column <= 0)
        {
            throw new ArgumentException(nameof(column), "Matrix size cannot be less than 1");
        }

        if (column == row)
            _isSquare = true;

        _dimensions = new (row, column);
    }
}
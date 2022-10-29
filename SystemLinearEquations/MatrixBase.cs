namespace Maths.LinearAlgebra;

public class MatrixBase
{
    public bool IsSquare
    {
        get { return _isSquare; }
    }

    private readonly bool _isSquare;

    // Example call: double number = obj.matrix[i][j];
    // I can't guarantee that the ith array, will not be accidentally resized;
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
            throw new ArgumentException("Matrix size cannot be less than 1", nameof(row));

        if (column <= 0)
            throw new ArgumentException("Matrix size cannot be less than 1", nameof(column));

        if (column == row)
            _isSquare = true;

        _dimensions = new (row, column);
    }
}
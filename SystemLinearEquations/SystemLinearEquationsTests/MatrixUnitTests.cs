using Maths.LinearAlgebra;

namespace MathTests.LinearAlgebra;

public static class MatrixUnitTests
{
    [Fact]
    public static void TestGetDeterminant()
    {
        // Got the correct answers from Wolfram Alpha and other online calculators 

        // Arrange
        var testMatrix1 = Matrix.GetIdentityMatrix(2);

        // Act
        var result1 = testMatrix1.GetDeterminant();

        // Assert
        Assert.Equal(1, result1);
        
        /// 2 0
        /// 3 0
        // Arrange
        var testMatrix2 = new Matrix(2, 2);
        testMatrix2.matrix[0][0] = 2;
        testMatrix2.matrix[0][1] = 0;
        testMatrix2.matrix[1][0] = 3;
        testMatrix2.matrix[1][1] = 0;

        // Act
        var result2 = testMatrix2.GetDeterminant();

        // Assert
        Assert.Equal(0, result2);

        /// 2 3
        /// 3 2
        // Arrange
        testMatrix2.matrix[0] = new double[] { 2, 3 };
        testMatrix2.matrix[1] = new double[] { 3, 2 };

        // Act
        result2 = testMatrix2.GetDeterminant();

        // Assert
        Assert.Equal(-5, result2);
        
        /// 3x3 (identity)
        // Arrange
        var testMatrix3 = Matrix.GetIdentityMatrix(3);

        // Act
        var result3 = testMatrix3.GetDeterminant();

        // Assert
        Assert.Equal(1, result3);
        

        /// 3x3
        // Arrange
        testMatrix3.matrix[0] = new double[] { 1, 2, 3 };
        testMatrix3.matrix[1] = new double[] { 2, 4, 5 };
        testMatrix3.matrix[2] = new double[] { 3, 8, 9 };

        // Act
        result3 = testMatrix3.GetDeterminant();

        // Assert
        Assert.Equal(2, result3);


        /// 4x4
        // Arrange
        var testMatrix4 = new Matrix(4, 4);
        testMatrix4.matrix[0] = new double[] { 1, 2, 3, 4 };
        testMatrix4.matrix[1] = new double[] { 2, 4, 5, 7 };
        testMatrix4.matrix[2] = new double[] { 3, 8, 9, 3 };
        testMatrix4.matrix[3] = new double[] { 1, 8, 9, 1 };

        // Act
        var result4 = testMatrix4.GetDeterminant();

        // Assert
        Assert.Equal(36, result4);


        /// 5x5
        // Arrange
        var testMatrix5 = new Matrix(5, 5);
        testMatrix5.matrix[0] = new double[] { 1, 2, 3, 4, 5 };
        testMatrix5.matrix[1] = new double[] { 2, 2, 4, 8, 10 };
        testMatrix5.matrix[2] = new double[] { 1, 3, 3, 4, 1 };
        testMatrix5.matrix[3] = new double[] { 8, 9, 0, 4, 6 };
        testMatrix5.matrix[4] = new double[] { 1, 1, 3, 2, 5 };

        // Act
        var result5 = testMatrix5.GetDeterminant();

        // Assert
        Assert.Equal(360, result5);


        /// 8x8
        // Arrange
        var testMatrix8 = new Matrix(8, 8);
        testMatrix8.matrix[0] = new double[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        testMatrix8.matrix[1] = new double[] { 2, 2, 4, 8, 1, 6, 3, 2 };
        testMatrix8.matrix[2] = new double[] { 1, 3, 3, 4, 1, 5, 6, 7 };
        testMatrix8.matrix[3] = new double[] { 8, 9, 0, 4, 6, 3, 4, 6 };
        testMatrix8.matrix[4] = new double[] { 4, 3, 3, 4, 5, 0, 7, 8 };
        testMatrix8.matrix[5] = new double[] { 2, 0, 4, 1, 1, 6, 3, 6 };
        testMatrix8.matrix[6] = new double[] { 9, 3, 3, 4, 1, 5, 6, 2 };
        testMatrix8.matrix[7] = new double[] { 8, 9, 3, 4, 6, 5, 4, 6 };

        // Act
        var result8 = testMatrix8.GetDeterminant();

        // Assert
        Assert.Equal(-823099, result8);

        /// 10x10
        // Arrange
        var testMatrix10 = new Matrix(10, 10);
        testMatrix10.matrix[0] = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
        testMatrix10.matrix[1] = new double[] { 2, 2, 4, 8, 1, 6, 3, 2, 6, 1 };
        testMatrix10.matrix[2] = new double[] { 1, 3, 3, 4, 1, 5, 6, 7, 1, 5 };
        testMatrix10.matrix[3] = new double[] { 8, 9, 0, 4, 6, 3, 4, 6, 7, 9 };
        testMatrix10.matrix[4] = new double[] { 4, 3, 3, 4, 5, 0, 7, 8, 4, 1 };
        testMatrix10.matrix[5] = new double[] { 2, 0, 4, 1, 1, 6, 3, 6, 5, 6 };
        testMatrix10.matrix[6] = new double[] { 9, 3, 3, 4, 1, 5, 6, 2, 2, 1 };
        testMatrix10.matrix[7] = new double[] { 8, 9, 3, 4, 7, 5, 4, 6, 8, 0 };
        testMatrix10.matrix[8] = new double[] { 3, 3, 7, 1, 1, 5, 6, 2, 3, 1 };
        testMatrix10.matrix[9] = new double[] { 3, 1, 3, 4, 6, 3, 1, 6, 8, 1 };

        // Act
        var result10 = testMatrix10.GetDeterminant();

        // Assert
        Assert.Equal(-87258626, result10);

        /// 11x11
        // Arrange
        var testMatrix11 = new Matrix(11, 11);
        testMatrix11.matrix[0] = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 1 };
        testMatrix11.matrix[1] = new double[] { 2, 2, 4, 8, 1, 6, 3, 2, 6, 1, 2 };
        testMatrix11.matrix[2] = new double[] { 1, 3, 3, 4, 1, 5, 6, 7, 1, 5, 3 };
        testMatrix11.matrix[3] = new double[] { 8, 9, 0, 4, 6, 3, 4, 6, 7, 9, 4 };
        testMatrix11.matrix[4] = new double[] { 4, 3, 3, 4, 5, 0, 7, 8, 4, 1, 5 };
        testMatrix11.matrix[5] = new double[] { 2, 0, 4, 1, 1, 6, 3, 6, 5, 6, 6 };
        testMatrix11.matrix[6] = new double[] { 9, 3, 3, 4, 1, 5, 6, 2, 2, 1, 7 };
        testMatrix11.matrix[7] = new double[] { 8, 9, 3, 4, 7, 5, 4, 6, 8, 0, 8 };
        testMatrix11.matrix[8] = new double[] { 3, 3, 7, 1, 1, 5, 6, 2, 3, 1, 9 };
        testMatrix11.matrix[9] = new double[] { 3, 1, 3, 4, 6, 3, 1, 6, 8, 1, 0 };
        testMatrix11.matrix[10] = new double[] { 3, 3, 1, 1, 1, 5, 6, 2, 3, 1, 9 };

        // Act
        var result11 = testMatrix11.GetDeterminant();

        // Assert
        Assert.Equal(-571432686, result11);


        /// 12x12
        // Arrange
        var testMatrix12 = new Matrix(12, 12);
        testMatrix12.matrix[0] = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 1, 0 };
        testMatrix12.matrix[1] = new double[] { 2, 2, 4, 8, 1, 6, 3, 2, 6, 1, 2, 9 };
        testMatrix12.matrix[2] = new double[] { 1, 3, 3, 4, 1, 5, 6, 7, 1, 5, 3, 8 };
        testMatrix12.matrix[3] = new double[] { 8, 9, 0, 4, 6, 3, 4, 6, 7, 9, 4, 7 };
        testMatrix12.matrix[4] = new double[] { 4, 3, 3, 4, 5, 0, 7, 8, 4, 1, 5, 6 };
        testMatrix12.matrix[5] = new double[] { 2, 0, 4, 1, 1, 6, 3, 6, 5, 6, 6, 5 };
        testMatrix12.matrix[6] = new double[] { 9, 3, 3, 4, 1, 5, 6, 2, 2, 1, 7, 4 };
        testMatrix12.matrix[7] = new double[] { 8, 9, 3, 4, 7, 5, 4, 6, 8, 0, 8, 3 };
        testMatrix12.matrix[8] = new double[] { 3, 3, 7, 1, 1, 5, 6, 2, 3, 1, 9, 2 };
        testMatrix12.matrix[9] = new double[] { 3, 1, 3, 4, 6, 3, 1, 6, 8, 1, 0, 1 };
        testMatrix12.matrix[10] = new double[] { 3, 3, 1, 1, 1, 5, 6, 2, 3, 1, 9, 3 };
        testMatrix12.matrix[11] = new double[] { 9, 1, 3, 4, 6, 3, 7, 6, 8, 1, 0, 7 };

        // Act
        var result12 = testMatrix12.GetDeterminant();

        //Assert
        Assert.Equal(-6022880484, result12);
    }

    [Fact]
    public static void TestCalculateMinor()
    {
        /// 3x3
        // Arrange
        var testMatrix3 = new Matrix(3, 3);
        testMatrix3.matrix[0] = new double[] { 1, 2, 3 };
        testMatrix3.matrix[1] = new double[] { 2, 4, 5 };
        testMatrix3.matrix[2] = new double[] { 3, 8, 9 };

        var expected3 = new Matrix(2, 2);
        expected3.matrix[0] = new double[] { 1, 2 };
        expected3.matrix[1] = new double[] { 2, 4 };

        // Act
        var result3 = testMatrix3.CalculateMinor(3, 3);

        // Assert
        Assert.Equal(expected3.GetDeterminant(), result3);

        // Arrange
        expected3.matrix[0] = new double[] { 2, 3 };
        expected3.matrix[1] = new double[] { 4, 5 };

        // Act
        result3 = testMatrix3.CalculateMinor(3, 1);

        // Assert
        Assert.Equal(expected3.GetDeterminant(), result3);

        /// 4x4
        // Arrange
        var testMatrix4 = new Matrix(4, 4);
        testMatrix4.matrix[0] = new double[] { 1, 2, 3, 4 };
        testMatrix4.matrix[1] = new double[] { 2, 4, 5, 7 };
        testMatrix4.matrix[2] = new double[] { 3, 6, 9, 3 };
        testMatrix4.matrix[3] = new double[] { 4, 8, 2, 1 };

        var expected4 = new Matrix(3, 3);
        expected4.matrix[0] = new double[] { 4, 5, 7 };
        expected4.matrix[1] = new double[] { 6, 9, 3 };
        expected4.matrix[2] = new double[] { 8, 2, 1 }; 

        // Act
        var result4 = testMatrix4.CalculateMinor(1, 1);

        // Assert
        Assert.Equal(expected4.GetDeterminant(), result4);

        // Arrange
        expected4 = new Matrix(3, 3);
        expected4.matrix[0] = new double[] { 2, 3, 4 };
        expected4.matrix[1] = new double[] { 4, 5, 7 };
        expected4.matrix[2] = new double[] { 6, 9, 3 };

        // Act
        result4 = testMatrix4.CalculateMinor(4, 1);

        // Assert
        Assert.Equal(expected4.GetDeterminant(), result4);


        // Arrange
        expected4 = new Matrix(3, 3);
        expected4.matrix[0] = new double[] { 2, 4, 5 };
        expected4.matrix[1] = new double[] { 3, 6, 9 };
        expected4.matrix[2] = new double[] { 4, 8, 2 };

        // Act
        result4 = testMatrix4.CalculateMinor(1, 4);

        // Assert
        Assert.Equal(expected4.GetDeterminant(), result4);

        // Arrange
        expected4 = new Matrix(3, 3);
        expected4.matrix[0] = new double[] { 1, 2, 3 };
        expected4.matrix[1] = new double[] { 2, 4, 5 };
        expected4.matrix[2] = new double[] { 3, 6, 9 };

        // Act
        result4 = testMatrix4.CalculateMinor(4, 4);

        // Assert
        Assert.Equal(expected4.GetDeterminant(), result4);

        expected4 = new Matrix(3, 3);
        expected4.matrix[0] = new double[] { 1, 2, 4 };
        expected4.matrix[1] = new double[] { 2, 4, 7 };
        expected4.matrix[2] = new double[] { 4, 8, 1 };

        // Act
        result4 = testMatrix4.CalculateMinor(3, 3);

        // Assert
        Assert.Equal(expected4.GetDeterminant(), result4);
    }

    [Fact]
    public static void TestMirror()
    {
        // Arrange
        var test3 = new Matrix(3, 3);
        test3.matrix[0] = new double[] { 2, 5, 7 };
        test3.matrix[1] = new double[] { 3, 9, 6 };
        test3.matrix[2] = new double[] { 1, 9, 2 };

        var expected3 = new Matrix(3, 3);
        expected3.matrix[0] = new double[] { 7, 5, -2 };
        expected3.matrix[1] = new double[] { 6, 9, -3 };
        expected3.matrix[2] = new double[] { 2, 9, -1 };

        // Act
        var result3 = test3.Mirror();

        // Assert
        Assert.Equal(expected3, result3);

        // Arrange
        var testMatrix4 = new Matrix(4, 4);
        testMatrix4.matrix[0] = new double[] { 1, 2, 3, 4 };
        testMatrix4.matrix[1] = new double[] { 2, 4, 5, 7 };
        testMatrix4.matrix[2] = new double[] { 3, 8, 9, 6 };
        testMatrix4.matrix[3] = new double[] { 5, 8, 9, 1 };

        var expected4 = new Matrix(4, 4);
        expected4.matrix[0] = new double[] { 4, 3, -2, -1 };
        expected4.matrix[1] = new double[] { 7, 5, -4, -2 };
        expected4.matrix[2] = new double[] { 6, 9, -8, -3 };
        expected4.matrix[3] = new double[] { 1, 9, -8, -5 };

        // Act
        var result4 = testMatrix4.Mirror();

        // Assert
        Assert.Equal(expected4, result4);
    }

    [Fact]
    public static void TestGetIdentity()
    {
        // Arrange & Act
        Matrix testMatrix = Matrix.GetIdentityMatrix(5);

        // Assert
        Assert.True(testMatrix.IsSquare);

        for (int i = 0; i < testMatrix.Dimensions.Column; i++)
        {
            Assert.Equal(1, testMatrix.matrix[i][i]);

            for (int j = 0; j < testMatrix.Dimensions.Row && j != i; j++)
            {
                Assert.Equal(0, testMatrix.matrix[i][j]);
            }
        }
    }

    [Fact]
    public static void TestAdd()
    {
        // Arrange
        var testM1 = new Matrix(2, 2);
        testM1.matrix[0] = new double[] { 1, 2 };
        testM1.matrix[1] = new double[] { 4, 3 };

        var testM2 = new Matrix(2, 2);
        testM2.matrix[0] = new double[] { 1, 5 };
        testM2.matrix[1] = new double[] { 7, 3 };

        //  Matrix with different dimensions
        var testM3 = new Matrix(2, 3);
        testM3.matrix[0] = new double[] { 1, 2, 3 };
        testM3.matrix[1] = new double[] { 4, 3, 2 };

        var expectedM1 = new Matrix(2, 2);
        expectedM1.matrix[0] = new double[] { 2, 7 };
        expectedM1.matrix[1] = new double[] { 11, 6 };

        // Act
        // checking that mismatched dimensions don't work
        var result1 = testM1.Add(testM3);
        var result2 = testM1.Add(testM2);

        // Assert
        Assert.False(result1);
        Assert.True(result2);
        Assert.Equal(expectedM1, testM1);
    }

    [Fact]
    public static void TestMultiply()
    {
        // Arrange
        var leftMatrix1 = new Matrix(1, 3);
        leftMatrix1.matrix[0] = new double[] { 1, 2, 3 };

        var rightMatrix1 = new Matrix(3, 2);
        rightMatrix1.matrix[0] = new double[] { 1, 2 };
        rightMatrix1.matrix[1] = new double[] { 3, 4 };
        rightMatrix1.matrix[2] = new double[] { 5, 6 };

        var matrix = new Matrix(1, 2);
        matrix.matrix[0] = new double[] { 22, 28 };

        // Act
        var result1 = Matrix.Multiply(rightMatrix1, leftMatrix1);

        // Assert
        /// Dimensions aren't correct so should fail
        Assert.Null(result1);

        // Act
        var result2 = Matrix.Multiply(leftMatrix1, rightMatrix1);

        // Assert
        Assert.Equal((1, 2), result2?.Dimensions);

        Assert.Equal(matrix, result2);


        // Arrange
        var leftMatrix2 = new Matrix(2, 3);
        leftMatrix2.matrix[0] = new double[] { 1, 3, 2 };
        leftMatrix2.matrix[1] = new double[] { 2, 5, 6 };

        var rightMatrix2 = new Matrix(3, 2);
        rightMatrix2.matrix[0] = new double[] { 1, 2 };
        rightMatrix2.matrix[1] = new double[] { 0, 3 };
        rightMatrix2.matrix[2] = new double[] { 7, 9 };

        var matrix2 = new Matrix(2, 2);
        matrix2.matrix[0] = new double[] { 15, 29 };
        matrix2.matrix[1] = new double[] { 44, 73 };

        // Act
        var result3 = Matrix.Multiply(leftMatrix2, rightMatrix2);

        // Assert
        Assert.Equal(matrix2, result3);


        /// reusing rightMatrix1
        //  1, 2
        //  3, 4
        //  5, 6

        //  7, 8, 9   1, 2
        //            3, 4
        //            5, 6

        //       1, 2  3
        //             4
        //       
        // Arrange
        var vector = new double[] { 1, 2, 3 };

        var columnVector = new double[] { 1, 2 };

        /// [3x2]*[3] is not possible
        // Act
        var result = Matrix.Multiply(rightMatrix1, vector);

        // Assert
        Assert.Empty(result);

        // Arrange
        // A*v
        // [3x2]*[2] returns a [3] vector
        var expected = new double[] { 5, 11, 17 };

        // Act
        result = Matrix.Multiply(rightMatrix1, columnVector);

        // Assert
        Assert.Equal(expected, result);

        // Arrange
        // v*A
        // [3]*[3x2] returns a [2] vector
        var expectedVector = new double[] { 22, 28 };

        // Act
        result = Matrix.Multiply(rightMatrix1, vector, true);

        // Assert
        Assert.Equal(expectedVector, result);

        //public static Matrix Multiply(double scalar, Matrix matrix)
    }

    [Fact]
    public static void TestTranspose()
    {
        // Arrange
        var testM1 = new Matrix(1, 3);
        testM1.matrix[0] = new double[] { 1, 2, 3 };

        var resultM1 = new Matrix(3, 1);
        resultM1.matrix[0] = new double[] { 1 };
        resultM1.matrix[1] = new double[] { 2 };
        resultM1.matrix[2] = new double[] { 3 };

        // Act
        testM1 = testM1.Transpose();

        // Assert
        Assert.Equal(testM1, resultM1);


        // Arrange
        var testM2 = new Matrix(3, 3);
        testM2.matrix[0] = new double[] { 1, 2, 3 };
        testM2.matrix[1] = new double[] { 4, 5, 6 };
        testM2.matrix[2] = new double[] { 7, 8, 9 };

        var expected = new Matrix(3, 3);
        expected.matrix[0] = new double[] { 1, 4, 7 };
        expected.matrix[1] = new double[] { 2, 5, 8 };
        expected.matrix[2] = new double[] { 3, 6, 9 };

        // Act
        testM2 = testM2.Transpose();

        // Assert
        Assert.Equal(expected, testM2);
    }

    [Fact]
    public static void TestGetRandomHermitianMatrix()
    {
        var rando = Matrix.GetRandomHermitianMatrix(5);

        // H = H ^ t
        Assert.True(rando.Transpose().Equals(rando));
    }

    [Fact]
    public static void TestGetRandomSquareMatrix()
    {
        var rando = Matrix.GetRandomSquareMatrix(5);

        Assert.True(rando.IsSquare);
    }
}

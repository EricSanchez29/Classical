using Maths.LinearAlgebra;
using Maths;

namespace MathTests.LinearAlgebra;

public class LinearSystemUnitTests
{
    [Fact]
    public static void TestCramersRule_Modified()
    {
        /// 3x3 (identity)
        // Arrange
        var m1 = Matrix.GetIdentityMatrix(3);
        var b1 = new double[3] { 1, 1, 1 };
        var expectedResult = new double[3] { 1, 1, 1 };

        // Act
        var result1 = LinearSystemAlgorithms.CramersRuleModified(m1, b1);

        // Assert
        Assert.Equal(expectedResult, result1);
        
        /// 3x3
        // Arrange
        m1 = Matrix.GetIdentityMatrix(3);
        m1.matrix[0] = new double[] { 1, 2, 3 };
        m1.matrix[1] = new double[] { 4, 5, 6 };
        m1.matrix[2] = new double[] { 7, 9, 9 };
        var b2 = new double[3] { 1, 5, 10 };
        var expectedResult2 = new double[3] { 1, 1, -Math.Round((2.0/3.0), Global.Precision) };

        // Act
        var result2 = LinearSystemAlgorithms.CramersRuleModified(m1, b2);

        // Assert
        Assert.Equal(expectedResult2, result2);

        /// 4x4
        // Arrange
        var m4 = new Matrix(4, 4);
        m4.matrix[0] = new double[] { 2, 4, 6, 8 };
        m4.matrix[1] = new double[] { 2, 6, 9, 0 };
        m4.matrix[2] = new double[] { 1, 5, 9, 3 };
        m4.matrix[3] = new double[] { 3, 1, 2, 6 };
        var b4 = new double[4] { 1, 2, 3, 4 };

        // from wolfram alpha
        var expectedResult3 = VectorAlgebra.Round(new double[4] { 247.0/178.0, -509.0 / 178.0, 162.0 / 89.0, -14.0 / 89.0 }, 15);

        // Act
        var result4 = LinearSystemAlgorithms.CramersRuleModified(m4, b4);

        // Assert
        Assert.Equal(expectedResult3, result4);
    }

    [Fact]
    public static void TestConjugateGradientMethod()
    {
        /// Only using Hermitian matricies so only exercising 
        /// conjugateGradientMethod()

        // Largest allowed percent difference in length
        var tolerance = 1.0;

        /// 2x2 (identity)
        // Arrange
        var m0 = Matrix.GetIdentityMatrix(2);
        var b0 = new double[2] { 1, 1 };
        var est0 = new double[2] { 0.5, 0.5 };
        var expectedResult0 = new double[2] { 1, 1 };

        // Act
        var result = LinearSystemAlgorithms.ConjugateTransposeMethod(m0, b0, est0);

        // Assert
        Assert.Equal(expectedResult0, result);


        // Arrange
        m0.matrix[0] = new double[2] { 4, 1 };
        m0.matrix[1] = new double[2] { 1, 3 };
        b0 = new double[2] { 1, 2 };
        est0 = new double[2] { 2, 1 };
        expectedResult0 = new double[2] { 0.090909090909091, 0.636363636363636 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m0, b0, est0);

        // Assert
        Assert.Equal(expectedResult0, result);


        /// 3x3 (identity)
        // Arrange
        var m1 = Matrix.GetIdentityMatrix(3);
        var b1 = new double[3] { 1, 1, 1 };
        var est1 = new double[3] { 1, 1, 0.1 };
        var expectedResult = new double[3] { 1, 1, 1 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m1, b1, est1);

        // Assert
        // Get exact answer with identity matricies
        Assert.Equal(expectedResult, result);


        // Arrange
        m1.matrix[0] = new double[] { 1, 4, 0};
        m1.matrix[1] = new double[] { 4, 5, 8 };
        m1.matrix[2] = new double[] { 0, 8, 9 };
        var b2 = new double[3] { 1, 5, 10 };
        est1 = new double[3] { 4, 2, -1 };
        expectedResult = new double[3] { -0.74233, 0.43558, 0.72393 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m1, b2, est1);
        var lengthDiff = VectorAlgebra.PercentDiffLength(result, expectedResult);

        // Assert
        Assert.NotNull(result);

        Assert.InRange(Math.Abs(lengthDiff), 0, tolerance);

        // Arrange
        m1.matrix[0] = new double[] { 1, 2, 3 };
        m1.matrix[1] = new double[] { 2, 5, 0 };
        m1.matrix[2] = new double[] { 3, 0, 9 };
        b2 = new double[3] { 10, 2, 3 };
        est1 = new double[3] { -10, 5, 4 };
        var expectedResult2 = new double[3] { -10.25, 4.5, 3.75 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m1, b2, est1);
 
        lengthDiff = VectorAlgebra.PercentDiffLength(expectedResult2, result);


        // Assert
        Assert.InRange(Math.Abs(lengthDiff), 0, tolerance);


        // Arrange
        var m4 = Matrix.GetIdentityMatrix(4);
        var b4 = new double[4] { 7, 6, 9, 1 };
        var g4 = new double[4] { 7, 3, 10, 1 };
        var expected4 = new double[4] { 7, 6, 9, 1 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m4, b4, g4);

        lengthDiff = VectorAlgebra.PercentDiffLength(result, expected4);

        // Assert
        Assert.InRange(Math.Abs(lengthDiff), 0, tolerance);


        // Arrange
        m4.matrix[0] = new double[] { 2, 0, 0, 8 };
        m4.matrix[1] = new double[] { 0, 6, 9, 0 };
        m4.matrix[2] = new double[] { 0, 9, 9, 0 };
        m4.matrix[3] = new double[] { 8, 0, 0, 6 };
        g4 = new double[4] { 1, -3, 3, -0.1 };
        b4 = new double[4] { 1, 1, 1, 3 };
        var expectedResult3 = new double[4] { 0.34615, 0, 0.11111, 0.038462 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m4, b4, g4);

        lengthDiff = VectorAlgebra.PercentDiffLength(result, expectedResult3);

        // Assert
        Assert.InRange(Math.Abs(lengthDiff), 0, tolerance);
    }

    [Fact]
    public static void TestBiconjugateMethod()
    {
        /// Only using non-Hermitian matricies so only exercising 
        /// biconjugateGradient()

        // Largest allowed percent difference in length
        var tolerance = 1.0;

        /// 2x2
        // Arrange
        var m2 = new Matrix(2, 2);
        m2.matrix[0] = new double[2] { 4, 2 };
        m2.matrix[1] = new double[2] { 1, 3 };
        var b = new double[2] { 1, 2 };
        var guess = new double[2] { 2, 1 };
        var expectedResult = new double[2] { -0.1, 0.7 };

        // Act
        var result = LinearSystemAlgorithms.ConjugateTransposeMethod(m2, b, guess);

        // Assert
        Assert.Equal(expectedResult, result);


        /// 3x3 
        // Arrange
        var m3 = new Matrix(3, 3);
        m3.matrix[0] = new double[] { 1, 2, 3 };
        m3.matrix[1] = new double[] { 4, 5, 6 };
        m3.matrix[2] = new double[] { 7, 0, 9 };
        b = new double[3] { 10, 7, 3 };
        guess = new double[3] { 1, 2, 3 };
        expectedResult = new double[3] { -6.5625, 0.125, 5.4375 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m3, b, guess);
        var lengthDiff = VectorAlgebra.PercentDiffLength(result, expectedResult);

        // Assert
        Assert.NotNull(result);

        Assert.InRange(Math.Abs(lengthDiff), 0, tolerance);


        // Arrange
        var m4 = new Matrix(4, 4);
        m4.matrix[0] = new double[] { 2, 4, 6, 8 };
        m4.matrix[1] = new double[] { 2, 6, 9, 0 };
        m4.matrix[2] = new double[] { 1, 5, 9, 3 };
        m4.matrix[3] = new double[] { 3, 1, 2, 6 };
        b = new double[4] { 1, 2, 3, 4 };
        guess = new double[4] { 1, 2, 3, 4 };
        expectedResult = new double[4] { 1.3876, -2.8596, 1.8202, -0.15730 };

        // Act
        result = LinearSystemAlgorithms.ConjugateTransposeMethod(m4, b, guess);

        lengthDiff = VectorAlgebra.PercentDiffLength(result, expectedResult);

        // Assert
        Assert.InRange(Math.Abs(lengthDiff), 0, tolerance);
    }
}

using Maths.LinearAlgebra;

namespace MathTests.LinearAlgebra;
public class VectorUnitTests
{
    [Fact]
    public static void VectorOperatorsTests()
    {
        var a = new double[] {1, 2, 3};
        var b = new double[] {2, 2, 2};
        var result1 = Vector.DotProduct(a, b);
        var expected1 = 12;
        Assert.Equal(expected1, result1);

        // confirm commutativity
        var result2 = Vector.DotProduct(b, a);
        Assert.Equal(expected1, result2);
            
        var result3 = Vector.CrossProduct(a, b);
        var expected3 = new double[] {-2, 4, -2};
        Assert.Equal(expected3, result3);

        // confirm non commutativity
        var result4 = Vector.CrossProduct(b, a);
        var expected4 = new double[] {2, -4, 2};
        Assert.Equal(expected4, result4);

        // Angle between two vectors
        var expected5 = 0.3876; // radians
        Assert.Equal(expected5, Math.Round(Vector.GetAngle(a, b), 4));
    }

    [Fact]
    public static void VectorAlgebraTests()
    {
        // Arrange
        var a = new double[] {1, 2, 3 };
        var b = new double[] {1, 1, -1};
        double c = 3;

        var expected1 = new double[] {2, 3, 2};
        var expected2 = new double[] {0, 1, 4};
        var expected3 = new double[] {3, 6, 9};

        // Act & Assert
        Assert.Equal(expected1, Vector.Add(a, b));
        Assert.Equal(expected2, Vector.Subtract(a, b));
        Assert.Equal(expected3, Vector.Multiply(c, a));
    }


    [Fact]
    public static void RandomVectorTest()
    {
        // Arrange
        var b = Vector.GetRandomVector(3);
        var c = Vector.GetRandomVector(3);

        // Act & Assert
        // Testing that random vectors are different
        Assert.NotEqual(c, b);

    }

    [Fact]
    public static void ToStringTest()
    {
        // Arrange
        var a = new double[] { 1, 1, 1 };
        var expected1 = "<1,1,1>";

        // Act & Assert
        Assert.Equal(expected1, Vector.ToString(a));
    }


    [Fact]
    public static void PercentDiffLengthTest()
    {
        // Arrange
        var d = new double[] {1, 1, 1, 1}; // length = 2
        var e = new double[] {2, 2, 2, 2}; // length = 4
        var expected2 = 100.0;

        // Act
        var x = Vector.PercentDiffLength(e, d);

        // Asser
        Assert.Equal(expected2, x);
    }
}
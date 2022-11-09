using Maths.LinearAlgebra;

namespace MathTests.LinearAlgebra;
public class VectorAlgebraUnitTests
{
    [Fact]
    public static void VectorOperatorsTests()
    {
        var a = new double[] {1, 2, 3};
        var b = new double[] {2, 2, 2};
        var result1 = VectorAlgebra.DotProduct(a, b);
        var expected1 = 12;
        Assert.Equal(expected1, result1);

        // confirm commutativity
        var result2 = VectorAlgebra.DotProduct(b, a);
        Assert.Equal(expected1, result2);
            
        var result3 = VectorAlgebra.CrossProduct(a, b);
        var expected3 = new double[] {-2, 4, -2};
        Assert.Equal(expected3, result3);

        // confirm non commutativity
        var result4 = VectorAlgebra.CrossProduct(b, a);
        var expected4 = new double[] {2, -4, 2};
        Assert.Equal(expected4, result4);

        // Angle between two vectors
        var expected5 = 0.3876; // radians
        Assert.Equal(expected5, Math.Round(VectorAlgebra.GetAngle(a, b), 4));
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
        Assert.Equal(expected1, VectorAlgebra.Add(a, b));
        Assert.Equal(expected2, VectorAlgebra.Subtract(a, b));
        Assert.Equal(expected3, VectorAlgebra.Multiply(c, a));
    }

    [Fact]
    public static void AssortedTests()
    {
        // Arrange
        var a = new double[] {1, 1, 1};
        var expected1 = "<1,1,1>";
        
        // Act & Assert
        Assert.Equal(expected1, VectorAlgebra.ToString(a));


        // Arrange
        var b = VectorAlgebra.GetRandomVector(3);
        var c = VectorAlgebra.GetRandomVector(3);

        // Act & Assert
        // Testing that random vectors are different
        Assert.NotEqual(c, b);


        // Arrange
        var d = new double[] {1, 1, 1, 1}; // length = 2
        var e = new double[] {2, 2, 2, 2}; // length = 4
        var expected2 = 100.0;

        // Act
        var x = VectorAlgebra.PercentDiffLength(e, d);

        // Asser
        Assert.Equal(expected2, x);
    }
}
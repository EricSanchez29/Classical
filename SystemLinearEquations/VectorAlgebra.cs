using System.Text;

namespace Maths.LinearAlgebra;

public static class VectorAlgebra
{
    // Use the product of a vector with itself, to get vector length^2
    // Sqrt(a * a) = lenght of a
    //
    // Commutative
    public static double DotProduct(double[] leftVector, double[] rightVector = null)
    {
        if (rightVector == null)
            rightVector = leftVector;

        // Dot product is only defined for vectors of the same dimension
        if (leftVector.Length != rightVector.Length)
            return double.MinValue;

        double result = 0;

        for (int i = 0; i < leftVector.Length; i++)
            result += leftVector[i] * rightVector[i];

        return result;
    }

    // a * b = |a||b|cos(theta)
    //
    // inverse Cos(a*b/|a||b|) = theta
    //
    // Commutative
    public static double GetAngle(double[] leftVector, double[] rightVector)
    {
        return System.Math.Acos(DotProduct(leftVector, rightVector)/(System.Math.Sqrt(DotProduct(leftVector, leftVector))*System.Math.Sqrt(DotProduct(rightVector, rightVector))));
    }

    /*
        i  j  k
        x1 x2 x3
        y1 y2 y3 

        (x2y3-x3y2, -(x1y3-x3y1), x1y2- x2y1)
    */
    // a x b = |a||b|sin(theta)(unit vector perpendicular to a and b)
    // Noncommutative
    public static double[] CrossProduct(double[] leftVector, double[] rightVector)
    {
        if (leftVector == null || rightVector == null || leftVector.Length != rightVector.Length)
            return Array.Empty<double>();

        var result = new double[leftVector.Length];

        if (leftVector.Length == 3)
        {
            result[0] = leftVector[1]*rightVector[2] - leftVector[2]*rightVector[1];
            result[1] = -leftVector[0]*rightVector[2] + leftVector[2]*rightVector[0];
            result[2] = leftVector[0]*rightVector[1] - leftVector[1]*rightVector[0];
        }

        return result;
    }

    // Commutative
    public static double[] Add(double[] leftVector, double[] rightVector)
    {
        if (leftVector == null || rightVector == null || leftVector.Length != rightVector.Length)
        {
            return Array.Empty<double>();
        }

        var result = new double[leftVector.Length];

        for (int i = 0; i < leftVector.Length; i++)
        {
            result[i] = leftVector[i] + rightVector[i];
        }

        return result;
    }

    // Noncommutative
    public static double[] Subtract(double[] leftVector, double[] rightVector)
    {
        if (leftVector == null || rightVector == null || leftVector.Length != rightVector.Length)
        {
            return Array.Empty<double>();
        }

        var result = new double[leftVector.Length];

        for (int i = 0; i < leftVector.Length; i++)
        {
            result[i] = leftVector[i] - rightVector[i];
        }

        return result;
    }

    // Commutative
    public static double[] Multiply(double scalar, double[] vector)
    {
        var result = new double[vector.Length];

        for (int i = 0; i < vector.Length; i++)
        {
            result[i] = scalar * vector[i];
        }
        
        return result;
    }

    public static double PercentDiffLength(double[] testVector, double[] refVector)
    {
        if (testVector == null || refVector == null || testVector.Length != refVector.Length)
            return double.MaxValue;

        var testLength = Math.Sqrt(DotProduct(testVector));
        var refLength = Math.Sqrt(DotProduct(refVector));

        // if reference vector length is 0,
        // percent diff is undefined
        if (refLength == 0)
        {
            return double.NaN;
        }

        double result = (testLength - refLength) / refLength;

        return result * 100;
    }

    public static double[] GetRandomVector(int length, bool integer = false)
    {
        var result = new double[length];

        var rand = new Random();

        if (integer)
        {
            for (int i = 0; i < length; i++)
            {
                result[i] = (double)rand.Next();
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {                         
                result[i] = rand.NextDouble();
            }
        }

        return result;
    }

    // Returns vector "<v0,v1,v2,...,vn>"
    // Also prints to console if flag is set to true
    public static string PrintVector(double[] vector, bool console = false)
    {
        if (vector == null || vector.Length == 0)
        {
            return string.Empty;
        }

        var length = vector.Length;

        var sb = new StringBuilder();
        sb.Append('<');
        for (int i = 0; i < length - 1; i++)
        {                
            sb.Append(vector[i].ToString());
            sb.Append(',');
        }

        // am doing this outside of the for loop to avoid checking 
        // for this case since its always last
        sb.Append(vector[length - 1].ToString());
        sb.Append('>');

        if (console)
            Console.WriteLine(sb);

        return sb.ToString();
    }

    public static double[] Round(double[] vector, int? precision = null)
    {
        precision = precision ?? Global.Precision;

        if ((vector?.Length ?? 0) == 0)
        {
            return Array.Empty<double>();
        }

        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] = Math.Round(vector[i], (int)precision);
        }

        return vector;
    }
}
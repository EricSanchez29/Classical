using System.Text;

namespace Maths.LinearAlgebra;

public static class Vector
{
    // Use the product of a vector with itself, to get vector length^2
    // Sqrt(a * a) = lenght of a
    //
    // Commutative
    public static double DotProduct(double[] leftVector, double[] rightVector)
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

    public static double GetVectorLength(double[] vector)
    {
        double answer = 0;

        for (int i = 0; i < vector.Length; i++)
        {
            answer += vector[i] * vector[i];
        }

        return Math.Sqrt(answer);
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

        var testLength = GetVectorLength(testVector);
        var refLength = GetVectorLength(refVector);

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

        for (int i = 0; i < length; i++)
        {
            if (integer)
            {
                result[i] = rand.NextInt64();
            }
            else
            {
                result[i] = rand.NextDouble();
            }
        }

        return result;
    }

    // Returns vector "<v0,v1,v2,...,vn>"
    public static string ToString(double[] vector)
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

        sb.Append(vector[length - 1].ToString());
        sb.Append('>');

        return sb.ToString();
    }

    

    public static double[] Round(double[] vector, int? precision = null)
    {
        precision ??= Global.Precision;

        if ((vector == null) || (vector.Length == 0))
        {
            return Array.Empty<double>();
        }

        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] = Math.Round(vector[i], (int)precision);
        }

        return vector;
    }

    public static bool Equal(double[] vector1, double[] vector2, int precision)
    {
        if (vector1 is null|| vector2 is null || vector1.Length != vector2.Length)
        {
            throw new ArgumentException();
        }

        for (int i = 0; i < vector1.Length; i++)
        {
            // round both values
            double value1 = Math.Round(vector1[i], precision);
            double value2 = Math.Round(vector2[i], precision);

            if (!double.Equals(value1, value2))
            {
                return false;
            }
        }

        return true;
    }
}
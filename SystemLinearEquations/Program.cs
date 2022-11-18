namespace Maths;
// This program compares different algorithms which achieve the same task
// - Linear System Solver (approximate vs exact) [only for Hermitian matricies]
// - "                                         " [for any square matrix]
//
// I originally intended for this code to be used as a libary, perhaps by a more advanced math exe.
// But there is no need to publish a math library, I'm not trying to reinvent NumPy
class Program
{
    static void Main()
    {
        //LinearSystemComparer.NonHermitianSystem();
        //LinearSystemComparer.HermitianSystem();
        //LinearSystemComparer.NonHermitianSystem_ApproxOnly();
        //LinearSystemComparer.HermitianSystem_ApproxOnly();
    }
}

public class Global
{
    public static readonly int Precision = 15;
}
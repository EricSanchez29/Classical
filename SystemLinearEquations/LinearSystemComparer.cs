using SystemLinearEquations.LinearSystemAlgorithms;
using Maths.LinearAlgebra;
using System.Diagnostics;

namespace Maths;

// should this type of functionality exist in the test solution?
//public class LinearSystemComparer
//{
//    public LinearSystemComparer(int startSize, int endSize)
//    {
//        // expose analysis data through functions or properties

//        IList<(Matrix, double[])>  _systems = new List<(Matrix, double[])>();

//        generateSystems(startSize, endSize, _systems);

//        List<ILinearSystemMethod> solvers = new List<ILinearSystemMethod>();

//        foreach(var system in _systems)
//        {
//            foreach (var sol in solvers)
//            {
//                Console.WriteLine(Vector.ToString(sol.Solve(system.Item1, system.Item2)));
//            }
//        }
//    }




//    private void generateSystems(int startSize, int endSize, IList<(Matrix m, double[] b)> sys)
//    {
//        // consider using previous matrix as basis for next matrix

//        // for now all matricies are independent

//        for (int i = startSize; i < endSize; i++)
//        {
//            sys.Add(new(Matrix.GetRandomSquareMatrix(i), Vector.GetRandomVector(i)));
//        }
//    }


    //// Used to compare the exact vs approximate solutions of Hermitian linear systems (n x n)
    //public static void HermitianSystem()
    //{
    //    // Solve for x vector (n dimensions)
    //    // Ax = b
    //    // where A is Hermitian
    //    // and b is any real vector (n dimensions)

    //    compareMethods(1, 23, true);
    //}

    //// Used to compare the exact vs approximate solutions of square(n x n), real valued linear systems 
    //public static void NonHermitianSystem()
    //{
    //    // Solve for x vector (n dimensions)
    //    // Ax = b
    //    // where A is a real square matrix
    //    // and b is any real vector (n dimensions)

    //    compareMethods(1, 23, false);
    //}

    //// Used to benchmark implementation of the algorithm
    //// Only running the Conjugate gradient method
    //public static void HermitianSystem_ApproxOnly()
    //{
    //    compareMethods(1700, 1701, true, false);
    //}

    //// Used to benchmark implementation of the algorithm
    //// Only running the Biconjugate gradient method
    //public static void NonHermitianSystem_ApproxOnly()
    //{
    //    compareMethods(400, 401, false, false);
    //}

    //// My individual trial timing of these algorithms does not include
    //// time to set up the inputs for before execution of algo
    ////
    //// However the total time does take this into account
    //private static void compareMethods(int startSize, int endSize, bool hermitian, bool includeCramers = true)
    //{
    //    if ((startSize > endSize) || (startSize < 1))
    //    {
    //        throw new ArgumentException("Wrong startSize and endSize combo");
    //    }

    //    // Largest System computed (n x n)
    //    var n = endSize;

    //    var sw = new Stopwatch();
    //    var sw1 = new Stopwatch();
    //    sw1.Start();

    //    // Prepare files
    //    var summaryFile = getFileName();
    //    string dataFile;



    //    if (hermitian)
    //    {
    //        dataFile = summaryFile + "_data_H.txt";
    //        summaryFile += "_H.txt";
    //    }
    //    else
    //    {
    //        dataFile = summaryFile + "_data.txt";
    //        summaryFile += ".txt";
    //    }

    //    var fileStream = new StreamWriter(summaryFile);
    //    var verboseStream = new StreamWriter(dataFile);

    //    int retryCount = 0;

    //    for (int i = startSize; i <= n && retryCount <=2; i++)
    //    {
    //        Matrix m1;

    //        if (hermitian)
    //        {
    //            // Prepare Hermitian matrix
    //            m1 = Matrix.GetRandomHermitianMatrix(i);
    //        }
    //        else
    //        {
    //            // Prepare Non-Hermitian matrix
    //            m1 = Matrix.GetRandomSquareMatrix(i);
    //        }

    //        // Prepare b vector
    //        var b1 = VectorAlgebra.GetRandomVector(i);

    //        double[] exact;
    //        TimeSpan exactTime;

    //        // Run different implementations with same input matrices and vectors
    //        // or run the same implementation and just do a self check, Ax = b compared with input b1
    //        if (includeCramers)
    //        {
    //            sw.Start();
    //            exact = CramersMethodModified.Solve(m1, b1);
    //            sw.Stop();
    //            exactTime = sw.Elapsed;
    //            Console.WriteLine("Approx: " + exactTime);
    //        }
    //        else
    //        {
    //            exact = Array.Empty<double>();
    //            exactTime = TimeSpan.MinValue;
    //        }

    //        sw.Restart();
    //        var approx = LinearSystemAlgorithms.ConjugateTransposeMethod(m1, b1);
    //        sw.Stop();

    //        if (approx.Length == 0)
    //        {
    //            Console.WriteLine("Approx: Divided by zero =(");
    //            Console.WriteLine("Retry");
    //            i--;
    //            retryCount++;
    //            continue;
    //        }

    //        if (!CheckSolution(m1, approx, b1, out var difference))
    //        {
    //            Console.WriteLine("Approx: Percent difference in length " + difference);
    //            Console.WriteLine("Retry");
    //            retryCount++;
    //            i--;
    //            continue;
    //        }

    //        // Only write if there is a comparison to make
    //        // aka if the approximation is not completly off

    //        verboseStream.WriteLine("n=" + i);
    //        fileStream.WriteLine("n=" + i);
    //        verboseStream.WriteLine("A=");

    //        var matrixA = m1.ConvertToStringArray();

    //        for (int j = 0; j < matrixA.Length; j++)
    //        {
    //            verboseStream.WriteLine(matrixA[j]);
    //        }

    //        verboseStream.WriteLine();


    //        verboseStream.WriteLine("b= " + VectorAlgebra.ToString(b1));
    //        verboseStream.WriteLine();

    //        if (includeCramers)
    //        {
    //            fileStream.WriteLine(string.Format(@"Size: {0} ", i));
    //            fileStream.WriteLine("Exact");
    //            fileStream.WriteLine(VectorAlgebra.ToString(exact));
    //            fileStream.WriteLine(string.Format(@"Elapsed: {0}", exactTime));
    //            fileStream.WriteLine();
    //        }

    //        fileStream.WriteLine("Approximate");
    //        fileStream.WriteLine(VectorAlgebra.ToString(approx));
    //        fileStream.WriteLine(string.Format(@"Elapsed: {0}", sw.Elapsed));
    //        fileStream.WriteLine();

    //        verboseStream.WriteLine("Approximation self check: " + difference + "%");
    //        fileStream.WriteLine("Approximation self check: " + difference + "%");


    //        // compare results
    //        if (includeCramers)
    //        {
    //            fileStream.WriteLine(string.Format(@"Percent Diff Length: {0}%", VectorAlgebra.PercentDiffLength(approx, exact)));
    //        }

    //        // break
    //        fileStream.WriteLine("__________________");
    //        fileStream.WriteLine();
    //    }

    //    sw1.Stop();

    //    var elapsed = sw1.Elapsed;
    //    Console.WriteLine("Total time:" + elapsed.ToString());

    //    fileStream.WriteLine("Total time:" + elapsed.ToString());

    //    // Close files
    //    fileStream.Close();
    //    verboseStream.Close();
    //}

    //private static void compareChiosMethod(int startSize, int endSize, int comparison)
    //{
    //    switch (comparison)
    //    {
    //        case 0:

    //            break;

    //        case 1:
    //            break;


    //        default:
    //            break;
    //    }

    //}

    //private static string getFileName()
    //{
    //    // example "6/22/2022 1:02:57 AM";
    //    var folderName = @"C:\logs\MathApp\";

    //    // create a directory for a file if it doesn't already exist
    //    Directory.CreateDirectory(folderName);

    //    var fileName = DateTime.Now.ToString().Replace(' ','_').Replace(':','_').Replace('/','_');

    //    return folderName + fileName;
    //}

    // Returns true if a solution is within error
    //private static bool CheckSolution(Matrix m, double[] x, double[] b, out double percentDifference)
    //{
    //    var _b = Matrix.Multiply(m, x);

    //    percentDifference = VectorAlgebra.PercentDiffLength(_b, b);

    //    Console.WriteLine("Percent Diff Length:" + percentDifference);

    //    if (Math.Abs(percentDifference) < 0.1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
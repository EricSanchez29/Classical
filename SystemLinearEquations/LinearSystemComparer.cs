using System;
using System.Diagnostics;
using System.IO;
using Maths.LinearAlgebra;

namespace Maths;

public static class LinearSystemComparer
{
    // Used to compare the exact vs approximate solutions of Hermitian linear systems (n x n)
    public static void HermitianSystem()
    {
        // Solve for x vector (n dimensions)
        // Ax = b
        // where A is Hermitian
        // and b is any real vector (n dimensions)
        
        compareMethods(1, 20, true);
    }

    // Used to compare the exact vs approximate solutions of square(n x n), real valued linear systems 
    public static void NonHermitianSystem()
    {
        // Solve for x vector (n dimensions)
        // Ax = b
        // where A is a real square matrix
        // and b is any real vector (n dimensions)

        compareMethods(1, 20, false);
    }

    // Used to benchmark implementation of the algorithm
    // Only running the Conjugate gradient method
    public static void HermitianSystem_ApproxOnly()
    {
        // Solve for x vector (n dimensions)
        // Ax = b
        // where A is Hermitian
        // and b is any real vector (n dimensions)

        compareMethods(999, 1000, true, false);
    }

    // Used to benchmark implementation of the algorithm
    // Only running the Biconjugate gradient method
    public static void NonHermitianSystem_ApproxOnly()
    {
        // Solve for x vector (n dimensions)
        // Ax = b
        // where A is a real square matrix
        // and b is any real vector (n dimensions)

        compareMethods(299, 300, false, false);
    }

    // My individual trial timing of these algorithms does not include
    // time to set up the inputs for before execution of algo
    //
    // However the total time does take this into account
    private static void compareMethods(int startSize, int endSize, bool hermitian, bool includeCramers = true)
    {
        if ((startSize > endSize) || (startSize < 1))
        {
            throw new ArgumentException();
        }

        // Largest System computed (n x n)
        var n = endSize;

        var sw = new Stopwatch();
        var sw1 = new Stopwatch();
        sw1.Start();

        // Prepare files
        var summaryFile = getFileName();
        string dataFile;

        if (hermitian)
        {
            dataFile = summaryFile + "_data_H.txt";
            summaryFile += "_H.txt";
        }
        else
        {
            dataFile = summaryFile + "_data.txt";
            summaryFile += ".txt";
        }
        
        var fileStream = new StreamWriter(summaryFile);
        var verboseStream = new StreamWriter(dataFile);

        for (int i = startSize; i <= n; i++)
        {
            Matrix m1;

            if (hermitian)
            {
                // Prepare Hermitian matrix
                m1 = Matrix.GetRandomHermitianMatrix(i);
            }
            else
            {
                // Prepare Non-Hermitian matrix
                m1 = Matrix.GetRandomSquareMatrix(i);
            }

            // Prepare b vector
            var b1 = VectorAlgebra.GetRandomVector(i);

            double[] exact;
            TimeSpan exactTime;

            // Run different implementations with same input matrices and vectors
            // or run the same implementation and just do a self check, Ax = b compared with input b1
            if (includeCramers)
            {
                sw.Start();
                exact = LinearSystemSolver.FindLinearSystemSolution_Exact(m1, b1);
                sw.Stop();
                exactTime = sw.Elapsed;
                Console.WriteLine("Approx: " + exactTime);
            }
            else
            {
                exact = null;
                exactTime = TimeSpan.MinValue;
            }

            sw.Restart();
            var approx = LinearSystemSolver.FindLinearSystemSolution_Approx(m1, b1);
            sw.Stop();

            //Console.WriteLine("Approx: " + approx);

            if (!CheckSolution(m1, approx, b1, out var difference))
            {
                Console.WriteLine("Approx: Percent difference in length " + difference);
                Console.WriteLine("Retry");
                i--;
                continue;
            }

            // Only write if there is a comparison to make
            // aka if the approximation is not completly off

            verboseStream.WriteLine("n=" + i);
            fileStream.WriteLine("n=" + i);
            verboseStream.WriteLine("A=");

            var matrixA = m1.ConvertToStringArray();

            for (int j = 0; j < matrixA.Length; j++)
            {
                verboseStream.WriteLine(matrixA[j]);
            }

            verboseStream.WriteLine();

            
            verboseStream.WriteLine("b= " + VectorAlgebra.PrintVector(b1));
            verboseStream.WriteLine();

            if (includeCramers)
            {
                fileStream.WriteLine(string.Format(@"Size: {0} ", i));
                fileStream.WriteLine("Exact");
                fileStream.WriteLine(VectorAlgebra.PrintVector(exact));
                fileStream.WriteLine(string.Format(@"Elapsed: {0}", exactTime));
                fileStream.WriteLine();
            }

            fileStream.WriteLine("Approximate");
            fileStream.WriteLine(VectorAlgebra.PrintVector(approx));
            fileStream.WriteLine(string.Format(@"Elapsed: {0}", sw.Elapsed));
            fileStream.WriteLine();

            verboseStream.WriteLine("Approximation self check: " + difference + "%");
            fileStream.WriteLine("Approximation self check: " + difference + "%");


            // compare results
            if (includeCramers)
            {
                fileStream.WriteLine(string.Format(@"Percent Diff Length: {0}%", VectorAlgebra.PercentDiffLength(approx, exact)));
            }

            // break
            fileStream.WriteLine("__________________");
            fileStream.WriteLine();
        }

        sw1.Stop();

        var elapsed = sw1.Elapsed;
        Console.WriteLine("Total time:" + elapsed.ToString());

        fileStream.WriteLine("Total time:" + elapsed.ToString());

        // Close files
        fileStream.Close();
        verboseStream.Close();
    }

    // Creates directory for file if it doesn't already exist
    private static string getFileName()
    {
        // example "6/22/2022 1:02:57 AM";
        var folderName = @"C:\logs\MathApp\";

        Directory.CreateDirectory(folderName);

        var fileName = DateTime.Now.ToString().Replace(' ','_').Replace(':','_').Replace('/','_');

        return folderName + fileName;
    }

    // returns true if a solution is within error
    private static bool CheckSolution(Matrix m, double[] x, double[] b, out double percentDifference)
    {
        var _b = Matrix.Multiply(m, x);

        percentDifference = VectorAlgebra.PercentDiffLength(_b, b);
        
        Console.WriteLine("Percent Diff Length:" + percentDifference);

        if (Math.Abs(percentDifference) < 0.1)
        {
            return true;
        }

        return false;
    }
}
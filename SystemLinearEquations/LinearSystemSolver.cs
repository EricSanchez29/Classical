using SystemLinearEquations.LinearSystemAlgorithms;
using Maths.LinearAlgebra;

namespace SystemLinearEquations
{
    public static class LinearSystemSolver
    {
        //private ICollection<ILinearSystemSolver> _solvers;
        

              // this logic could eventually be generated or mutated by AI
    private static double[]  SolveSystem((Matrix matrix, double[] b) sys, IList<ILinearSystemMethod> solvers)
    {
        // this block of conditional statements are based on optimizing getting a solution based on time to execute
        if (sys.matrix.Dimensions.Column <= 0)
        {
            throw new ArgumentException("Dimension of" + sys.matrix.Dimensions.Column + "not allowed");
        }
        // am going to have to change this bound
        else if (sys.matrix.Dimensions.Column < 24)
        {
            return getSolver(solvers, "").Solve(sys.matrix, sys.b);
        }
        else
        {
            //return ConjugateTransposeMethods.ConjugateTransposeMethod(matrix.matrix, )
        }

        return Array.Empty<double>();
    }

    private static ILinearSystemMethod  getSolver(IList<ILinearSystemMethod> solvers, string name)
    {
        ILinearSystemMethod solver = solvers.FirstOrDefault(s => name.Equals(s.GetType().ToString() == name));

        if (solver == null)
        {
            throw new ArgumentException();
        }

        return solver;
    }

    }
}

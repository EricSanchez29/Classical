using Maths.LinearAlgebra;

namespace SystemLinearEquations.LinearSystemAlgorithms
{
    public interface ILinearSystemMethod
    {
        public double[] Solve(Matrix A, double[] b);
    }
}

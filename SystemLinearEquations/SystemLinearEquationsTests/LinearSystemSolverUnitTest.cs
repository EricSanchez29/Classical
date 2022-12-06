using Maths.LinearAlgebra;
using Maths;
using SystemLinearEquations.LinearSystemAlgorithms;
using SystemLinearEquations;

namespace SystemLinearEquationsTests
{
    public static class LinearSystemSolverUnitTest
    {
        [Theory]
        //[InlineData(3, 14)]
        //[InlineData(10, 13)]
        //[InlineData(15, 12)]
        [InlineData(20, 10)]
        //[InlineData(30, 2)]
        //[InlineData(40, 2)] 
        //[InlineData(100, 0)]
        public static void TestSolveSystem_Int_Happy(int size, int precision)
        {
            var A2 = Matrix.GetRandomHermitianMatrix(size);
            var b2 = Vector.GetRandomVector(size);

            // x = (A^-1 * b)
            var x2 = LinearSystemSolver.SolveSystem(A2, b2);

            // b = Ax
            var mult = A2.Multiply(x2);

            // Ax = b
            Assert.True(Vector.Equal(A2.Multiply(x2), b2, precision));

            // maybe use a different comparer, percent diff
        }

        [Fact]
        public static void TestSolveSystem_Int_BadFormula()
        {
            var A2 = Matrix.GetRandomHermitianMatrix(3);
            var b2 = Vector.GetRandomVector(3);

            // Wrong equation for solving a system
            // x = (A^-1 * b) + b
            var x2 = Vector.Add(LinearSystemSolver.SolveSystem(A2, b2), b2);

            // Ax = b
            Assert.NotEqual(A2.Multiply(x2), b2);
        }
    }
}

using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IExpressionSolver
    {
        Solution SolveExpression(string expression);
        Solution SolveExpression(IExpression expression);
    }
}
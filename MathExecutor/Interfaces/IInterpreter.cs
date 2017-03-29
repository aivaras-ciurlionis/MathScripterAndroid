using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IInterpreter
    {
        bool CanBeParsed(string expression);
        Solution FindSolution(string expression);
        Solution FindSolution(IExpression expression);
        IExpression GetExpression(string expression);
    }
}
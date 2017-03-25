using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IInterpreter
    {
        bool CanBeParsed(string expression);
        Solution FindSolution(string expression);
        IExpression GetExpression(string expression);
    }
}
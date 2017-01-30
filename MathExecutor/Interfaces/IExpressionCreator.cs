using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IExpressionCreator
    {
        IExpression CreateExpression(IEnumerable<Token> tokens);
    }
}
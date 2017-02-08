using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IExpressionFactory
    {
        IExpression GetExpression(string key);
        IExpression GetExpression(string key, Token lastToken);
        IExpression GetExpression(string key, IList<IExpression> operands);
    }
}
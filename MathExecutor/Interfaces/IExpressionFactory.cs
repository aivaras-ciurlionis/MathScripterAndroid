using System.Collections.Generic;

namespace MathExecutor.Interfaces
{
    public interface IExpressionFactory
    {
        IExpression GetExpression(string key);
        IExpression GetExpression(string key, IList<IExpression> operands);
    }
}
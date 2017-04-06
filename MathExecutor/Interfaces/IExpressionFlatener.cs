using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IExpressionFlatener
    {
        IEnumerable<FlatExpressionResult> FlattenExpression(IExpression expression, bool onlyFirstLevel = false,
          bool includeMonomial = false, bool includeMult = false);
    }
}
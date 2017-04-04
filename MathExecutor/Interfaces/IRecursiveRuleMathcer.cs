using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IRecursiveRuleMathcer
    {
        IEnumerable<Step> SolveExpression(IExpression expression);
    }
}
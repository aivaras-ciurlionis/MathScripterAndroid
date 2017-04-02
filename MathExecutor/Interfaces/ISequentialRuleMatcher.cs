using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface ISequentialRuleMatcher
    {
        IEnumerable<Step> GetSequentialRuleSteps(IExpression expression);
    }
}
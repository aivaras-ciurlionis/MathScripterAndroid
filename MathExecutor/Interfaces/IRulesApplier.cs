using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IRulesApplier
    {
        IEnumerable<RuleApplyResult> ApplyRules(IExpression expression);
    }
}
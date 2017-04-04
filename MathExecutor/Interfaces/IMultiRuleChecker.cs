using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IMultiRuleChecker
    {
        IList<Step> ApplyRules(IExpression expression);
    }
}
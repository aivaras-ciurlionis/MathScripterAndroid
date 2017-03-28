using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IRule
    {
        RuleApplyResult ApplyRule(IExpression expression);
        string Description { get; set; }
    }
}
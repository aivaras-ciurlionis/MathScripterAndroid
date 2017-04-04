using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class InnerRuleResult
    {
        public IExpression Expression { get; set; }
        public bool ConsumeParent { get; set; }

        public InnerRuleResult(IExpression expression)
        {
            Expression = expression;
            ConsumeParent = false;
        }

        public InnerRuleResult(IExpression expression, bool consumeParent)
        {
            Expression = expression;
            ConsumeParent = consumeParent;
        }
    }
}
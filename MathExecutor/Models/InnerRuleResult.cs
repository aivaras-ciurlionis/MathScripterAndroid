using System.Collections.Generic;
using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class InnerRuleResult
    {
        public IExpression Expression { get; set; }
        public IEnumerable<IExpression> HelperSteps { get; set; }
        public bool ConsumeParent { get; set; }

        public InnerRuleResult(IExpression expression) : this(expression, false, null)
        {
        }

        public InnerRuleResult(IExpression expression, bool consumeParent) : this(expression, consumeParent, null)
        {
        }

        public InnerRuleResult(IExpression expression, bool consumeParent, IEnumerable<IExpression> expressions)
        {
            Expression = expression;
            ConsumeParent = consumeParent;
            HelperSteps = expressions;
        }
    }
}
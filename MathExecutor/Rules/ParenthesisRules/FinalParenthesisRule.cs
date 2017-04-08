using System.Linq;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.ParenthesisRules
{
    public class FinalParenthesisRule : AbstractRecursiveRule
    {
       protected override InnerRuleResult ApplyRuleInner(IExpression expression)
       {
           return new InnerRuleResult(expression.Operands[0]);
       }

        protected override bool CanBeApplied(IExpression expression)
        {
            return expression is ParenthesisExpression &&
                   (expression.ParentExpression is ParenthesisExpression ||
                    expression.ParentExpression is EqualityExpression ||
                    expression.ParentExpression is RootExpression ||
                    expression.ParentExpression == null);

        }

        public override string Description => "Parenthesis removal";
    }
}
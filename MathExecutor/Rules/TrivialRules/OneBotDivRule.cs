using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.TrivialRules
{
    public class OneBotDivRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            return new InnerRuleResult(expression.Operands[0]);
        }

        private static bool IsOneMonomial(IExpression expression)
        {
            var monomial = expression as Monomial;
            return monomial != null && monomial.IsNumericWithCoeficient(1);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is DivisionExpression))
            {
                return false;
            }
            return IsOneMonomial(expression.Operands[1]);
        }

        public override string Description => "Dividing by 1 leaves top part.";
    }
}
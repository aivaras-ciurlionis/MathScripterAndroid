using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.TrivialRules
{
    public class ZeroTopDivRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            return new InnerRuleResult(new Monomial(0));
        }

        private static bool IsZeroMonomial(IExpression expression)
        {
            var monomial = expression as Monomial;
            return monomial != null && monomial.IsNumericWithCoeficient(0);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is DivisionExpression))
            {
                return false;
            }
            return IsZeroMonomial(expression.Operands[0]);
        }

        public override string Description => "0 divided by non zero expression is 0.";
    }
}
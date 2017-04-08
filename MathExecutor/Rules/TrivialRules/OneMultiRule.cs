using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.TrivialRules
{
    public class OneMultiRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var m1 = expression.Operands[0];
            var m2 = expression.Operands[1];
            if (IsOneMonomial(m1))
            {
                return new InnerRuleResult(expression.Operands[1]);
            }
            else
            {
                return new InnerRuleResult(expression.Operands[0]);
            }
        }

        private static bool IsOneMonomial(IExpression expression)
        {
            var monomial = expression as Monomial;
            return monomial != null && monomial.IsNumericWithCoeficient(1);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is MultiplyExpression))
            {
                return false;
            }
            return IsOneMonomial(expression.Operands[0]) || IsOneMonomial(expression.Operands[1]);
        }

        public override string Description => "Multiplying by 1";
    }
}
using System;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.MonomialRules
{
    public class MonomialZeroRole : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var m1 = expression.Operands[0] as Monomial;
            var m2 = expression.Operands[1] as Monomial;
            var isNegative = expression is SubtractExpression;
            if (m1 != null)
            {
                return Math.Abs(m1.Coefficient) < 0.001 ? new InnerRuleResult(
                    isNegative 
                    ? new NegationExpression(expression.Operands[1]) 
                    : expression.Operands[1]) 
                    : null;
            }
            if (m2 != null)
            {
                return Math.Abs(m2.Coefficient) < 0.001 ? new InnerRuleResult(
                    isNegative
                    ? new NegationExpression(expression.Operands[0])
                    : expression.Operands[0])
                    : null;
            }
            return null;
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            var isSum = expression is SumExpression || expression is SubtractExpression;
            return isSum && (expression.Operands[0] is Monomial || expression.Operands[1] is Monomial);
        }

        public override string Description => "Removing zero";
    }
}
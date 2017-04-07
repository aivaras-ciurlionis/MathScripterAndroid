using System;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.MonomialRules
{
    public class MonomialRemovalRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            return new InnerRuleResult(new Monomial(0));
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            var monomial = expression as Monomial;
            return monomial != null &&
                   Math.Abs(monomial.Coefficient) < 0.001;
        }

        public override string Description => "Removing monomial, because coeficient is 0";
    }
}
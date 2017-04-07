using System;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.QuadraticRules
{
    public class QuadratDifferenceRule : AbstractRecursiveRule
    {
        private readonly IParentChecker _parentChecker;

        public QuadratDifferenceRule(IParentChecker parentChecker)
        {
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var a = expression.Operands[0];
            var b = expression.Operands[1];
            var left = new ParenthesisExpression(new SubtractExpression(a, b));
            var right = new ParenthesisExpression(new SumExpression(a, b));
            return new InnerRuleResult(new ParenthesisExpression(new MultiplyExpression(left, right)));
        }

        private bool IsQuadratExponent(IExpression expression)
        {
            if (!(expression is ExponentExpression))
            {
                return false;
            }
            var right = expression.Operands[1] as Monomial;
            return right != null && Math.Abs(right.Coefficient - 2) < 0.001;
        }
     
        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is SubtractExpression) || expression.Arity != 2)
            {
                return false;
            }

            if (!_parentChecker.LeftParentIsPositive(expression))
            {
                return false;
            }

            return IsQuadratExponent(expression.Operands[0]) &&
                IsQuadratExponent(expression.Operands[1]);
        }

        public override string Description => "a^2-b^2=(a-b)(a+b)";
    }
}
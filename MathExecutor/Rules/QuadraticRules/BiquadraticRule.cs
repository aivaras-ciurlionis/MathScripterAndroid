using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.QuadraticRules
{
    public class BiquadraticRule : AbstractRecursiveRule
    {
        private readonly IExpressionFlatener _expressionFlatener;

        public BiquadraticRule(IExpressionFlatener expressionFlatener)
        {
            _expressionFlatener = expressionFlatener;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var parenthesis = expression.Operands[0];
            var insideParenthesis = _expressionFlatener.FlattenExpression(parenthesis.Operands[0], true);
            var parenthesisOperations = insideParenthesis.Where(p => p.Expression is SumExpression ||
                                                                     p.Expression is SubtractExpression);

            var flatExpressionResults = parenthesisOperations as IList<FlatExpressionResult> ?? parenthesisOperations.ToList();
            if (flatExpressionResults.Count() != 1)
            {
                return null;
            }
            var operation = flatExpressionResults.First().Expression;
            var a = operation.Operands[0];
            var b = operation.Operands[1];
            var e1 = new ExponentExpression(a.Clone(true), new Monomial(2));
            var e2 = new MultiplyExpression(new Monomial(2), new MultiplyExpression(a.Clone(true), b.Clone(true)));
            var e3 = new ExponentExpression(b.Clone(true), new Monomial(2));
            IExpression signExpression;
            if (operation is SumExpression)
            {
                signExpression = new SumExpression(e1, e2);
            }
            else
            {
                signExpression = new SubtractExpression(e1, e2);
            }
            var result = new SumExpression(signExpression, e3);
            return new InnerRuleResult(new ParenthesisExpression(result));
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is ExponentExpression))
            {
                return false;
            }
            var exponent = expression.Operands[1] as Monomial;
            var left = expression.Operands[0];
            return exponent != null &&
                   Math.Abs(exponent.Coefficient - 2) < 0.001 &&
                   left is ParenthesisExpression;
        }

        public override string Description => "Biquadratic rule: (a+b)^2 = a^2+2ab+b^2";
    }
}
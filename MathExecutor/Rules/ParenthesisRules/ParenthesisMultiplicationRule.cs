using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.ParenthesisRules
{
    public class ParenthesisMultiplicationRule : AbstractRecursiveRule
    {
        private readonly IExpressionFlatener _expressionFlatener;
        private readonly IParentChecker _parentChecker;

        public ParenthesisMultiplicationRule(IExpressionFlatener expressionFlatener, 
            IParentChecker parentChecker)
        {
            _expressionFlatener = expressionFlatener;
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var parent = expression.ParentExpression;
            var left = expression.Operands[0];
            var right = expression.Operands[1];
            var leftElements = _expressionFlatener.FlattenExpression(left, true, true);
            var rightElements = _expressionFlatener.FlattenExpression(right, true, true);
            var replacableLeft = leftElements.Where(e => !(e.Expression is SumExpression ||
                                                           e.Expression is SubtractExpression))
                                                          .ToList();
            var replacableRight = rightElements.Where(e => !(e.Expression is SumExpression ||
                                                             e.Expression is SubtractExpression))
                                                           .ToList();
            IExpression newExpression = null;
            foreach (var elementX in replacableLeft)
            {
                foreach (var elementY in replacableRight)
                {
                    IExpression mul = new MultiplyExpression(elementX.Expression, elementY.Expression);
                    var isNegative = (!_parentChecker.LeftParentIsPositive(elementX.Expression) || 
                                      !_parentChecker.LeftParentIsPositive(elementY.Expression)) &&
                                      !(!_parentChecker.LeftParentIsPositive(elementX.Expression) &&
                                      !_parentChecker.LeftParentIsPositive(elementY.Expression));
                    if (isNegative)
                    {
                        mul = new NegationExpression(mul);
                    }
                    newExpression = new SumExpression(newExpression, mul);
                }
            }
            if (newExpression == null)
            {
                return null;
            }
            newExpression.ParentExpression = parent;
            return new InnerRuleResult(new ParenthesisExpression(newExpression));
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is MultiplyExpression))
            {
                return false;
            }

            return expression.Operands[0] is ParenthesisExpression &&
                   expression.Operands[1] is ParenthesisExpression;
        }

        public override string Description => "Parenthesis multiplication: (a+b)(c+d) = ac + ad + bc + bd";
    }
}
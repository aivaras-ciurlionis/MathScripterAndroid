using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules
{
    public class ReorderRule : IRule
    {
        private readonly IExpressionFlatener _flatener;

        public ReorderRule(IExpressionFlatener flatener)
        {
            _flatener = flatener;
        }

        private IExpression GetLeftParentExpression(IExpression expression)
        {
            if (expression.ParentExpression.Operands[1] == expression)
            {
                return expression.ParentExpression;
            }
            var e = expression as Monomial;
            if (e.Coefficient < 0)
            {
                return new SubtractExpression(null, e);
            }
            return new SumExpression(null, e);
        }

        public RuleApplyResult ApplyRule(IExpression expression)
        {
            var elements = _flatener.FlattenExpression(expression, true, true);
            var flatExpressionResults = elements as IList<FlatExpressionResult> ?? elements.ToList();

            var topExpression = flatExpressionResults.Last();
            var topExpressionValue = topExpression.Expression.ToString();
            var topParent = topExpression.Expression.ParentExpression;

            var monomials = flatExpressionResults
                .Where(e => e.Expression is Monomial &&
                            (e.Expression.ParentExpression is SumExpression ||
                            e.Expression.ParentExpression is SubtractExpression));

            var reorderableExpressions = flatExpressionResults
                .Where(r => r.Expression is SumExpression || r.Expression is SubtractExpression)
                .Select(r => r.Expression)
                .ToList();

            var orderedMonomials = monomials.OrderBy(m => m.Expression as Monomial, new MonomialsComparer())
                .Select(m => m.Expression);

            var expressions = orderedMonomials as IList<IExpression> ?? orderedMonomials.ToList();
            Monomial nextExpression = null;
            IExpression lastGroup = null;
            while (expressions.Any())
            {
                IExpression currentGroup = null;

                while (expressions.Any())
                {
                    var lastMonomial = nextExpression;
                    nextExpression = expressions.First() as Monomial;
                    if (lastMonomial != null &&
                        !lastMonomial.AreVariablesEqual(nextExpression))
                    {
                        break;
                    }
                    if (currentGroup == null)
                    {
                        currentGroup = nextExpression;
                        var firstMonomialParent = GetLeftParentExpression(nextExpression);
                        if (firstMonomialParent is SubtractExpression)
                        {
                            nextExpression.Coefficient = -nextExpression.Coefficient;
                        }
                        expressions.Remove(nextExpression);
                        reorderableExpressions.Remove(firstMonomialParent);
                    }
                    else
                    {
                        var nextParent = GetLeftParentExpression(nextExpression);
                        nextParent.Operands[0] = currentGroup;
                        currentGroup.ParentExpression = nextParent;
                        currentGroup = nextParent;
                        expressions.Remove(nextExpression);
                        reorderableExpressions.Remove(nextParent);
                    }
                }

                var firstParent = currentGroup;

                if (lastGroup != null)
                {
                    firstParent = new SumExpression(lastGroup, currentGroup);
                    lastGroup.ParentExpression = firstParent;
                    currentGroup.ParentExpression = firstParent;
                }
                lastGroup = firstParent;
            }

            foreach (var reorderableExpression in reorderableExpressions)
            {
                reorderableExpression.Operands[0] = lastGroup;
                lastGroup.ParentExpression = reorderableExpression;
                lastGroup = reorderableExpression;
            }

            topExpression.Expression = lastGroup;
            topExpression.Expression.ParentExpression = topParent;

            return new RuleApplyResult
            {
                Applied = topExpressionValue != topExpression.Expression.ToString(),
                Expression = topExpression.Expression,
                RuleDescription = Description
            };
        }

        public string Description => "Variables reorder";
    }
}
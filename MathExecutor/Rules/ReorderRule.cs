using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules
{
    public class ReorderRule : AbstractRecursiveRule
    {
        private readonly IExpressionFlatener _flatener;
        private readonly IOtherExpressionAdder _expressionAdder;

        private IList<FlatExpressionResult> _flatExpressionResults;
        private IList<IExpression> _orderedMonomials;
        private IList<IExpression> _reorderableExpressions;


        public ReorderRule(IExpressionFlatener flatener,
            IOtherExpressionAdder expressionAdder)
        {
            _flatener = flatener;
            _expressionAdder = expressionAdder;
        }

        private IExpression GetLeftParentExpression(IExpression expression)
        {
            if (expression.ParentExpression.Operands[1] == expression)
            {
                return expression.ParentExpression;
            }
            var e = expression as Monomial;
            return new SumExpression(null, e);
        }

        private bool SetReorderVariables(IEnumerable<FlatExpressionResult> flatExpression)
        {
            _flatExpressionResults = flatExpression as IList<FlatExpressionResult> ?? flatExpression.ToList();
            var monomials = _flatExpressionResults
                .Where(e => e.Expression is Monomial &&
                            (e.Expression.ParentExpression is SumExpression ||
                             e.Expression.ParentExpression is SubtractExpression));

            _reorderableExpressions = _flatExpressionResults
               .Where(r => !(r.Expression is Monomial ||
                           r.Expression is SumExpression ||
                           r.Expression is SubtractExpression))
               .Select(r => r.Expression)
               .ToList();

            _orderedMonomials = monomials.OrderBy(m => m.Expression as Monomial, new MonomialsComparer())
                 .Select(m => m.Expression).ToList();

            return _orderedMonomials.Count > 0;
        }

        private IExpression GetNextGroup()
        {
            IExpression currentGroup = null;
            Monomial nextExpression = null;
            var expressions = _orderedMonomials;
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
                    _reorderableExpressions.Remove(firstMonomialParent);
                }
                else
                {
                    var nextParent = GetLeftParentExpression(nextExpression);
                    nextParent.Operands[0] = currentGroup;
                    currentGroup.ParentExpression = nextParent;
                    currentGroup = nextParent;
                    expressions.Remove(nextExpression);
                    _reorderableExpressions.Remove(nextParent);
                }
            }
            return currentGroup;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var elements = _flatener.FlattenExpression(expression, true, true);
            var hasExpressions = SetReorderVariables(elements);
            if (!hasExpressions)
            {
                return null;
            }
            var topExpression = _flatExpressionResults.Last();
            var topExpressionValue = topExpression.Expression.Clone();
            var topParent = topExpression.Expression.ParentExpression;
            var expressions = _orderedMonomials;
            IExpression lastGroup = null;
            while (expressions.Any())
            {
                var currentGroup = GetNextGroup();
                var firstParent = currentGroup;
                if (lastGroup != null)
                {
                    firstParent = new SumExpression(lastGroup, currentGroup);
                    lastGroup.ParentExpression = firstParent;
                    currentGroup.ParentExpression = firstParent;
                }
                lastGroup = firstParent;
            }
            lastGroup = _expressionAdder.AddExpressions(lastGroup, _reorderableExpressions, false);
            topExpression.Expression = lastGroup;
            topExpression.Expression.ParentExpression = topParent;
            var applied = !topExpressionValue.IsEqualTo(topExpression.Expression);
            return applied ? new InnerRuleResult(topExpression.Expression) : null;
        }

        public override string Description => "Variables reorder";

        protected override bool CanBeApplied(IExpression expression)
        {
            var r = expression != null &&
                   !(
                       expression is Monomial ||
                       expression.ParentExpression is SumExpression ||
                       expression.ParentExpression is SubtractExpression ||
                       expression.ParentExpression is NegationExpression
                   );
            return r;
        }
    }
}
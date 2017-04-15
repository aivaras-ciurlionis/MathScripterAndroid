using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules
{
    public class EqualityReorderRule : AbstractRecursiveRule
    {
        private readonly IExpressionFlatener _flatener;
        private readonly IOtherExpressionAdder _expressionAdder;

        private IList<ExpressionWithSide> _orderedMonomials;
        private IList<ExpressionWithSide> _orderedNumerics;
        private IList<ExpressionWithSide> _reorderableExpressions;

        public EqualityReorderRule(
            IExpressionFlatener flatener,
            IOtherExpressionAdder expressionAdder
            )
        {
            _flatener = flatener;
            _expressionAdder = expressionAdder;
        }

        private IExpression GetLeftParentExpression(IExpression expression, bool switchSides, bool firstInGroup)
        {
            var e = expression as Monomial;
            if (expression.ParentExpression.Operands[1] != expression ||
                expression.ParentExpression is EqualityExpression)
            {
                if (switchSides)
                {
                    e.Coefficient *= -1;
                }
                return new SumExpression(null, e);
            }

            if (!switchSides)
            {
                if (firstInGroup && expression.ParentExpression is SubtractExpression)
                {
                    e.Coefficient = -e.Coefficient;
                }
                return expression.ParentExpression;
            }

            if (!(expression.ParentExpression is SumExpression)) return new SumExpression(null, e);
            if (!firstInGroup) return new SubtractExpression(null, e);
            e.Coefficient *= -1;
            return new SumExpression(null, e);
        }

        private void RemoveFromReorderable(IExpression expression)
        {
            if (expression == null)
            {
                return;
            }

            var ex = _reorderableExpressions.SingleOrDefault(e => e.Expression == expression);
            if (ex != null)
            {
                _reorderableExpressions.Remove(ex);
            }
        }

        private IExpression GetNextGroup(IList<ExpressionWithSide> expressions, ExpressionSide side)
        {
            IExpression currentGroup = null;
            Monomial nextExpression = null;

            while (expressions.Any())
            {
                var lastMonomial = nextExpression;
                var nextExpressionSide = expressions.First();
                nextExpression = nextExpressionSide.Expression as Monomial;
                var parent = nextExpression?.ParentExpression;
                var switchSide = nextExpressionSide.Side != side;
                if (lastMonomial != null &&
                    !lastMonomial.AreVariablesEqual(nextExpression))
                {
                    break;
                }
                if (currentGroup == null)
                {
                    currentGroup = nextExpression;
                    
                    var firstMonomialParent = GetLeftParentExpression(nextExpression, switchSide, true);
                    expressions.Remove(nextExpressionSide);
                    RemoveFromReorderable(parent);
                }
                else
                {
                    var nextParent = GetLeftParentExpression(nextExpression, switchSide, false);
                    nextParent.Operands[0] = currentGroup;
                    currentGroup.ParentExpression = nextParent;
                    currentGroup = nextParent;
                    expressions.Remove(nextExpressionSide);
                    RemoveFromReorderable(parent);
                }
            }
            return currentGroup;
        }

        private IExpression GetExpressionSide(
            IList<ExpressionWithSide> expressions,
            ExpressionSide side,
            bool addOther)
        {
            IExpression lastGroup = null;
            while (expressions.Any())
            {
                var currentGroup = GetNextGroup(expressions, side);
                var firstParent = currentGroup;
                if (lastGroup != null)
                {
                    firstParent = new SumExpression(lastGroup, currentGroup);
                    lastGroup.ParentExpression = firstParent;
                    currentGroup.ParentExpression = firstParent;
                }
                lastGroup = firstParent;
            }
            var remainingLeft = _reorderableExpressions.Where(e => e.Side == ExpressionSide.Left)
                .Select(e => e.Expression)
                .ToList();
            var remainingRight = _reorderableExpressions.Where(e => e.Side == ExpressionSide.Right)
                .Select(e => e.Expression)
                .ToList();
            if (!addOther) return lastGroup;
            lastGroup = _expressionAdder.AddExpressions(lastGroup, remainingLeft, false);
            lastGroup = _expressionAdder.AddExpressions(lastGroup, remainingRight, true);
            return lastGroup;
        }

        private bool SetReorderVariables(
            IEnumerable<FlatExpressionResult> leftFlatExpression,
            IEnumerable<FlatExpressionResult> rightFlatExpression,
            IExpression leftTop,
            IExpression rightTop
            )
        {
            var flatExpressionsLeft = leftFlatExpression.Select(f => new ExpressionWithSide
            {
                Side = ExpressionSide.Left,
                Expression = f.Expression
            }).ToList();

            if (flatExpressionsLeft.All(e => e.Expression != leftTop))
            {
                flatExpressionsLeft.Add(new ExpressionWithSide {Expression = leftTop, Side = ExpressionSide.Left});
            }

            var flatExpressionsRight = rightFlatExpression.Select(f => new ExpressionWithSide
            {
                Side = ExpressionSide.Right,
                Expression = f.Expression
            }).ToList();


            if (flatExpressionsRight.All(e => e.Expression != rightTop))
            {
                flatExpressionsRight.Add(new ExpressionWithSide { Expression = rightTop, Side = ExpressionSide.Right });
            }

            flatExpressionsLeft.AddRange(flatExpressionsRight);
            var flatExpressions = flatExpressionsLeft;

            var monomials = flatExpressions
                .Where(e => e.Expression is Monomial &&
                            (e.Expression.ParentExpression is SumExpression ||
                             e.Expression.ParentExpression is SubtractExpression) ||
                             e.Expression.ParentExpression is EqualityExpression)
                .ToList();

            _reorderableExpressions = flatExpressions
               .Where(r => !(r.Expression is Monomial ||
                           r.Expression is SumExpression ||
                           r.Expression is SubtractExpression))
               .ToList();

            _orderedMonomials = monomials.Where(m => !(m.Expression as Monomial)?.IsNumeral() ?? false)
                                         .OrderBy(m => m.Expression as Monomial, new MonomialsComparer())
                                         .ToList();

            _orderedNumerics = monomials.Where(m => (m.Expression as Monomial)?.IsNumeral() ?? false)
                                        .ToList();

            return _orderedMonomials.Count > 0 || _orderedNumerics.Count > 0;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var topValue = expression.Clone();
            var leftTop = expression.Operands[0];
            var rightTop = expression.Operands[1];
            var left = _flatener.FlattenExpression(expression.Operands[0], true, true);
            var right = _flatener.FlattenExpression(expression.Operands[1], true, true);
            var hasVariables = SetReorderVariables(left, right, leftTop, rightTop);
            if (!hasVariables)
            {
                return null;
            }
            var rightSide = GetExpressionSide(_orderedNumerics, ExpressionSide.Right, false);
            var leftSide = GetExpressionSide(_orderedMonomials, ExpressionSide.Left, true);
            var zero = new Monomial(0) {ParentExpression = expression};
            expression.Operands[0] = leftSide ?? zero;
            expression.Operands[1] = rightSide ?? zero;
            if (leftSide != null)
            {
                leftSide.ParentExpression = expression;
            }
            if (rightSide != null)
            {
                rightSide.ParentExpression = expression;
            }
            var applied = !topValue.IsEqualTo(expression);
            return applied ? new InnerRuleResult(expression) : null;
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            return expression is EqualityExpression;
        }

        public override string Description => "Equality Variables reorder";
    }
}
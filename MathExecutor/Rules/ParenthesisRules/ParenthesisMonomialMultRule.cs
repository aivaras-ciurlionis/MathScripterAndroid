using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.ParenthesisRules
{
    public class ParenthesisMonomialMultRule : AbstractRecursiveRule
    {
        private readonly IExpressionFlatener _expressionFlatener;
        private readonly IElementsChanger _elementsChanger;
        private readonly IParentChecker _parentChecker;

        public ParenthesisMonomialMultRule(
            IExpressionFlatener expressionFlatener,
            IElementsChanger elementsChanger,
            IParentChecker parentChecker)
        {
            _expressionFlatener = expressionFlatener;
            _elementsChanger = elementsChanger;
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var workingExpression = expression;
            var workingOperand = expression.Operands.First() as Monomial;
            var insideParenthesis = expression.Operands.Last().Operands[0];
            if (insideParenthesis == null || workingOperand == null)
            {
                return null;
            }
            var isNegative = !_parentChecker.LeftParentIsPositive(expression);
            if (isNegative)
            {
                workingOperand.Coefficient *= -1;
            }
            var insideElements = _expressionFlatener.FlattenExpression(insideParenthesis, true, true);
            var replacableElements = insideElements.Where(e => !(e.Expression is SumExpression ||
                                                               e.Expression is SubtractExpression));

            var flatExpressionResults = replacableElements as IList<FlatExpressionResult> ?? replacableElements.ToList();
            if (flatExpressionResults.Count == 1)
            {
                return new InnerRuleResult(new MultiplyExpression(workingOperand.Clone(true), 
                    flatExpressionResults.First().Expression.Clone(true)));
            }

            foreach (var element in flatExpressionResults)
            {
                var replacement = new MultiplyExpression(workingOperand.Clone(true), element.Expression.Clone(true));
                _elementsChanger.ChangeElement(element.Expression, replacement);
            }
            _elementsChanger.ChangeElement(workingExpression, insideParenthesis);
            return new InnerRuleResult(insideParenthesis, isNegative);
        }

        protected override bool CanBeApplied(IExpression expression)
        {

            return expression is MultiplyExpression &&
                   expression.Operands.First() is Monomial &&
                   expression.Operands?.Last() is ParenthesisExpression;
        }

        public override string Description => "Parenthesis multiplication";
    }
}
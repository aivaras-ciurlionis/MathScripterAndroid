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

        public ParenthesisMonomialMultRule(
            IExpressionFlatener expressionFlatener,
            IElementsChanger elementsChanger)
        {
            _expressionFlatener = expressionFlatener;
            _elementsChanger = elementsChanger;
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
            var isNegative = expression.ParentExpression is SubtractExpression;
            if (isNegative)
            {
                workingOperand.Coefficient *= -1;
            }
            var insideElements = _expressionFlatener.FlattenExpression(insideParenthesis, true, true);
            var replacableElements = insideElements.Where(e => !(e.Expression is SumExpression ||
                                                               e.Expression is SubtractExpression));
            foreach (var element in replacableElements)
            {
                var replacement = new MultiplyExpression(workingOperand.Clone(), element.Expression.Clone());
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
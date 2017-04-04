using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.ParenthesisRules
{
    public class ParenthesisRemovalRule : AbstractRecursiveRule
    {
        private readonly IExpressionFlatener _expressionFlatener;
        private readonly IElementsChanger _elementsChanger;

        public ParenthesisRemovalRule(IExpressionFlatener expressionFlatener, 
            IElementsChanger elementsChanger)
        {
            _expressionFlatener = expressionFlatener;
            _elementsChanger = elementsChanger;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var workingExpression = expression;
            var insideParenthesis = expression.Operands.Last().Operands[0];
            if (insideParenthesis == null)
            {
                return null;
            }
            var insideElements = _expressionFlatener.FlattenExpression(insideParenthesis, true, true);
            var replacableElements = insideElements.Where(e => !(e.Expression is SumExpression ||
                                                               e.Expression is SubtractExpression));
            foreach (var element in replacableElements)
            {
                var replacement = element.Expression;
                if (!(workingExpression is SumExpression))
                {
                    replacement = new NegationExpression(element.Expression.Clone());
                }
                _elementsChanger.ChangeElement(element.Expression, replacement);
            }
            if (workingExpression.Arity == 1)
            {
                insideParenthesis.ParentExpression = workingExpression.ParentExpression;
                return new InnerRuleResult(insideParenthesis);
            }
            if (workingExpression is SubtractExpression)
            {
                var sum = new SumExpression(workingExpression.Operands[0], insideParenthesis)
                {
                    ParentExpression = workingExpression.ParentExpression
                };
                return new InnerRuleResult(sum);
            }
            _elementsChanger.ChangeElement(workingExpression.Operands[1], insideParenthesis);
            return new InnerRuleResult(workingExpression);
        }

        protected override bool CanBeApplied(IExpression expression)
        {

            return (expression is SumExpression ||
                    expression is SubtractExpression ||
                    expression is NegationExpression) &&
                    expression.Operands?.Last() is ParenthesisExpression;
        }

        public override string Description => "Parenthesis removal";
    }
}
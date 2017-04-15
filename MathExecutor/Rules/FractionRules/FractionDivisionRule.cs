using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.FractionRules
{
    public class FractionDivisionRule : AbstractRecursiveRule
    {
        private readonly IParentChecker _parentChecker;
        private readonly IElementsChanger _elementsChanger;

        public FractionDivisionRule(IParentChecker parentChecker,
            IElementsChanger elementsChanger)
        {
            _parentChecker = parentChecker;
            _elementsChanger = elementsChanger;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var divisionLeft = _parentChecker.GetUnderParenthesis(expression.Operands[0]);
            var divisionRight = _parentChecker.GetUnderParenthesis(expression.Operands[1]);
            var rightTop = divisionRight.Operands[0];
            var rightBot = divisionRight.Operands[1];
            var newRight = new DivisionExpression(rightBot, rightTop);
            _elementsChanger.ChangeElement(divisionRight, newRight);
            var result = new MultiplyExpression(divisionLeft, newRight);
            return new InnerRuleResult(result);
        }
       
        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is DivisionExpression))
            {
                return false;
            }
            return _parentChecker.GetUnderParenthesis(expression.Operands[0]) is DivisionExpression &&
                   _parentChecker.GetUnderParenthesis(expression.Operands[1]) is DivisionExpression;
        }

        public override string Description => "Fraction product";
    }
}
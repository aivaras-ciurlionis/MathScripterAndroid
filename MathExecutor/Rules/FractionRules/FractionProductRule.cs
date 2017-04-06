using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.FractionRules
{
    public class FractionProductRule : AbstractRecursiveRule
    {
        private readonly IParentChecker _parentChecker;

        public FractionProductRule(IParentChecker parentChecker)
        {
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var parent = expression.ParentExpression;
            var divisionLeft = _parentChecker.GetUnderParenthesis(expression.Operands[0]);
            var divisionRight = _parentChecker.GetUnderParenthesis(expression.Operands[1]);

            var leftTop = divisionLeft.Operands[0];
            var leftBot = divisionLeft.Operands[1];

            var rightTop = divisionRight.Operands[0];
            var rightBot = divisionRight.Operands[1];

            var newTop = new MultiplyExpression(leftTop, rightTop);
            var newBot = new MultiplyExpression(leftBot, rightBot);

            var result = new DivisionExpression(newTop, newBot) {ParentExpression = parent};
            return new InnerRuleResult(result);
        }

       
        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is MultiplyExpression))
            {
                return false;
            }
            return _parentChecker.GetUnderParenthesis(expression.Operands[0]) is DivisionExpression &&
                   _parentChecker.GetUnderParenthesis(expression.Operands[1]) is DivisionExpression;
        }

        public override string Description => "Fraction product";
    }
}
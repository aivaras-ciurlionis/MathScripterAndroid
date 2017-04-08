using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.FractionRules
{
    public class CommonDenominatorSum : AbstractRecursiveRule
    {
        private readonly IParentChecker _parentChecker;

        public CommonDenominatorSum(IParentChecker parentChecker)
        {
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var parent = expression.ParentExpression;

            var divisionLeft = _parentChecker.GetUnderParenthesis(expression.Operands[0]);
            var divisionRight = _parentChecker.GetUnderParenthesis(expression.Operands[1]);

            var isSubtract = expression is SubtractExpression;
            var isNegative = !_parentChecker.LeftParentIsPositive(divisionLeft);

            var leftTop = divisionLeft.Operands[0];
            var leftBot = divisionLeft.Operands[1];

            var adjustedLeftTop = isNegative ? new NegationExpression(leftTop) : leftTop;

            var rightTop = divisionRight.Operands[0];
            var rightBot = divisionRight.Operands[1];

            if (!leftBot.IsEqualTo(rightBot))
            {
                return null;
            }
            var newBot = leftBot;
            IExpression newTop;
            if (isSubtract)
            {
                newTop = new SubtractExpression(adjustedLeftTop, rightTop);
            }
            else
            {
                newTop = new SumExpression(adjustedLeftTop, rightTop);
            }
            var result = new DivisionExpression(newTop, newBot);
            return new InnerRuleResult(result, isNegative);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is SumExpression || expression is SubtractExpression))
            {
                return false;
            }
            return _parentChecker.GetUnderParenthesis(expression.Operands[0]) is DivisionExpression &&
                   _parentChecker.GetUnderParenthesis(expression.Operands[1]) is DivisionExpression;
        }

        public override string Description => "Fraction sum with same denominator";
    }
}
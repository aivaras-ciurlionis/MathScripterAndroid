using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;

namespace MathExecutor.Helpers
{
    public class ParentChecker : IParentChecker
    {
        public bool LeftParentIsPositive(IExpression expression)
        {
            IExpression parent;
            if (expression.ParentExpression?.Operands != null &&
                expression.ParentExpression.Operands.Last() == expression)
            {
                parent = expression.ParentExpression;
            }
            else
            {
                parent = null;
            }
            return !(parent is SubtractExpression) && !(parent is NegationExpression);
        }

        public IExpression GetUnderParenthesis(IExpression expression)
        {
            if (expression is ParenthesisExpression)
            {
                return expression.Operands[0];
            }
            return expression;
        }
    }
}
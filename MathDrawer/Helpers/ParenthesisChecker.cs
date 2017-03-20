using MathDrawer.Interfaces;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;

namespace MathDrawer.Helpers
{
    public class ParenthesisChecker : IParenthesisChecker
    {
        public bool NeedsParenthesis(IExpression expression)
        {
            var parentExpression = expression.ParentExpression;
            if (parentExpression == null) return false;
            return !(parentExpression is DivisionExpression ||
                   parentExpression is ExponentExpression ||
                   parentExpression is SqrRootExpression);
        }
    }
}
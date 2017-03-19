using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IParenthesisChecker
    {
        bool NeedsParenthesis(IExpression expression);
    }
}
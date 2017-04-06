namespace MathExecutor.Interfaces
{
    public interface IParentChecker
    {
        bool LeftParentIsPositive(IExpression expression);
        IExpression GetUnderParenthesis(IExpression expression);
    }
}
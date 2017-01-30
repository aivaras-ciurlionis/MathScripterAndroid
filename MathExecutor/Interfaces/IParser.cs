namespace MathExecutor.Interfaces
{
    public interface IParser
    {
        IExpression Parse(string equation);
    }
}
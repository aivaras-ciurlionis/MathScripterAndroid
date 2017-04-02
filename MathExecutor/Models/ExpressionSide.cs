using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public enum ExpressionSide
    {
        Left = 0,
        Right = 1
    }

    public class ExpressionWithSide
    {
        public IExpression Expression { get; set; }
        public ExpressionSide Side { get; set; }
    }

}
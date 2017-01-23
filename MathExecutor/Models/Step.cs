using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class Step
    {
        public IExpression FullExpression { get; set; }
        public IExpression ComputedExpression { get; set; }
        public IExpression ExpressionResult { get; set; }
    }
}
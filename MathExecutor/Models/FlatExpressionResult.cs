
using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class FlatExpressionResult
    {
        public IExpression Expression { get; set; }
        public int Level { get; set; }
        public int Number { get; set; }
    }
}
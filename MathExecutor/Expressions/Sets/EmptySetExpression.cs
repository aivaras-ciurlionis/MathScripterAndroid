using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Sets
{
    public class EmptySetExpression : AbstractNullaryExpression
    {
        public override int Order => 8;
        public override IExpression Clone()
        {
            return new EmptySetExpression();
        }

        public override string Name => "\u2205";
        protected override double Value => -1;
        public override bool CanBeExecuted() => false;

        public override string ToString()
        {
            return $"{Name}";
        }

        public override ExpressionType Type => ExpressionType.Set;
    }
}
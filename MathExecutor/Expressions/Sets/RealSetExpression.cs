using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Sets
{
    public class RealSetExpression : AbstractNullaryExpression
    {
        public RealSetExpression(string id = null) : base(id)
        {
        }

        public override int Order => 8;
        public override IExpression Clone()
        {
            return new RealSetExpression(Id);
        }

        public override string Name => "R";
        protected override double Value => -1;
        public override bool CanBeExecuted() => false;

        public override string ToString()
        {
            return $"{Name}";
        }

        public override ExpressionType Type => ExpressionType.Set;
    }
}
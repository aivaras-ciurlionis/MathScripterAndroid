using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractUnaryExpression : AbstractExpression
    {
        protected AbstractUnaryExpression(IExpression operand) : base(new List<IExpression> { operand })
        {
        }

        public override int Arity => 1;

        public abstract override IExpression InnerExecute();
        public abstract override ExpressionType Type { get; }
        public abstract override int Order { get; }
        public abstract override bool CanBeExecuted();
        public abstract override IExpression Clone();
    }
}
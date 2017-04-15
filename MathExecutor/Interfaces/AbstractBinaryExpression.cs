using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractBinaryExpression : AbstractExpression
    {
        protected AbstractBinaryExpression(
            IExpression leftOperand,
            IExpression rightOperand,
            string id = null) : base(new List<IExpression> {leftOperand, rightOperand}, id)
        {
        }

        public override int Arity => 2;

        public abstract override IExpression InnerExecute();
        public abstract override bool CanBeExecuted();
        public abstract override ExpressionType Type { get; }
        public abstract override int Order { get; }
        public abstract override IExpression Clone(bool changeId);
    }
}
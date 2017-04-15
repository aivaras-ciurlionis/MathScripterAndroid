using System;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class SqrRootExpression : AbstractUnaryExpression
    {
        public SqrRootExpression(IExpression operand, string id = null) : base(operand, id)
        {
        }

        public override IExpression Clone(bool changeId)
        {
            return new SqrRootExpression(Operands[0].Clone(changeId), changeId ? null : Id);
        }

        public override IExpression InnerExecute()
        {
            var op = Operands[0] as Monomial;
            if (op == null)
            {
                return this;
            }
            return new Monomial(Math.Sqrt(op.Coefficient), ParentExpression);
        }

        public override ExpressionType Type => ExpressionType.Root;
        public override int Order => 1;

        public override string ToString()
        {
            return $"sqrt({Operands[0]})";
        }

        public override bool CanBeExecuted()
        {
            var op = Operands[0] as Monomial;
            return op != null && op.IsNumeral();
        }

        public override string Name => "sqrt";
    }
}
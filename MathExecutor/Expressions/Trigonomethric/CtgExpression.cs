using System;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Trigonomethric
{
    public class CtgExpression : AbstractUnaryExpression
    {
        public CtgExpression(IExpression operand) : base(operand)
        {
        }

        public override IExpression Clone()
        {
            return new CtgExpression(Operands[0].Clone());
        }

        public override IExpression InnerExecute()
        {
            var op = Operands[0] as Monomial;
            if (op == null)
            {
                return this;
            }
            return new Monomial(1 / Math.Tan(op.Coefficient));
        }

        public override ExpressionType Type => ExpressionType.Trigonometric;
        public override int Order => 1;

        public override string ToString()
        {
            return $"ctg {Operands[0]}";
        }

        public override bool CanBeExecuted()
        {
            var op = Operands[0] as Monomial;
            return op != null && op.IsNumeral();
        }

        public override string Name => "ctg";
    }
}
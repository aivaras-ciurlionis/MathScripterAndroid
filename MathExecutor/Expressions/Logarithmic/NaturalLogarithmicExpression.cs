using System;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Logarithmic
{
    public class NaturalLogarithmicExpression : AbstractUnaryExpression
    {
        public NaturalLogarithmicExpression(IExpression operand) : base(operand)
        {
        }

        public override IExpression Clone()
        {
            return new NaturalLogarithmicExpression(Operands[0].Clone());
        }

        public override IExpression InnerExecute()
        {
            var op = Operands[0] as Monomial;
            if (op == null)
            {
                return this;
            }
            return new Monomial(Math.Log(Math.E, op.Coefficient), ParentExpression);
        }

        public override ExpressionType Type => ExpressionType.Logarithmic;
        public override int Order => 1;

        public override string ToString()
        {
            return $"ln {Operands[0]}";
        }

        public override bool CanBeExecuted()
        {
            var op = Operands[0] as Monomial;
            return op != null && op.IsNumeral();
        }

        public override string Name => "ln";
    }
}
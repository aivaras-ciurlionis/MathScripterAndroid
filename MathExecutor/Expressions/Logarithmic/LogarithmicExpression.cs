using System;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Logarithmic
{
    public class LogarithmicExpression : AbstractBinaryExpression
    {
        public LogarithmicExpression(IExpression leftOperand, IExpression rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override IExpression InnerExecute()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;
            if (left == null || right == null)
            {
                return this;
            }
            return new Monomial(Math.Log(left.Coefficient, right.Coefficient), ParentExpression);
        }

        public override int Order => 3;

        public override bool CanBeExecuted()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;
            if (left == null || right == null)
            {
                return false;
            }
            return left.IsNumeral() && right.IsNumeral();
        }

        public override ExpressionType Type => ExpressionType.Arithmetic;

        public override string ToString()
        {
            return $"{Operands[0]} log {Operands[1]}";
        }

        public override IExpression Clone()
        {
            return new SumExpression(Operands[0].Clone(), Operands[1].Clone());
        }

        public override string Name => "log";
    }
}
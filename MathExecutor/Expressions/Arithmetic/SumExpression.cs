using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class SumExpression : AbstractBinaryExpression
    {
        public SumExpression(IExpression leftOperand, IExpression rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override IExpression InnerExecute()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;
            return new Monomial(left.Coefficient + right.Coefficient, left.Variables);
        }

        public override int Order => 3;

        public override bool CanBeExecuted()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;
            return left != null && left.AreVariablesEqual(right);
        }

        public override ExpressionType Type => ExpressionType.Arithmetic;

        public override string ToString()
        {
            return $"{Operands[0]} + {Operands[1]}";
        }

        public override IExpression Clone()
        {
            return new SumExpression(Operands[0].Clone(), Operands[1].Clone());
        }

    }
}
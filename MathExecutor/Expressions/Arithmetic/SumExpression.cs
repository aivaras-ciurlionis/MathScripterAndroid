using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class SumExpression : AbstractBinaryExpression
    {
        public SumExpression(IExpression leftOperand, IExpression rightOperand) : base(leftOperand, rightOperand)
        {
        }

        protected override IExpression InnerExecute()
        {
            var left = LeftOperand as Monomial;
            var right = RightOperand as Monomial;
            return new Monomial(left.Coefficient + right.Coefficient, left.Variables);
        }

        public override int Order => 2;

        public override bool CanBeExecuted()
        {
            var left = LeftOperand as Monomial;
            var right = RightOperand as Monomial;
            return left != null && left.AreVariablesEqual(right);
        }

        public override ExpressionType Type => ExpressionType.Arithmetic;

        public override string ToString()
        {
            return $"{LeftOperand} + {RightOperand}";
        }

        public override IExpression Clone()
        {
            return new SumExpression(LeftOperand.Clone(), RightOperand.Clone());
        }

    }
}
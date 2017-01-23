using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class SubtractExpression : AbstractBinaryExpression
    {
        public SubtractExpression(IExpression leftOperand, IExpression rightOperand) : base(leftOperand, rightOperand)
        {
        }

        protected override IExpression InnerExecute()
        {
            var left = LeftOperand as Monomial;
            var right = RightOperand as Monomial;
            return new Monomial
            {
                Coefficient = left.Coefficient - right.Coefficient,
                Variables = left.Variables
            };
        }

        public override int Order => 2;

        public override bool CanBeExecuted()
        {
            var left = LeftOperand as Monomial;
            var right = RightOperand as Monomial;
            return left != null && left.IsEqual(right);
        }

        public override ExpressionType Type => ExpressionType.Arithmetic;
    }
}
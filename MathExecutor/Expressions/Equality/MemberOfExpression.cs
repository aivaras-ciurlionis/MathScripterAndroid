using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Equality
{
    public class MemberOfExpression : AbstractBinaryExpression
    {
        public MemberOfExpression(IExpression leftOperand, IExpression rightOperand, string id = null)
            : base(leftOperand, rightOperand, id)
        {
        }

        public override IExpression Clone()
        {
            return new MemberOfExpression(Operands[0].Clone(), Operands[1].Clone(), Id);
        }

        public override IExpression InnerExecute()
        {
            return this;
        }

        public override int Order => 10;

        public override bool CanBeExecuted()
        {
            return false;
        }

        public override ExpressionType Type => ExpressionType.Equation;

        public override string ToString()
        {
            return $"{Operands[0]} \u2208 {Operands[1]}";
        }

        public override string Name => "\u2208";
    }
}
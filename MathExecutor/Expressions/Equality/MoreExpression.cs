using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Equality
{
    public class MoreExpression : AbstractBinaryExpression
    {
        public MoreExpression(IExpression leftOperand, IExpression rightOperand, string id = null)
            : base(leftOperand, rightOperand, id)
        {
        }

        public override IExpression Clone()
        {
            return new MoreExpression(Operands[0].Clone(), Operands[1].Clone(), Id);
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
            return $"{Operands[0]} > {Operands[1]}";
        }

        public override string Name => ">";
    }
}
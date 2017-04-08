using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions
{
    public class SeparationExpression : AbstractBinaryExpression
    {
        public SeparationExpression(IExpression leftOperand, IExpression rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override IExpression Clone()
        {
            return new SeparationExpression(Operands[0].Clone(), Operands[1].Clone());
        }

        public override IExpression InnerExecute()
        {
            return this;
        }

        public override int Order => 15;

        public override bool CanBeExecuted()
        {
            return false;
        }

        public override ExpressionType Type => ExpressionType.List;

        public override string ToString()
        {
            return $"{Operands[0]} , {Operands[1]}";
        }

        public override string Name => ",";
    }
}
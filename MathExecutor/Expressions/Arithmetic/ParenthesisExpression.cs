using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class ParenthesisExpression : AbstractUnaryExpression
    {
        public ParenthesisExpression(IExpression operand) : base(operand)
        {
        }

        public override IExpression Clone()
        {
            return new ParenthesisExpression(Operands.First().Clone());
        }

        public override IExpression InnerExecute()
        {
            return Operands.First();
        }

        public override string ToString()
        {
            return $"({Operands[0]})";
        }

        public override ExpressionType Type => ExpressionType.Arithmetic;
        public override int Order => 5;
        public override bool CanBeExecuted() => true;
        public override string Name => "()";
    }
}
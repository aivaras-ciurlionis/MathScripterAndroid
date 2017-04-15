using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class ParenthesisExpression : AbstractUnaryExpression
    {
        public ParenthesisExpression(IExpression operand, string id = null) : base(operand, id)
        {
        }

        public override IExpression Clone(bool changeId)
        {
            return new ParenthesisExpression(Operands.First().Clone(changeId), changeId ? null : Id);
        }

        public override IExpression InnerExecute()
        {
            var result = Operands.First();
            result.ParentExpression = ParentExpression;
            return result;
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
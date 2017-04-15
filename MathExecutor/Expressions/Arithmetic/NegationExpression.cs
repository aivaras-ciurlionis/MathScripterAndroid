using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class NegationExpression : AbstractUnaryExpression
    {
        public NegationExpression(IExpression operand, string id = null) : base(operand, id)
        {
        }

        public override IExpression Clone(bool changeId)
        {
           return new NegationExpression(Operands[0].Clone(changeId), changeId ? null : Id);
        }

        public override IExpression InnerExecute()
        {
            var operand = Operands[0] as Monomial;
            return new Monomial
            (
                -1 * operand.Coefficient,
                operand.Variables,
                ParentExpression
            );
        }

        public override ExpressionType Type => ExpressionType.Arithmetic;
        public override int Order => 1;
        public override bool CanBeExecuted() => true;

        public override string ToString()
        {
            return $"-{Operands[0]}";
        }

        public override string Name => "-";
    }
}
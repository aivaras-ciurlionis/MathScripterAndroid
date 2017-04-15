using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Equality
{
    public class LessEqualExpression : AbstractBinaryExpression
    {
        public LessEqualExpression(IExpression leftOperand, IExpression rightOperand, string id = null) 
            : base(leftOperand, rightOperand, id)
        {
        }

        public override IExpression Clone(bool changeId)
        {
            return new LessEqualExpression(Operands[0].Clone(changeId), Operands[1].Clone(changeId), changeId ? null : Id);
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
            return $"{Operands[0]} <= {Operands[1]}";
        }

        public override string Name => "<=";
    }
}
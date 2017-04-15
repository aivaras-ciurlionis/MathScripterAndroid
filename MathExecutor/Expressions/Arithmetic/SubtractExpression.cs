using System;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class SubtractExpression : AbstractBinaryExpression
    {
        public SubtractExpression(IExpression leftOperand, IExpression rightOperand, string id = null) : base(leftOperand, rightOperand, id)
        {
        }

        public override IExpression InnerExecute()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;
            return new Monomial(left.Coefficient - right.Coefficient, left.Variables, ParentExpression);
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
            return $"{Operands[0]} - {Operands[1]}";
        }

        public override IExpression Clone(bool changeId)
        {
            return new SubtractExpression(Operands[0].Clone(changeId), 
                Operands[1].Clone(changeId), changeId ? null : Id);
        }

        public override string Name => "-";
    }
}
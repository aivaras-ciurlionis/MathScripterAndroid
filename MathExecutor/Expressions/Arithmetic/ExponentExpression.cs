using System;
using System.Collections.Generic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class ExponentExpression : AbstractBinaryExpression
    {
        public ExponentExpression(IExpression leftOperand, IExpression rightOperand, string id = null) 
            : base(leftOperand, rightOperand, id)
        {
        }

        public override IExpression Clone(bool changeId)
        {
            return new ExponentExpression(Operands[0].Clone(changeId), Operands[1].Clone(changeId), changeId ? null : Id);
        }

        public override IExpression InnerExecute()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;

            if (Math.Abs(left.Coefficient) < 0.001 && 
                Math.Abs(right.Coefficient) < 0.001)
            {
                throw new ArithmeticException("Cannot exponentiate 0 by 0");
            }

            var exponent = right.Coefficient;

            var newVariables = new List<IVariable>();
            foreach (var variable in left.Variables ?? new List<IVariable>())
            {
                newVariables.Add(new Variable {Name = variable.Name, Exponent = variable.Exponent*exponent});
            }

            return new Monomial(Math.Pow(left.Coefficient, exponent), newVariables, ParentExpression);
        }

        public override int Order => 1;

        public override bool CanBeExecuted()
        {
            var right = Operands[1] as Monomial;
            return right != null && right.IsNumeral();
        }

        public override ExpressionType Type => ExpressionType.Exponent;

        public override string ToString()
        {
            return $"{Operands[0]} ^ {Operands[1]}";
        }

        public override string Name => "^";
    }
}
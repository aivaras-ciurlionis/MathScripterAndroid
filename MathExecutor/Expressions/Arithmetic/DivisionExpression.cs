using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions.Arithmetic
{
    public class DivisionExpression : AbstractBinaryExpression
    {
        public DivisionExpression(IExpression leftOperand, IExpression rightOperand,
            string id = null) : base(leftOperand, rightOperand, id)
        {
        }

        public override IExpression Clone()
        {
            return new DivisionExpression(Operands[0].Clone(), Operands[1].Clone(), Id);
        }

        public override IExpression InnerExecute()
        {
            var left = Operands[0] as Monomial;
            var right = Operands[1] as Monomial;

            if (Math.Abs(right.Coefficient) < 0.001)
            {
                throw new ArithmeticException("Division by zero:!");
            }

            var topVariables = new List<IVariable>();
            var botVariables = right.Variables?.ToList() ?? new List<IVariable>();

            foreach (var variable in left?.Variables ?? new List<IVariable>())
            {
                var rightVar = botVariables.FirstOrDefault(v => v.Name == variable.Name);
                var rightExponent = rightVar?.Exponent ?? 0;

                if (rightVar != null)
                {
                    botVariables.Remove(rightVar);
                }

                var newVariable = new Variable { Name = variable.Name, Exponent = variable.Exponent - rightExponent };

                if (newVariable.Exponent > 0.001)
                {
                    topVariables.Add(newVariable);
                }

                if (newVariable.Exponent < -0.001)
                {
                    newVariable.Exponent = -newVariable.Exponent;
                    botVariables.Add(newVariable);
                }
            }

            if (botVariables.Count < 1)
            {
                return new Monomial(left.Coefficient / right.Coefficient, topVariables, ParentExpression);
            }

            return new DivisionExpression(new Monomial(left.Coefficient / right.Coefficient, topVariables, ParentExpression),
                new Monomial(1, botVariables, ParentExpression));
        }

        public override int Order => 2;

        public override bool CanBeExecuted() => true;

        public override ExpressionType Type => ExpressionType.Rational;

        public override string ToString()
        {
            return $"{Operands[0]} / {Operands[1]}";
        }

        public override string Name => "/";
    }
}
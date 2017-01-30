using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class Monomial : IExpression
    {
        public double Coefficient { get; set; }
        public IEnumerable<IVariable> Variables { get; set; }

        public Monomial(double coefficient)
        {
            Coefficient = coefficient;
        }

        public Monomial(double coefficient, IEnumerable<IVariable> variables)
        {
            Coefficient = coefficient;
            Variables = variables;
        }

        public IExpression Execute()
        {
            return this;
        }

        public ExpressionType Type => ExpressionType.Terminal;
        public int Arity => 0;
        public int Order => 0;
        public bool CanBeExecuted() => true;

        public bool IsNumeral() => Variables == null || !Variables.Any();

        public bool AreVariablesEqual(Monomial other)
        {
            if (IsNumeral() && other.IsNumeral())
            {
                return true;
            }

            if (other.Variables == null || Variables == null)
            {
                return false;
            }

            return other.Variables.All(o => Variables.Any(v => v.IsEqualTo(o)));
        }

        public bool IsEqual(Monomial other)
        {
            return Math.Abs(Coefficient - other.Coefficient) < 0.001 && AreVariablesEqual(other);
        }

        public IExpression ParentExpression { get; set; }

        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
        }

        public IExpression ReplaceVariables(Dictionary<string, double> values)
        {
            var product = Variables?.Aggregate(Coefficient, (current, variable) =>
            current * (values.ContainsKey(variable.Name) ? variable.Evaluate(values[variable.Name]) : 1)) ?? Coefficient;
            return new Monomial(product);
        }

        public override string ToString()
        {
            return Variables?.Aggregate(Coefficient.ToString(CultureInfo.InvariantCulture),
                (text, variable) => text + variable.ToString())
                ?? Coefficient.ToString(CultureInfo.InvariantCulture);
        }

        public IExpression Clone()
        {
            return new Monomial(Coefficient, Variables);
        }

        public IList<IExpression> Operands => null;
    }
}
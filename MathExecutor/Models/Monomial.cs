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

        public Monomial(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public Monomial(double coefficient, string id = null) : this(id)
        {
            Coefficient = coefficient;
        }

        public Monomial(double coefficient, IExpression parent, string id = null) : this(id)
        {
            Coefficient = coefficient;
            ParentExpression = parent;
        }

        public Monomial(double coefficient, char variableName, string id = null) : this(id)
        {
            Coefficient = coefficient;
            Variables = new List<IVariable> {new Variable {Exponent = 1, Name = variableName.ToString()} };
        }

        public Monomial(double coefficient, IEnumerable<IVariable> variables, string id = null) : this(id)
        {
            Coefficient = coefficient;
            Variables = variables;
        }

        public Monomial(double coefficient, IEnumerable<IVariable> variables, IExpression parent, string id = null) : this(id)
        {
            Coefficient = coefficient;
            Variables = variables;
            ParentExpression = parent;
        }

        public IExpression Execute()
        {
            return this;
        }

        public IEnumerable<string> GetVariables()
        {
            return IsNumeral() 
                   ? new List<string>()
                   : Variables.Select(v => v.Name);
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

            return other.Variables.Count() == Variables.Count() &&
                other.Variables.All(o => Variables.Any(v => v.IsEqualTo(o)));
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

        public bool IsNumericWithCoeficient(double coef)
        {
            return IsNumeral() && Math.Abs(Coefficient - coef) < 0.001;
        }

        public bool HasSingleVariableWithExponent(double exponent, string name="")
        {
            var single = Variables != null && Variables.Count() == 1;
            if (!single)
            {
                return false;
            }
            return Math.Abs(Variables.First().Exponent - exponent) < 0.001 && 
                   string.IsNullOrWhiteSpace(name) || Variables.First().Name == name;
        }

        private string GetCoefficientString()
        {
            if (Coefficient < 0)
            {
                return Math.Abs(Coefficient + 1) < 0.001 && !IsNumeral()
                    ? "-"
                    : Coefficient.ToString(CultureInfo.InvariantCulture);
            }
            return Math.Abs(Coefficient - 1) < 0.001 && !IsNumeral() ? "" : 
                Coefficient.ToString(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return Variables?.Aggregate(GetCoefficientString(),
                (text, variable) => text + variable.ToString())
                ?? GetCoefficientString();
        }

        public IExpression Clone(bool changeId)
        {
            return new Monomial(Coefficient, Variables, changeId ? null : Id);
        }

        public IList<IExpression> Operands => null;
        public string Name => "MONOMIAL";

        public bool IsEqualTo(IExpression other)
        {
            var otherMonomial = other as Monomial;
            return otherMonomial != null && IsEqual(otherMonomial);
        }

        public string Id { get; set; }
        public IExpression Clone()
        {
            return Clone(false);
        }
    }
}
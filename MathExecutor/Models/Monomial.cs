using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class Monomial : IExpression
    {
        public decimal Coefficient { get; set; }
        public IEnumerable<Variable> Variables { get; set; }

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
            return other.Variables.All(
                o => Variables.Any(v => v.Name == o.Name && v.Exponent == o.Exponent));
        }

        public bool IsEqual(Monomial other)
        {
            return Coefficient == other.Coefficient && AreVariablesEqual(other);
        }

        public IExpression ParentExpression { get; set; }

        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
        }
    }
}
using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractNullaryExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Arithmetic;
        public int Arity => 0;
        public abstract int Order { get; }
        public bool CanBeExecuted() => true;
        protected abstract double Value { get; }

        public IExpression Execute()
        {
            return new Monomial(Value);
        }
        public IExpression ParentExpression { get; set; }
        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
        }

        public IExpression ReplaceVariables(Dictionary<string, double> values)
        {
            return this;
        }

        public abstract IExpression Clone();

        public IList<IExpression> Operands => null;
        public abstract string Name { get; }
    }
}
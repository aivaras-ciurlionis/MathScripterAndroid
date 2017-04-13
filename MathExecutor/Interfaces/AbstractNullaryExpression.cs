using System;
using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractNullaryExpression : IExpression
    {
        public abstract ExpressionType Type { get; }
        public int Arity => 0;
        public abstract int Order { get; }
        public abstract bool CanBeExecuted();
        protected abstract double Value { get; }

        protected AbstractNullaryExpression(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public IExpression Execute()
        {
            if (CanBeExecuted())
            {
                return new Monomial(Value);
            }
            return this;
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
        public bool IsEqualTo(IExpression other)
        {
            var otherE = other as AbstractNullaryExpression;
            return otherE != null && otherE.Name == Name;
        }

        public string Id { get; set; }
    }
}
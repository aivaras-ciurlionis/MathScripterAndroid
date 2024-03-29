using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractExpression : IExpression
    {
        protected AbstractExpression(IList<IExpression> operands, string id = null)
        {
            Operands = operands;
            foreach (var expression in operands)
            {
                if (expression != null)
                {
                    expression.ParentExpression = this;
                }
            }
            Id = id ?? Guid.NewGuid().ToString();
        }

        public IList<IExpression> Operands { get; set; }
        public IExpression ParentExpression { get; set; }
        public abstract ExpressionType Type { get; }
        public abstract int Arity { get; }
        public abstract int Order { get; }
        public abstract bool CanBeExecuted();

        public IExpression Execute()
        {
            for (var i = 0; i < Operands.Count; i++)
            {
                Operands[i] = Operands[i].Execute();
            }
            if (!Operands.All(o => o is Monomial) || !CanBeExecuted()) return this;
            var expressionBefore = this;
            var result = InnerExecute();
            AddStep(expressionBefore, result);
            return result;
        }

        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
            ParentExpression?.AddStep(expressionBefore, expressionAfter);
        }

        public IExpression ReplaceVariables(Dictionary<string, double> values)
        {
            for (var i = 0; i < Operands.Count; i++)
            {
                var e = Operands[i].ReplaceVariables(values);
                Operands[i] = e;
                e.ParentExpression = this;
            }
            return this;
        }

        public IExpression Clone()
        {
            return Clone(false);
        }

        public abstract IExpression InnerExecute();
        public abstract override string ToString();
        public abstract string Name { get; }
        public virtual bool IsEqualTo(IExpression other)
        {
            if (other.Name != Name ||
                other.Arity != Arity)
            {
                return false;
            }

            for (var i = 0; i < Operands.Count; i++)
            {
                if (!Operands[i].IsEqualTo(other.Operands[i]))
                {
                    return false;
                };
            }
            return true;
        }

        public string Id { get; set; }
        public abstract IExpression Clone(bool changeId);
    }
}
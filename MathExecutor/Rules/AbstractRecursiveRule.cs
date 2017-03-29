using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules
{
    public abstract class AbstractRecursiveRule : IRule
    {
        private IExpression ApplyRuleRecursive(IExpression expression)
        {
            var result = CanBeApplied(expression) ? ApplyRuleInner(expression) : null;
            if (result != null)
            {
                return result;
            }
            return (
                from operand in expression.Operands ?? new List<IExpression>()
                select ApplyRuleRecursive(operand)
                )
                .FirstOrDefault(operandResult => operandResult != null);
        }

        public RuleApplyResult ApplyRule(IExpression expression)
        {
            var result = ApplyRuleRecursive(expression);
            return new RuleApplyResult
            {
                Applied = result != null,
                Expression = result,
                RuleDescription = Description
            };
        }

        protected abstract IExpression ApplyRuleInner(IExpression expression);
        protected abstract bool CanBeApplied(IExpression expression);

        public abstract string Description { get; }
    }
}
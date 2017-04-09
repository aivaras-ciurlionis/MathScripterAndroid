using System.Collections.Generic;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules
{
    public abstract class AbstractRecursiveRule : IRule
    {
        private InnerRuleResult ApplyRuleRecursive(IExpression expression)
        {
            var result = CanBeApplied(expression) ? ApplyRuleInner(expression) : null;
            if (result != null)
            {
                return result;
            }
            var operands = expression.Operands ?? new List<IExpression>();
            for (var i = 0; i < operands.Count; i++)
            {
                var operandResult = ApplyRuleRecursive(operands[i]);
                if (operandResult == null) continue;
                operandResult.Expression.ParentExpression = expression;
                operands[i] = operandResult.Expression;
                if (operandResult.ConsumeParent)
                {
                    expression = new SumExpression(operands[0], operandResult.Expression);
                }
                return new InnerRuleResult (expression, false, operandResult.HelperSteps);
            }
            return null;
        }

        public RuleApplyResult ApplyRule(IExpression expression)
        {
            var result = ApplyRuleRecursive(expression);
            return new RuleApplyResult
            {
                Applied = result != null,
                Expression = result?.Expression,
                RuleDescription = Description,
                HelperExpressions = result?.HelperSteps,
                RuleType = GetType()
            };
        }

        protected abstract InnerRuleResult ApplyRuleInner(IExpression expression);
        protected abstract bool CanBeApplied(IExpression expression);

        public abstract string Description { get; }
    }
}
using System.Collections.Generic;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Rules.FinalRules;

namespace MathExecutor.RuleBinders
{
    public class MultiRuleChecher : IMultiRuleChecker
    {
        private readonly IList<IRule> _rules = new List<IRule>
        {
            new LinearEquationRule()
        };

        public IList<Step> ApplyRules(IExpression expression)
        {
            var results = new List<Step>();
            foreach (var rule in _rules)
            {
                var result = rule.ApplyRule(expression.Clone());
                if (result.Applied)
                {
                    results.Add(new Step
                    {
                        FullExpression = result.Expression,
                        RuleDescription = result.RuleDescription
                    });
                }
            }
            return results;
        }
    }
}
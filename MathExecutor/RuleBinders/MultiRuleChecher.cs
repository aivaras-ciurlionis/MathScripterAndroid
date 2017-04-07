using System.Collections.Generic;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Rules.FinalRules;
using MathExecutor.Rules.FractionRules;
using MathExecutor.Rules.MonomialRules;
using MathExecutor.Rules.ParenthesisRules;
using MathExecutor.Rules.QuadraticRules;

namespace MathExecutor.RuleBinders
{
    public class MultiRuleChecher : IMultiRuleChecker
    {
        private readonly IList<IRule> _rules;

        public MultiRuleChecher(
            IExpressionFlatener expressionFlatener,
            IElementsChanger elementsChanger,
            IParentChecker parentChecker
            )
        {
            _rules = new List<IRule>
            {
                new LinearEquationRule(),
                new QuadraticEquationRule(),
                //----------------------------------------
                new MonomialRemovalRule(),
                new MonomialZeroRole(),
                //----------------------------------------
                new ParenthesisRemovalRule(expressionFlatener, elementsChanger),
                new ParenthesisMonomialMultRule(expressionFlatener, elementsChanger, parentChecker),
                //----------------------------------------
                new BiquadraticRule(expressionFlatener),
                new QuadratDifferenceRule(parentChecker),
                //----------------------------------------
                new FractionSimplifyRule(expressionFlatener, elementsChanger, parentChecker),
                new FractionDivisionRule(parentChecker, elementsChanger),
                new FractionProductRule(parentChecker),
                new FractionSumRule(parentChecker),
                new ParenthesisMultiplicationRule(expressionFlatener, parentChecker)
            };
        }

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
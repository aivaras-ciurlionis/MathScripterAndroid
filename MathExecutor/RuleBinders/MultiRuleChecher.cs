using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Rules.FinalRules;
using MathExecutor.Rules.FractionRules;
using MathExecutor.Rules.MonomialRules;
using MathExecutor.Rules.ParenthesisRules;
using MathExecutor.Rules.QuadraticRules;
using MathExecutor.Rules.TrivialRules;

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
                new SquareMonomialRule(),
                new PartQuadraticEquationRule(parentChecker),
                new QuadraticIdentityRule(parentChecker),
                //----------------------------------------
                new MonomialRemovalRule(),
                new MonomialZeroRole(),
                new OneBotDivRule(),
                new OneMultiRule(),
                new ZeroTopDivRule(),
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
                new CommonDenominatorSum(parentChecker),
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
                        RuleDescription = result.RuleDescription,
                        IsDescriptive = result.HelperExpressions != null && result.HelperExpressions.Any(),
                        HelperSteps = result.HelperExpressions
                    });
                }
            }
            return results;
        }
    }
}
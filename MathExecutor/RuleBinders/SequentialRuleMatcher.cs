using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Rules;
using MathExecutor.Rules.FinalRules;
using MathExecutor.Rules.ParenthesisRules;

namespace MathExecutor.RuleBinders
{
    public class SequentialRuleMatcher : ISequentialRuleMatcher
    {
        private readonly IRule _signMergeRule;
        private readonly IRule _reorderRule;
        private readonly IRule _equalityReorderRule;
        private readonly IRule _finalParenthesisRule;

        private List<Step> _steps;

        private readonly IInterpreter _interpreter;
        private readonly IStepsReducer _stepsReducer;

        private void AddStep(Step step)
        {
            _steps.Add(step);
        }

        private void AddSteps(IEnumerable<Step> steps)
        {
            foreach (var step in steps)
            {
                AddStep(step);
            }
        }

        private IEnumerable<Step> ApplyAndInterpret(IExpression expression, IRule rule, IRule fixingRule = null)
        {
            var steps = new List<Step>();
            if (rule != null)
            {
                var ruleResult = rule.ApplyRule(expression);
                if (ruleResult.Applied)
                {
                    if (fixingRule != null)
                    {
                        var fixedResult = fixingRule.ApplyRule(ruleResult.Expression.Clone());
                        ruleResult = fixedResult.Applied ? fixedResult : ruleResult;
                    }
                    steps.Add(
                        new Step
                        {
                            FullExpression = ruleResult.Expression.Clone(),
                            RuleDescription = ruleResult.RuleDescription
                        });
                    expression = ruleResult.Expression.Clone();
                }
            }
            var result = _interpreter.FindSolution(expression.Clone());
            steps.AddRange(result.Steps);
            return steps;
        }

        public SequentialRuleMatcher(
            IInterpreter interpreter,
            IExpressionFlatener expressionFlatener,
            IOtherExpressionAdder expressionAdder, 
            IStepsReducer stepsReducer)
        {
            _signMergeRule = new SignMergeRule();
            _reorderRule = new ReorderRule(expressionFlatener, expressionAdder);
            _equalityReorderRule = new EqualityReorderRule(expressionFlatener, expressionAdder);
            _finalParenthesisRule = new FinalParenthesisRule();
            _interpreter = interpreter;
            _stepsReducer = stepsReducer;
            _steps = new List<Step>();
        }

        public IEnumerable<Step> GetSequentialRuleSteps(IExpression expression)
        {
            _steps = new List<Step>();
            IExpression startingExpression;
            do
            {
                startingExpression = expression.Clone();
                // only interpreter
                AddSteps(ApplyAndInterpret(expression, null));
                expression = _steps.Last().FullExpression.Clone();

                AddSteps(ApplyAndInterpret(expression, _equalityReorderRule, _signMergeRule));
                expression = _steps.Last().FullExpression.Clone();

                AddSteps(ApplyAndInterpret(expression, _reorderRule, _signMergeRule));
                expression = _steps.Last().FullExpression.Clone();

                AddSteps(ApplyAndInterpret(expression, _signMergeRule));
                expression = _steps.Last().FullExpression.Clone();

                AddSteps(ApplyAndInterpret(expression, _finalParenthesisRule));
                expression = _steps.Last().FullExpression.Clone();

            } while (!startingExpression.IsEqualTo(expression));
            return _stepsReducer.ReduceSteps(_steps);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.RuleBinders
{
    public class RecursiveRuleMatcher : IRecursiveRuleMathcer
    {
        private readonly ISequentialRuleMatcher _sequentialRuleMatcher;
        private readonly IMultiRuleChecker _multiRuleChecker;
        private readonly IFinalResultChecker _finalResultChecker;

        private IEnumerable<Step> _shortestResultSteps = new List<Step>();

        public RecursiveRuleMatcher(
            ISequentialRuleMatcher sequentialRuleMatcher,
            IMultiRuleChecker multiRuleChecker,
            IFinalResultChecker finalResultChecker)
        {
            _sequentialRuleMatcher = sequentialRuleMatcher;
            _multiRuleChecker = multiRuleChecker;
            _finalResultChecker = finalResultChecker;
        }

        private static bool StepExists(IEnumerable<Step> stepsBefore, IExpression expression)
        {
            return stepsBefore.Any(s => s.FullExpression.IsEqualTo(expression));
        }

        public IEnumerable<Step> SolveExpression(IExpression expression)
        {
            var firstStep = new List<Step> { new Step { FullExpression = expression } };
            _shortestResultSteps = firstStep;
            var result = SolveExpressionRecursive(new List<Step>(), expression);
            return result ?? _shortestResultSteps;
        }

        private IEnumerable<Step> SolveExpressionRecursive(IEnumerable<Step> stepsBefore, IExpression expression)
        {
            var sequentialSteps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression);
            var steps = sequentialSteps as IList<Step> ?? sequentialSteps.ToList();
            var lastStep = steps.Last();
            var addedSteps = stepsBefore.Union(steps);
            if (_finalResultChecker.IsFinalResult(lastStep.FullExpression))
            {
                return addedSteps;
            }
            var possibleRules = _multiRuleChecker.ApplyRules(lastStep.FullExpression.Clone());
            var ruleAddedSteps = addedSteps as IList<Step> ?? addedSteps.ToList();
            foreach (var possibleRule in possibleRules)
            {
                if (StepExists(ruleAddedSteps, possibleRule.FullExpression))
                {
                    continue;
                }
                var ruleResult = SolveExpressionRecursive(ruleAddedSteps, possibleRule.FullExpression.Clone());
                if (ruleResult != null)
                {
                    return ruleResult;
                }
            }
            if (lastStep.FullExpression.ToString().Length <
                _shortestResultSteps.Last().FullExpression.ToString().Length)
            {
                _shortestResultSteps = ruleAddedSteps;
            }
            return null;
        }

    }
}
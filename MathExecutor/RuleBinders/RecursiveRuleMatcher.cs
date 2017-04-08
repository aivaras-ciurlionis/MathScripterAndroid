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
            var result = SolveExpressionRecursive(new List<Step>(), expression, 0);
            return result ?? _shortestResultSteps;
        }

        private IEnumerable<Step> SolveExpressionRecursive(IEnumerable<Step> stepsBefore, IExpression expression, int height)
        {
            if (height > 30)
            {
                return null; 
            }
            var sequentialSteps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression).ToList();
            var lastStep = sequentialSteps.Last();
            var enumerable = stepsBefore as IList<Step> ?? stepsBefore.ToList();
            var addedSteps = enumerable.Union(sequentialSteps);
            if (_finalResultChecker.IsFinalResult(lastStep.FullExpression))
            {
                return addedSteps;
            }

            if(sequentialSteps.Any(s => StepExists(enumerable.Take(enumerable.Count() - 1), s.FullExpression)))
            {
                return null;
            }

            var possibleRules = _multiRuleChecker.ApplyRules(lastStep.FullExpression.Clone());
            var ruleAddedSteps = addedSteps as IList<Step> ?? addedSteps.ToList();
            foreach (var possibleRule in possibleRules)
            {
                if (StepExists(ruleAddedSteps, possibleRule.FullExpression))
                {
                    continue;
                }
                var newSteps = ruleAddedSteps.Union(new List<Step> {possibleRule});
                var ruleResult = SolveExpressionRecursive(newSteps, possibleRule.FullExpression.Clone(), height+1);
                if (ruleResult != null)
                {
                    return ruleResult;
                }
            }
            if (StepsAreBetter(ruleAddedSteps))
            {
                _shortestResultSteps = ruleAddedSteps;
            }
            return null;
        }

        private bool StepsAreBetter(IEnumerable<Step> steps)
        {
            var enumerable = steps as IList<Step> ?? steps.ToList();
            if (enumerable.Last().FullExpression.ToString().Length <
                _shortestResultSteps.Last().FullExpression.ToString().Length)
            {
                return true;
            }
            return enumerable.Last().FullExpression.ToString().Length ==
                   _shortestResultSteps.Last().FullExpression.ToString().Length &&
                   enumerable.Count() <_shortestResultSteps.Count();
        }

    }
}
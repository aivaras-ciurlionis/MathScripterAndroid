using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Helpers
{
    public class StepsReucer : IStepsReducer
    {
        public IEnumerable<Step> ReduceSteps(IEnumerable<Step> steps)
        {
            var reducedSteps = new List<Step>();
            var i = 0;
            var numSteps = steps.ToList();
            while (i < numSteps.Count-1)
            {
                if (numSteps[i].IsDescriptive)
                {
                    var innerSteps = numSteps[i].HelperSteps
                        .Select(s => new Step {FullExpression = s, IsDescriptive = true});
                    reducedSteps.AddRange(innerSteps);
                }
                if (numSteps[i].ToString() != numSteps[i + 1].ToString())
                {
                    reducedSteps.Add(numSteps[i]);
                }
                i++;
            }
            reducedSteps.Add(numSteps.Last());
            return reducedSteps;
        }
    }
}
using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IStepsReducer
    {
        IEnumerable<Step> ReduceSteps(IEnumerable<Step> steps);
    }
}
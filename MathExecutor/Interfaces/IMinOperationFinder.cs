using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IMinOperationFinder
    {
        int FindMinOperationIndex(IList<Token> tokens);
    }
}
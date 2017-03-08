using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IBlockBuilder
    {
        string GetEquationInBlock(IList<NamedSegment> segments);
    }
}
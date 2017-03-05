using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IEquationsBuilder
    {
        IEnumerable<string> GetEquations(IEnumerable<NamedSegment> segments);
    }
}
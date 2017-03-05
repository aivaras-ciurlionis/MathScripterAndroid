using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IEquationBuilder
    {
        string GetEquation(IEnumerable<NamedSegment> segments);
    }
}
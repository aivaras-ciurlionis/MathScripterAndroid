using System.Collections.Generic;
using MathRecongizer.Models;

namespace MathRecongizer.Interfaces
{
    public interface IEquationBuilder
    {
        string GetEquation(IEnumerable<NamedSegment> segments);
    }
}
using System.Collections.Generic;
using MathRecongizer.Models;

namespace MathRecongizer.Interfaces
{
    public interface IEquationsBuilder
    {
        IEnumerable<string> GetEquations(IEnumerable<NamedSegment> segments);
    }
}
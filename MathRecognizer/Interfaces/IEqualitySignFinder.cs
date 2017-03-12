using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IEqualitySignFinder
    {
        IList<NamedSegment> FindEqualitySigns(IList<NamedSegment> segments);
    }
}
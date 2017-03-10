using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ISegmentBuilder
    {
        NamedSegment GetBoundingSegment(IEnumerable<NamedSegment> namedSegments, string name);
    }
}
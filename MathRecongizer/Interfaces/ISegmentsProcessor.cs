using System.Collections.Generic;
using MathRecongizer.Models;

namespace MathRecongizer.Interfaces
{
    public interface ISegmentsProcessor
    {
        IEnumerable<NamedSegment> RecognizeSegments(IEnumerable<Segment> segments);
    }
}
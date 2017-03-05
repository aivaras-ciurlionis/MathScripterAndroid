using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ISegmentsProcessor
    {
        IEnumerable<NamedSegment> RecognizeSegments(IEnumerable<Segment> segments);
    }
}
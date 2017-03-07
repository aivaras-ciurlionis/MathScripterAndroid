using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ISegmentsSplitter
    {
        IEnumerable<IEnumerable<NamedSegment>> SplitSegments(IEnumerable<NamedSegment>  segments, int imageHeight);
    }
}
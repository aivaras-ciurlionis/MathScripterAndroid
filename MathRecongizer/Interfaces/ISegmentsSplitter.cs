using System.Collections.Generic;
using MathRecongizer.Models;

namespace MathRecongizer.Interfaces
{
    public interface ISegmentsSplitter
    {
        IEnumerable<IEnumerable<NamedSegment>> SplitSegments(IEnumerable<NamedSegment>  segments);
    }
}
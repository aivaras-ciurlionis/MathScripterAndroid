using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IRectangleIntersectionFinder
    {
        bool SegmentsIntersect(NamedSegment s1, NamedSegment s2);
        bool HasSegmentsToTop(NamedSegment target, IList<NamedSegment> otherSegments);
        bool HasSegmentsToBottom(NamedSegment target, IList<NamedSegment> otherSegments);
        bool HasSegmentsTopAndBottom(NamedSegment target, IList<NamedSegment> otherSegments);
        IList<NamedSegment> SegmentsToTop(NamedSegment target, IList<NamedSegment> otherSegments, int pixelLimit=1000);
        IList<NamedSegment> SegmentsToBottom(NamedSegment target, IList<NamedSegment> otherSegments, int pixelLimit=1000);
        IList<NamedSegment> SegmentsInside(NamedSegment target, IList<NamedSegment> otherSegments);
    }
}
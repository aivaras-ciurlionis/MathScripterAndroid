using System.Collections.Generic;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class MinusRowSeparator : IMinusRowSeparator
    {
        private readonly IRectangleIntersectionFinder _intersectionFinder;

        public MinusRowSeparator(IRectangleIntersectionFinder intersectionFinder)
        {
            _intersectionFinder = intersectionFinder;
        }

        public IList<NamedSegment> FindEquationRowSegments(IList<NamedSegment> segments)
        {
            foreach (var segment in segments)
            {
                if (segment.SegmentName == "-" &&
                    _intersectionFinder.HasSegmentsTopAndBottom(segment, segments))
                {
                    segment.SegmentName = "_";
                }
            }
            return segments;
        }
    }
}
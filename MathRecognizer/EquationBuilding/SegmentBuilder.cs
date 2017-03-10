using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class SegmentBuilder : ISegmentBuilder
    {
        public NamedSegment GetBoundingSegment(IEnumerable<NamedSegment> namedSegments, string name)
        {
            var enumerable = namedSegments as IList<NamedSegment> ?? namedSegments.ToList();
            return new NamedSegment
            {
                SegmentName = name,
                MaxX = enumerable.Max(s => s.MaxX),
                MaxY = enumerable.Max(s => s.MaxY),
                MinX = enumerable.Min(s => s.MinX),
                MinY = enumerable.Min(s => s.MinY)
            };
        }
    }
}
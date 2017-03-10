using System;
using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class RectangleIntersectionFinder : IRectangleIntersectionFinder
    {
        private const int PixelRange = 30;

        public bool SegmentsIntersect(NamedSegment s1, NamedSegment s2)
        {
            return s1.MinX < s2.MaxX && s1.MaxX > s2.MinX &&
                   s1.MinY < s2.MaxY && s1.MaxY > s2.MinY;
        }

        public bool HasSegmentsToTop(NamedSegment target, IList<NamedSegment> otherSegments)
        {
            return SegmentsToTop(target, otherSegments, PixelRange)
                .Any(s => s.SegmentName != "-" && s.SegmentName != "q" && s.SegmentName != "_");
        }

        public bool HasSegmentsToBottom(NamedSegment target, IList<NamedSegment> otherSegments)
        {
            return SegmentsToBottom(target, otherSegments, PixelRange)
                .Any(s => s.SegmentName != "-" && s.SegmentName != "q");
        }

        public bool HasSegmentsTopAndBottom(NamedSegment target, IList<NamedSegment> otherSegments)
        {
            return HasSegmentsToTop(target, otherSegments) && HasSegmentsToBottom(target, otherSegments);
        }

        public IList<NamedSegment> SegmentsToTop(NamedSegment target, IList<NamedSegment> otherSegments, int pixelLimit = 1000)
        {
            var tempSegment = new NamedSegment
            {
                MaxX = target.MaxX,
                MaxY = target.MinY,
                MinX = target.MinX,
                MinY = target.MinY - pixelLimit
            };
            return otherSegments.Where(s => SegmentsIntersect(tempSegment, s)).ToList();
        }

        public IList<NamedSegment> SegmentsToBottom(NamedSegment target, IList<NamedSegment> otherSegments, int pixelLimit = 1000)
        {
            var tempSegment = new NamedSegment
            {
                MaxX = target.MaxX,
                MaxY = target.MaxY + pixelLimit,
                MinX = target.MinX,
                MinY = target.MaxY
            };
            return otherSegments.Where(s => SegmentsIntersect(tempSegment, s)).ToList();
        }

        public IList<NamedSegment> SegmentsInside(NamedSegment target, IList<NamedSegment> otherSegments)
        {
            return otherSegments.Where(s => s != target && SegmentsIntersect(target, s)).ToList();
        }
    }
}
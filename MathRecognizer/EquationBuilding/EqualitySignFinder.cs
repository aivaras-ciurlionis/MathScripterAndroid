using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class EqualitySignFinder : IEqualitySignFinder
    {

        private const int PixelMargin = 20;

        private readonly IRectangleIntersectionFinder _intersectionFinder;
        private readonly ISegmentBuilder _segmentBuilder;

        public EqualitySignFinder(
            IRectangleIntersectionFinder intersectionFinder,
            ISegmentBuilder segmentBuilder
            )
        {
            _intersectionFinder = intersectionFinder;
            _segmentBuilder = segmentBuilder;
        }

        public IList<NamedSegment> FindEqualitySigns(IList<NamedSegment> segments)
        {
            var lines = segments.Where(s => s.SegmentName == "-").ToList();

            while (lines.Count > 0)
            {
                var line = lines.First();
                lines.Remove(line);
                var topLines = _intersectionFinder.SegmentsToTop(line, lines, PixelMargin);
                var botLines = _intersectionFinder.SegmentsToBottom(line, lines, PixelMargin);
                if (topLines.Count != 1 && botLines.Count != 1) continue;
                segments.Remove(line);
                var otherLine = topLines.Count == 1 ? topLines.First() : botLines.First();
                segments.Remove(otherLine);
                lines.Remove(otherLine);
                segments.Add(_segmentBuilder.GetBoundingSegment(new List<NamedSegment> { line, otherLine }, "="));
            }
            return segments;
        }
    }
}
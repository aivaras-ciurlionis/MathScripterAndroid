using System;
using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class SegmentsSplitter : ISegmentsSplitter
    {
        private const double DistanceLimit = 0.1;
        private const double MinLimit = 30;

        private IList<NamedSegment> _unusedSegments;
        private readonly IRectangleDistanceFinder _distanceFinder;

        public SegmentsSplitter(IRectangleDistanceFinder distanceFinder)
        {
            _distanceFinder = distanceFinder;
        }

        public IEnumerable<IEnumerable<NamedSegment>> SplitSegments(IEnumerable<NamedSegment> segments, int imageEdge)
        {
            _unusedSegments = segments.ToList();
            var minDist = imageEdge * DistanceLimit;

            if (minDist < MinLimit)
            {
                minDist = MinLimit;
            }

            var segmentGroups = new List<List<NamedSegment>>();
            while (_unusedSegments.Count > 1)
            {
                var firstSegment = _unusedSegments.First();
                var segmentGroup = new List<NamedSegment> { firstSegment };
                _unusedSegments.Remove(firstSegment);
                var i = 0;
                while (i < segmentGroup.Count)
                {
                    var currentSegment = segmentGroup[i];
                    var j = 0;
                    while (j < _unusedSegments.Count)
                    {
                        var otherSegment = _unusedSegments[j];
                        var d = _distanceFinder.DistanceBetweenSegments(currentSegment, otherSegment);
                        if (d < minDist)
                        {
                            Console.WriteLine($"{currentSegment} : {otherSegment}  -> {d}");
                            segmentGroup.Add(otherSegment);
                            _unusedSegments.Remove(otherSegment);
                        }
                        j++;
                    }
                    i++;
                }
                segmentGroups.Add(segmentGroup);
            }
            return segmentGroups;
        }
    }
}
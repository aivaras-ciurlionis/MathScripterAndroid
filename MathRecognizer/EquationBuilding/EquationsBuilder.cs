using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class EquationsBuilder : IEquationsBuilder
    {
        private readonly ISegmentsSplitter _segmentsSplitter;
        private readonly IEquationBuilder _equationBuilder;

        public EquationsBuilder(
            ISegmentsSplitter segmentsSplitter,
            IEquationBuilder equationBuilder
            )
        {
            _segmentsSplitter = segmentsSplitter;
            _equationBuilder = equationBuilder;
        }

        public IEnumerable<string> GetEquations(IEnumerable<NamedSegment> segments, int imageEdge)
        {
            var groupedSegments = _segmentsSplitter.SplitSegments(segments, imageEdge);
            return groupedSegments
                .Select(segmentGroup => _equationBuilder.GetEquation(segmentGroup))
                .ToList();
        }

    }
}
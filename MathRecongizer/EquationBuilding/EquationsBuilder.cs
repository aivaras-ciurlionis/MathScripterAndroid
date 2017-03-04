using System.Collections.Generic;
using System.Linq;
using MathRecongizer.Interfaces;
using MathRecongizer.Models;

namespace MathRecongizer.EquationBuilding
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

        public IEnumerable<string> GetEquations(IEnumerable<NamedSegment> segments)
        {
            var groupedSegments = _segmentsSplitter.SplitSegments(segments);
            return groupedSegments
                .Select(segmentGroup => _equationBuilder.GetEquation(segmentGroup))
                .ToList();
        }

    }
}
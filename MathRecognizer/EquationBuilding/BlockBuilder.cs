using System;
using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class BlockBuilder : IBlockBuilder
    {
        private readonly IRectangleIntersectionFinder _intersectionFinder;
        private readonly ISegmentBuilder _segmentBuilder;
        private readonly IEquationStripper _equationStripper;
        private readonly ICharacterFixer _characterFixer;

        private IList<NamedSegment> _segments;

        public BlockBuilder(
            IRectangleIntersectionFinder intersectionFinder,
            ISegmentBuilder segmentBuilder,
            IEquationStripper equationStripper, ICharacterFixer characterFixer)
        {
            _intersectionFinder = intersectionFinder;
            _segmentBuilder = segmentBuilder;
            _equationStripper = equationStripper;
            _characterFixer = characterFixer;
        }

        private IList<NamedSegment> GetSuperscriptSegments(NamedSegment lastSegment)
        {
            var subscriptSegments = new List<NamedSegment>();
            if (lastSegment == null) return null;
            while (_segments.Count > 0 &&
                   lastSegment.HasSuperscript(_segments.First()))
            {
                subscriptSegments.Add(_segments.First());
                _segments.Remove(_segments.First());
            }
            return subscriptSegments.Count > 0 ? subscriptSegments : null;
        }

        private void RemoveSegments(IEnumerable<NamedSegment> segments)
        {
            foreach (var segment in segments)
            {
                _segments.Remove(segment);
            }
        }

        private IList<NamedSegment> GetDivisionTopSegments(NamedSegment divisionSegment)
        {
            var topSegments = _intersectionFinder.SegmentsToTop(divisionSegment, _segments);
            return topSegments;
        }

        private IList<NamedSegment> GetDivisionBottomSegments(NamedSegment divisionSegment)
        {
            var botSegments = _intersectionFinder.SegmentsToBottom(divisionSegment, _segments);
            return botSegments;
        }

        private void FormRootSegments()
        {
            var innerBuilder = new BlockBuilder(new RectangleIntersectionFinder(),
            new SegmentBuilder(), new EquationStripper(), new CharacterFixer());
            for (var i = 0; i < _segments.Count; i++)
            {
                if (_segments[i].SegmentName != "q") continue;
                var innerSegments = _intersectionFinder.SegmentsInside(_segments[i], _segments);
                _segments.Remove(_segments[i]);
                RemoveSegments(innerSegments);
                var inner = innerBuilder.GetEquationInBlock(innerSegments);
                if (!string.IsNullOrEmpty(inner))
                {
                    _segments.Add(_segmentBuilder.GetBoundingSegment(innerSegments, $"sqrt({inner})"));
                }
            }
        }

        private void FormDivisionSegments()
        {
            var innerBuilder = new BlockBuilder(new RectangleIntersectionFinder(),
            new SegmentBuilder(), new EquationStripper(), new CharacterFixer());
            for (var i = 0; i < _segments.Count; i++)
            {
                if (_segments[i].SegmentName != "_") continue;
                var topSegments = GetDivisionTopSegments(_segments[i]);
                var botSegments = GetDivisionBottomSegments(_segments[i]);
                _segments.Remove(_segments[i]);
                RemoveSegments(botSegments);
                RemoveSegments(topSegments);
                var top = innerBuilder.GetEquationInBlock(topSegments);
                var bot = innerBuilder.GetEquationInBlock(botSegments);
                if (!string.IsNullOrEmpty(top) && !string.IsNullOrEmpty(bot))
                {
                    _segments.Add(_segmentBuilder.GetBoundingSegment(topSegments.Union(botSegments), $"({top})/({bot})"));
                }
            }
        }

        public string GetEquationInBlock(IList<NamedSegment> segments)
        {
            var equation = "";
            _segments = segments;
            FormRootSegments();
            FormDivisionSegments();
            _segments = _segments.OrderBy(s => s.MinX).ToList();
            NamedSegment lastSegment = null;
            var innerBuilder = new BlockBuilder(new RectangleIntersectionFinder(),
            new SegmentBuilder(), new EquationStripper(), new CharacterFixer());
            while (_segments.Count > 0)
            {
                var superscriptSegments = GetSuperscriptSegments(lastSegment);
                if (superscriptSegments != null)
                {
                    var superscriptEquation = innerBuilder.GetEquationInBlock(superscriptSegments);
                    equation += string.IsNullOrEmpty(superscriptEquation) ? "" : $"^({superscriptEquation})";
                    //lastSegment = null;
                }
                else
                {
                    var currentSegment = _segments.First();
                    _segments.Remove(currentSegment);
                    equation = _characterFixer.AddAdjusted(equation, lastSegment, currentSegment, _segments.FirstOrDefault());
                    lastSegment = currentSegment;
                }
            }
            return _equationStripper.StripEquation(equation);
        }
    }
}
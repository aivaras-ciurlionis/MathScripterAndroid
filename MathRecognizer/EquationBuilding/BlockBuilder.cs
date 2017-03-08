using System;
using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class BlockBuilder : IBlockBuilder
    {
        private IList<NamedSegment> _segments;

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

        public string GetEquationInBlock(IList<NamedSegment> segments)
        {
            var equation = "";
            _segments = segments.OrderBy(s => s.MinX).ToList();
            var innerBuilder = new BlockBuilder();
            NamedSegment lastSegment = null;
            while (_segments.Count > 0)
            {
                var superscriptSegments = GetSuperscriptSegments(lastSegment);
                if (superscriptSegments != null)
                {
                    equation += $"^({innerBuilder.GetEquationInBlock(superscriptSegments)})";
                    lastSegment = null;
                }
                else
                {
                    var currentSegment = _segments.First();
                    _segments.Remove(currentSegment);
                    equation += currentSegment.SegmentName;
                    lastSegment = currentSegment;
                }
            }
            return equation;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class EquationBuilder : IEquationBuilder
    {
        private readonly IBlockBuilder _blockBuilder;
        private readonly IMinusRowSeparator _rowSeparator;
        private readonly IEqualitySignFinder _equalitySignFinder;

        public EquationBuilder(IBlockBuilder blockBuilder,
            IMinusRowSeparator rowSeparator,
            IEqualitySignFinder equalitySignFinder)
        {
            _blockBuilder = blockBuilder;
            _rowSeparator = rowSeparator;
            _equalitySignFinder = equalitySignFinder;
        }

        public string GetEquation(IEnumerable<NamedSegment> segments)
        {
            segments = _rowSeparator.FindEquationRowSegments(segments.ToList());
            segments = _equalitySignFinder.FindEqualitySigns(segments.ToList());
            return _blockBuilder.GetEquationInBlock(segments.ToList());
        }
    }
}
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

        public EquationBuilder(IBlockBuilder blockBuilder,
            IMinusRowSeparator rowSeparator
            )
        {
            _blockBuilder = blockBuilder;
            _rowSeparator = rowSeparator;
        }

        public string GetEquation(IEnumerable<NamedSegment> segments)
        {
            segments = _rowSeparator.FindEquationRowSegments(segments.ToList());
            return _blockBuilder.GetEquationInBlock(segments.ToList());
        }
    }
}
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

        public EquationBuilder(IBlockBuilder blockBuilder)
        {
            _blockBuilder = blockBuilder;
        }

        public string GetEquation(IEnumerable<NamedSegment> segments)
        {
            return _blockBuilder.GetEquationInBlock(segments.ToList());
        }
    }
}
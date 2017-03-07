using System;
using System.Collections.Generic;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class EquationBuilder : IEquationBuilder 
    {
        public string GetEquation(IEnumerable<NamedSegment> segments)
        {
            throw new NotImplementedException();
        }
    }
}
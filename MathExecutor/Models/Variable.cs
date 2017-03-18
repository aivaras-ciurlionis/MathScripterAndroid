using System;
using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class Variable : IVariable
    {
        public double Exponent { get; set; }
        public string Name { get; set; }

        public bool IsEqualTo(IVariable other)
        {
            return Math.Abs(Exponent - other.Exponent) < 0.001 &&
                string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public double Evaluate(double value)
        {
            return Math.Pow(value, Exponent);
        }

        public override string ToString()
        {
            return Math.Abs(Exponent - 1) < 0.001 ? $"{Name}" : $"{Name}^{Exponent}";
        }


    }
}
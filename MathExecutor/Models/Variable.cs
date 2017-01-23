using System;

namespace MathExecutor.Models
{
    public class Variable
    {
        public decimal Exponent { get; set; }
        public string Name { get; set; }

        public bool IsEqualTo(Variable other)
        {
            return Exponent == other.Exponent && 
                string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
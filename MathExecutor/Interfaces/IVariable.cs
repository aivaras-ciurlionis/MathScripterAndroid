using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IVariable
    {
        double Exponent { get; set; }
        string Name { get; set; }

        double Evaluate(double value);
        bool IsEqualTo(IVariable other);
    }
}
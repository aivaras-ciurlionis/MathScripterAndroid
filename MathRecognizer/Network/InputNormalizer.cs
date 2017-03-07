using System.Linq;
using MathRecognizer.Interfaces;

namespace MathRecognizer.Network
{
    public class InputNormalizer : IInputNormalizer
    {
        public double[] NormalizeInput(byte[] input)
        {
            return input.Take(4096).Select(i => (double) i / 255).ToArray();
        }
    }
}
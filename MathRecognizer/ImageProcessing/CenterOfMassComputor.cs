using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.ImageProcessing
{
    public class CenterOfMassComputor : ICenterOfMassComputor
    {
        public Point GetCenterOfMass(byte[] pixels, int width, int height)
        {
            var totalIntensity = pixels.Sum(p => p);
            var sumX = 0;
            var sumY = 0;
            for (var i = 0; i < pixels.Length; i++)
            {
                sumX += i % width * pixels[i];
                sumY += (i / width) * pixels[i];
            }
            return new Point(sumX / totalIntensity,
                sumY / totalIntensity);
        }
    }
}
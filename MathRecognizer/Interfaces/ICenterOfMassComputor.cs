using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ICenterOfMassComputor
    {
        Point GetCenterOfMass(byte[] pixels, int width, int height);
    }
}
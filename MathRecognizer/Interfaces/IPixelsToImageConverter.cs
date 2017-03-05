using ImageSharp;

namespace MathRecognizer.Interfaces
{
    public interface IPixelsToImageConverter
    {
        Image GetImage(byte[] pixels, int width, int height);
    }
}
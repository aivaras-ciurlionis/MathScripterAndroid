using Android.Graphics;
using ImageSharp;

namespace MathRecognizer.Interfaces
{
    public interface IImageDecoder
    {
        byte[] GetPixels(Bitmap bitmap);
        byte[] GetPixels(Image image);
    }
}
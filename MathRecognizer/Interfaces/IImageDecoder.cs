using Android.Graphics;

namespace MathRecognizer.Interfaces
{
    public interface IImageDecoder
    {
        byte[] GetPixels(Bitmap bitmap);
    }
}
using Android.Graphics;

namespace MathRecongizer.Interfaces
{
    public interface IImageDecoder
    {
        byte[] GetPixels(Bitmap bitmap);
    }
}
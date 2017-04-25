using Android.Graphics;

namespace MathScripter.Providers
{
    public class BitmapProcessor
    {
        public static Bitmap ChangeBitmapContrastBrightness(Bitmap bmp, float contrast, float brightness)
        {
            if (bmp.IsRecycled)
            {
                return bmp;
            }
            var cm = new ColorMatrix(new[]
            {
                contrast, 0, 0, 0, brightness,
                0, contrast, 0, 0, brightness,
                0, 0, contrast, 0, brightness,
                0, 0, 0, 1, 0
            });
            var ret = Bitmap.CreateBitmap(bmp.Width, bmp.Height, bmp.GetConfig());
            var canvas = new Canvas(ret);
            var paint = new Paint();
            paint.SetColorFilter(new ColorMatrixColorFilter(cm));
            canvas.DrawBitmap(bmp, 0, 0, paint);
            return ret;
        }
    }
}
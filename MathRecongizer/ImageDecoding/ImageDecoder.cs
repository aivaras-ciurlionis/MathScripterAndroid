using System.IO;
using System.Linq;
using Android.Graphics;
using ImageSharp;
using MathRecongizer.Interfaces;

namespace MathRecongizer.ImageDecoding
{
    public class ImageDecoder : IImageDecoder
    {
        public byte[] GetPixels(Bitmap bitmap)
        {
            Image image;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                var bitmapData = stream.ToArray();
                image = new Image(bitmapData);
            }
            var processedPixels = image
                .Grayscale()
                .Brightness(20)
                .Contrast(70)
                .Pixels;

            return processedPixels
                .Select(p => (byte)(255 - p.R))
                .ToArray();
        }
    }
}
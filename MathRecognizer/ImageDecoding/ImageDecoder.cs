using System.IO;
using System.Linq;
using Android.Graphics;
using ImageSharp;
using MathRecognizer.Interfaces;

namespace MathRecognizer.ImageDecoding
{
    public class ImageDecoder : IImageDecoder
    {
        public byte[] GetPixels(Bitmap bitmap)
        {
            Image image;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 90, stream);
                var bitmapData = stream.ToArray();
                image = new Image(bitmapData);
            }

            return GetPixels(image);
        }

        public byte[] GetPixels(Image image)
        {
            return image
                .Grayscale()
                .Invert()
                .Pixels
                .Select(p => p.R)
                .ToArray();
        }
    }
}
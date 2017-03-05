using ImageSharp;
using MathRecognizer.Interfaces;

namespace MathRecognizer.ImageProcessing
{
    public class PixelsToImageConverter : IPixelsToImageConverter
    {
        public Image GetImage(byte[] pixels, int width, int height)
        {
            var image = new Image(width, height);
            for (var i = 0; i < image.Pixels.Length; i++)
            {
                if (i < pixels.Length)
                {
                    image.Pixels[i] = new Color(pixels[i], pixels[i], pixels[i]);
                }
            }
            return image;
        }
    }
}
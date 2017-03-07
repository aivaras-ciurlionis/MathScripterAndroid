using System;
using ImageSharp;
using MathRecognizer.Interfaces;

namespace MathRecognizer.ImageProcessing
{
    public class RatioResizer : IRatioResizer
    {
        public Image ResizeImage(Image source, int size)
        {

            if (source.Width > source.Height)
            {
                var ratio = (double)source.Width / size;
                var r = source.Height / ratio;
                var temp = (Image)source.Resize(size, Convert.ToInt32(r));
                var d = (size - temp.Height) / 2;
                var placeholder = new Image(size, size);
                Console.WriteLine(placeholder.Pixels.Length);
                var pixelCount = temp.Width * temp.Height;
                for (var i = 0; i < pixelCount; i++)
                {
                    var x = i % temp.Width;
                    var y = i / temp.Width + d;
                    placeholder.Pixels[y * placeholder.Width + x] = temp.Pixels[i];
                }
                return placeholder;
            }
            else
            {
                var ratio = (double)source.Height / size;
                var r = source.Width / ratio;
                var temp = (Image)source.Resize(Convert.ToInt32(r), size);
                var d = (size - temp.Width) / 2;
                var placeholder = new Image(size, size);
                Console.WriteLine(placeholder.Pixels.Length);
                var pixelCount = temp.Width * temp.Height;
                for (var i = 0; i < pixelCount; i++)
                {
                    var x = i % temp.Width +d;
                    var y = i / temp.Width;
                    placeholder.Pixels[y * placeholder.Width + x] = temp.Pixels[i];
                }
                return placeholder;
            }
        }
    }
}
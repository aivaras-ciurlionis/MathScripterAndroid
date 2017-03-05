using System;
using MathRecognizer.Interfaces;

namespace MathRecognizer.ImageProcessing
{
    public class ImageMover : IImageMover
    {
        public byte[] MovePixels(byte[] pixels, int width, int height, int offsetX, int offsetY)
        {
            var movedPixels = new byte[width * height];
            for (var i = 0; i < pixels.Length; i++)
            {
                var newX = (i % width + offsetX) % width;
                var newY = (i / width + offsetY) % height;
                if (newX < 0)
                {
                    newX = width + newX;
                }
                if (newY < 0)
                {
                    newY = height + newY;
                }
                movedPixels[newY * width + newX] = pixels[i];
            }
            return movedPixels;
        }
    }
}
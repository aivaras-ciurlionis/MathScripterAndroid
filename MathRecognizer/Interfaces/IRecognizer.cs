using System.Collections.Generic;
using Android.Graphics;
using ImageSharp;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IRecognizer
    {
        IEnumerable<string> GetEquationsInImage(Image image);
        //IEnumerable<NamedSegment> GetEquationsInImage(Bitmap image);
        //IEnumerable<Image> GetSegmentsInImage(Bitmap image);
    }
}
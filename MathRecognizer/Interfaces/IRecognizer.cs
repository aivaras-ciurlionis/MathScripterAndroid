using System.Collections.Generic;
using Android.Graphics;
using ImageSharp;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IRecognizer
    {
        IEnumerable<NamedSegment> GetEquationsInImage(Bitmap image);
        IEnumerable<Image> GetSegmentsInImage(Bitmap image);
    }
}
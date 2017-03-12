using System.Collections.Generic;
using Android.Graphics;
using ImageSharp;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IRecognizer
    {
        string GetEquationsInImage(Image image);
       // IEnumerable<string> GetEquationsInImage(Bitmap image);
        //IEnumerable<Image> GetSegmentsInImage(Bitmap image);
    }
}
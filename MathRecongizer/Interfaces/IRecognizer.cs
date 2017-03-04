using System.Collections.Generic;
using Android.Graphics;

namespace MathRecongizer.Interfaces
{
    public interface IRecognizer
    {
        IEnumerable<string> GetEquationsInImage(Bitmap image);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ImageSharp;
using Java.Security;

namespace MathRecognizer.Interfaces
{
    public interface IRatioResizer
    {
        Image ResizeImage(Image source, int size);
    }
}
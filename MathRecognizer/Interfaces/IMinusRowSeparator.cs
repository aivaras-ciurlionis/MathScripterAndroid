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
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IMinusRowSeparator
    {
        IList<NamedSegment> FindEquationRowSegments(IList<NamedSegment> segments);
    }
}
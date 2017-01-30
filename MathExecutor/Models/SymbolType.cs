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

namespace MathExecutor.Models
{
    public enum SymbolType
    {
        Numeric = 0,
        Parenthesis = 1,
        Symbol = 2,
        Other = 3
    }
}
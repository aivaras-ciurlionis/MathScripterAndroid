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
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IInterpreter
    {
        Solution FindSolution(string expression);
        IExpression GetExpression(string expression);
    }
}
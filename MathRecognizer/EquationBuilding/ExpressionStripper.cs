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
using MathRecognizer.Interfaces;

namespace MathRecognizer.EquationBuilding
{
    public class ExpressionStripper : IExpressionStripper
    {
        private readonly IList<string> _startPatterns = new List<string>
        {
            "f(x)=",
            "g(x)=",
            "8(x)=",
            "1(x)="
        };

        public string StripExpression(string expression)
        {
            return _startPatterns.Any(expression.StartsWith)
                ? expression.Substring(5, expression.Length - 5)
                : expression;
        }
    }
}
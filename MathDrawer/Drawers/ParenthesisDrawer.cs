using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class ParenthesisDrawer : IDrawer
    {
        public void DrawExpression(IExpression expression, Paint p, Canvas c, EquationBounds bounds)
        {
            throw new NotImplementedException();
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            throw new NotImplementedException();
        }
    }
}
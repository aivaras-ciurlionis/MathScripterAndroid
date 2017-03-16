using System;
using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class UnaryDrawer : IDrawer
    {
        public IList<DrawableExpression> DrawExpression(IExpression expression, Paint p, EquationBounds bounds)
        {
            throw new NotImplementedException();
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            throw new NotImplementedException();
        }
    }
}
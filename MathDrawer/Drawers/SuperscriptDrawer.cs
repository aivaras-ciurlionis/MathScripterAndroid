using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class SuperscriptDrawer : IDrawer
    {
        public IList<DrawableExpression> DrawExpression(IExpression expression, Paint p, EquationBounds bounds)
        {
            throw new System.NotImplementedException();
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            throw new System.NotImplementedException();
        }
    }
}
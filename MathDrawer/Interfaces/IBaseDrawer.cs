using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IBaseDrawer
    {
        IList<DrawableExpression> DrawExpression(IExpression expression, Paint p, EquationBounds bounds);
        EquationBounds GetBounds(IExpression expression, Paint p);
    }
}
using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Models;

namespace MathDrawer.Interfaces
{
    public interface IElementsDrawer
    {
        void DrawExpressions(IList<DrawableExpression> expressions, Paint p, Canvas c);
        void DrawExpressions(IList<DrawableExpression> expressions, Paint p, Canvas c, float offsetY);
    }
}
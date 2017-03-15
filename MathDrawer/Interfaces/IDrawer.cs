using Android.Graphics;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IDrawer
    {
        void DrawExpression(IExpression expression, Paint p, Canvas c, EquationBounds bounds);
        EquationBounds GetBounds(IExpression expression, Paint p);
    }
}
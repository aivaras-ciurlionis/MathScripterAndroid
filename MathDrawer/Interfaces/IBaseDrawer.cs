using Android.Graphics;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IBaseDrawer
    {
        void DrawExpression(IExpression expression, Paint p, Canvas c);
        EquationBounds GetBounds(IExpression expression, Paint p);
    }
}
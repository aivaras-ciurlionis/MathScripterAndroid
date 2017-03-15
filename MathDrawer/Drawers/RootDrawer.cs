using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class RootDrawer : IDrawer
    {
        public void DrawExpression(IExpression expression, Paint p, Canvas c, EquationBounds bounds)
        {
            throw new System.NotImplementedException();
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            throw new System.NotImplementedException();
        }
    }
}
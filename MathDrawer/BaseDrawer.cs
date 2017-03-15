using System.Linq;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer
{
    public class BaseDrawer : IBaseDrawer
    {
        private readonly IDrawerFactory _drawerFactory;

        public BaseDrawer(IDrawerFactory drawerFactory)
        {
            _drawerFactory = drawerFactory;
        }

        public void DrawExpression(IExpression expression, Paint p, Canvas c)
        {
            var innerExpression = expression.Operands.First();
            var drawer = _drawerFactory.GetDrawer(innerExpression);
            var width = GetBounds(expression, p).Width;
            drawer.DrawExpression(innerExpression, p, c,
                new EquationBounds { X = c.Width / 2 - width / 2, Y = c.Height / 2, Width = -1, Height = -1 });
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            var innerExpression = expression.Operands.First();
            var drawer = _drawerFactory.GetDrawer(innerExpression);
            return drawer.GetBounds(innerExpression, p);
        }
    }
}
using System.Collections.Generic;
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

        public IList<DrawableExpression> DrawExpression(IExpression expression, Paint p, EquationBounds bounds)
        {
            var innerExpression = expression.Operands.First();
            var drawer = _drawerFactory.GetDrawer(innerExpression);
            var width = GetBounds(expression, p).Width;
            return drawer.DrawExpression(innerExpression, p,
                new EquationBounds { X = bounds.X - width / 2, Y = bounds.Y, Width = -1, Height = -1 });
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            var innerExpression = expression.Operands.First();
            var drawer = _drawerFactory.GetDrawer(innerExpression);
            return drawer.GetBounds(innerExpression, p);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            var innerExpression = expression.Operands.First();
            var drawer = _drawerFactory.GetDrawer(innerExpression);
            var equationBounds = GetBounds(expression, p);
            var ratioW = (float)bounds.Width / equationBounds.Width;
            var ratioH = (float)bounds.Height / equationBounds.Height;
            var ratio = Math.Min(ratioW, ratioH);
            p.Size *= ratio;
            var newBounds = GetBounds(expression, p);
            return drawer.DrawExpression(innerExpression, p,
                new EquationBounds
                {
                    X = bounds.X - newBounds.Width / 2,
                    Y = bounds.Y,
                    Width = newBounds.Width,
                    Height = newBounds.Height
                });
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var innerExpression = expression.Operands.First();
            var drawer = _drawerFactory.GetDrawer(innerExpression);
            return drawer.GetBounds(innerExpression, p);
        }
    }
}
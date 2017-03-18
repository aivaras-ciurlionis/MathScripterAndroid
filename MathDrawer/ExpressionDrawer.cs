using System;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer
{
    public class ExpressionDrawer : IExpressionDrawer
    {
        private readonly IBaseDrawer _baseDrawer;
        private readonly IElementsDrawer _elementsDrawer;

        public ExpressionDrawer(IBaseDrawer baseDrawer, 
            IElementsDrawer elementsDrawer)
        {
            _baseDrawer = baseDrawer;
            _elementsDrawer = elementsDrawer;
        }

        public void Draw(IExpression expression, Paint p, Canvas c)
        {
            var elements = _baseDrawer.DrawExpression(
                expression,
                new TextParameters
                {
                    Size = p.TextSize,
                    Typeface = p.Typeface
                },
                new EquationBounds
                {
                    X = c.Width / 2,
                    Y = c.Height / 2,
                    Height = (int)(c.Height * 0.8),
                    Width = (int)(c.Width * 0.8)
                }
            );
            _elementsDrawer.DrawExpressions(elements, p, c);
        }
    }
}
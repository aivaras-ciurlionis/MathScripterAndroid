using System;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions;
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

        public void Draw(IExpression expression, Paint p, Canvas c, int width, int height, int yOffset=0)
        {
            if (!(expression is RootExpression))
            {
                expression = new RootExpression(expression, null);
            }
            float s;
            var elements = _baseDrawer.DrawExpression(
                expression,
                new TextParameters
                {
                    Size = 60,
                    Typeface = p.Typeface
                },
                new EquationBounds
                {
                    X = width / 2,
                    Y = height / 2 + yOffset,
                    Height = (int)(height * 0.8),
                    Width = (int)(width * 0.8)
                },
                0,
                out s
            );
            _elementsDrawer.DrawExpressions(elements, p, c);
        }
    }
}
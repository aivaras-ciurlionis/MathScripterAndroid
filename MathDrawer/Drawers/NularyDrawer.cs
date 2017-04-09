using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class NularyDrawer : IDrawer
    {
        private readonly ITextMeasurer _textMeasurer;

        public NularyDrawer(
            ITextMeasurer textMeasurer)
        {
            _textMeasurer = textMeasurer;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            var expressionName = expression.Name;
            var drawX = bounds.X;
            var drawY = bounds.Y;
            var operationElement = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = expressionName,
                X = drawX,
                Y = drawY,
                Size = p.Size
            };
            var drawableExpressions = new List<DrawableExpression>
            {
                new DrawableExpression
                {
                    Elements = new List<DrawableElement> {operationElement}
                }
            };
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var name = expression.Name;
            var exprBounds = _textMeasurer.GetTextBounds(name, p);
            return new EquationBounds
            {
                Height = exprBounds.Height(),
                Width = exprBounds.Width(),
                CenterOffset = 0
            };
        }
    }
}
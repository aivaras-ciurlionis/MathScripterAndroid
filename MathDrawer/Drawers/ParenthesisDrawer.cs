using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class ParenthesisDrawer : IDrawer
    {
        private readonly IBoundsMeasurer _boundsMeasurer;
        private readonly IDrawerFactory _drawerFactory;
        private readonly ITextMeasurer _textMeasurer;

        public ParenthesisDrawer(IDrawerFactory drawerFactory,
            IBoundsMeasurer boundsMeasurer,
            ITextMeasurer textMeasurer)
        {
            _boundsMeasurer = boundsMeasurer;
            _textMeasurer = textMeasurer;
            _drawerFactory = drawerFactory;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            if (bounds.Width < 0 || bounds.Height < 0)
            {
                var expressionBounds = GetBounds(expression, p);
                bounds.Width = expressionBounds.Width;
                bounds.Height = expressionBounds.Height;
            }

            const string expressionName = "(";
            var realBounds = _textMeasurer.GetTextBounds(expressionName, p);
            var textBounds = _textMeasurer.GetTextBounds(p);
            var operand = expression.Operands[0];
            var insBounds = _boundsMeasurer.GetOperandBounds(operand, p);

            var drawX = bounds.X;
            var drawY = bounds.Y + textBounds.Height() / 2;
            var operationElement1 = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = "(",
                X = drawX,
                Y = drawY,
                Size = p.Size
            };
            var operationElement2 = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = ")",
                X = drawX + realBounds.Width() + insBounds.Width,
                Y = drawY,
                Size = p.Size
            };

            insBounds.Y = bounds.Y;
            insBounds.X = drawX + realBounds.Width();
            var insDrawables = _drawerFactory.GetDrawer(operand).DrawExpression(operand, p, insBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(insDrawables);
            drawableExpressions.Add(new DrawableExpression
            {
                Elements = new List<DrawableElement> { operationElement1, operationElement2 }
            });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var name = expression.Name;
            var exprBounds = _textMeasurer.GetTextBounds(name, p);
            var operand = expression.Operands[0];
            var bounds = _boundsMeasurer.GetOperandBounds(operand, p);
            return new EquationBounds
            {
                Height = bounds.Height,
                Width = exprBounds.Width() + bounds.Width
            };
        }
    }
}
using System;
using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class UnaryDrawer : IDrawer
    {
        private readonly IBoundsMeasurer _boundsMeasurer;
        private readonly IDrawerFactory _drawerFactory;
        private readonly ITextMeasurer _textMeasurer;

        public UnaryDrawer(
            IBoundsMeasurer boundsMeasurer,
            IDrawerFactory drawerFactory,
            ITextMeasurer textMeasurer)
        {
            _boundsMeasurer = boundsMeasurer;
            _drawerFactory = drawerFactory;
            _textMeasurer = textMeasurer;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            var expressionName = expression.Name;
            var realBounds = _textMeasurer.GetTextBounds(expressionName, p);
            var operand = expression.Operands[0];
            var insBounds = _boundsMeasurer.GetOperandBounds(operand, p);
            var drawX = bounds.X;
            var drawY = bounds.Y;
            var centerPosition = insBounds.CenterOffset;
            var operationElement = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = expressionName,
                X = drawX,
                Y = centerPosition > 0 ? drawY - centerPosition + _textMeasurer.GetGenericTextHeight(p) / 2 : drawY,
                Size = p.Size
            };
            insBounds.Y = bounds.Y;
            insBounds.X = drawX + realBounds.Width();
            var insDrawables = _drawerFactory.GetDrawer(operand).DrawExpression(operand, p, insBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(insDrawables);
            drawableExpressions.Add(new DrawableExpression
            {
                Id = expression.Id,
                Elements = new List<DrawableElement> { operationElement }
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
                Height = Math.Max(bounds.Height, exprBounds.Height()),
                Width = exprBounds.Width() + bounds.Width,
                CenterOffset = bounds.CenterOffset
            };
        }
    }
}
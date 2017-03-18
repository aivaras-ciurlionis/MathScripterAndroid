using System;
using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class BinaryDrawer : IDrawer
    {
        private readonly IBoundsMeasurer _boundsMeasurer;
        private readonly IDrawerFactory _drawerFactory;
        private readonly ITextMeasurer _textMeasurer;

        public BinaryDrawer(IDrawerFactory drawerFactory,
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

            var expressionName = expression.Name;

            var realBounds = _textMeasurer.GetTextBounds(expressionName, p);

            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];

            var leftBounds = _boundsMeasurer.GetOperandBounds(leftOperand, p);
            var rightBounds = _boundsMeasurer.GetOperandBounds(rightOperand, p);

            var drawX = bounds.X + leftBounds.Width;
            var drawY = bounds.Y + _textMeasurer.GetGenericTextHeight(p) / 2;
            var operationElement = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = expressionName,
                X = drawX,
                Y = drawY,
                Size = p.Size
            };

            leftBounds.Y = bounds.Y;
            leftBounds.X = drawX - leftBounds.Width;

            rightBounds.Y = bounds.Y;
            rightBounds.X = drawX + realBounds.Width();

            var leftDrawables = _drawerFactory.GetDrawer(leftOperand).DrawExpression(leftOperand, p, leftBounds);
            var rightDrawables = _drawerFactory.GetDrawer(rightOperand).DrawExpression(rightOperand, p, rightBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(leftDrawables);
            drawableExpressions.AddRange(rightDrawables);
            drawableExpressions.Add(new DrawableExpression { Elements = new List<DrawableElement> { operationElement } });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var name = expression.Name;
            var exprBounds = _textMeasurer.GetTextBounds(name, p);
            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];
            var leftBounds = _boundsMeasurer.GetOperandBounds(leftOperand, p);
            var rightBounds = _boundsMeasurer.GetOperandBounds(rightOperand, p);
            return new EquationBounds
            {
                Height = Math.Max(leftBounds.Height, rightBounds.Height),
                Width = exprBounds.Width() + leftBounds.Width + rightBounds.Width
            };
        }
    }
}
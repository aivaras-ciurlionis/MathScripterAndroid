using System;
using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

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
            var expressionName = expression.Name;
            var realBounds = _textMeasurer.GetTextBounds(expressionName, p);
            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];
            var leftBounds = _boundsMeasurer.GetOperandBounds(leftOperand, p);
            var rightBounds = _boundsMeasurer.GetOperandBounds(rightOperand, p);
            var centerPosition = Math.Max(leftBounds.CenterOffset, rightBounds.CenterOffset);
            var drawX = bounds.X + leftBounds.Width;
            var drawY = bounds.Y;
            var operationElement = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = expressionName,
                X = drawX,
                Y = centerPosition > 0 ? drawY - centerPosition + _textMeasurer.GetGenericTextHeight(p) / 2 : drawY,
                Size = p.Size
            };
            var leftD = centerPosition - leftBounds.CenterOffset;
            var rightD = centerPosition - rightBounds.CenterOffset;
            var offsetD = Math.Abs(leftBounds.CenterOffset - rightBounds.CenterOffset);
            leftBounds.Y = (int)(bounds.Y - leftD + (leftD == centerPosition && centerPosition > 0 ? _textMeasurer.GetGenericTextHeight(p) / 2 : 0));
            leftBounds.X = drawX - leftBounds.Width;
            rightBounds.Y = (int)(bounds.Y - rightD + (rightD == centerPosition && centerPosition > 0 ? _textMeasurer.GetGenericTextHeight(p) / 2 : 0));
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
                Width = exprBounds.Width() + leftBounds.Width + rightBounds.Width,
                CenterOffset = Math.Max(leftBounds.CenterOffset, rightBounds.CenterOffset)
            };
        }
    }
}
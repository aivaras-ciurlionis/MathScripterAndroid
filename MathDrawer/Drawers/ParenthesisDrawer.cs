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
        private readonly IParenthesisChecker _parenthesisChecker;

        public ParenthesisDrawer(IDrawerFactory drawerFactory,
            IBoundsMeasurer boundsMeasurer,
            ITextMeasurer textMeasurer,
            IParenthesisChecker parenthesisChecker)
        {
            _boundsMeasurer = boundsMeasurer;
            _textMeasurer = textMeasurer;
            _parenthesisChecker = parenthesisChecker;
            _drawerFactory = drawerFactory;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            const string expressionName = "(";
            var realBounds = _textMeasurer.GetTextBounds(expressionName, p);
            var operand = expression.Operands[0];
            var insBounds = _boundsMeasurer.GetOperandBounds(operand, p);
            var centerPosition = insBounds.CenterOffset;
            var drawX = bounds.X;
            var drawY = bounds.Y;
            var needsParenthesis = _parenthesisChecker.NeedsParenthesis(expression);
            var operationElement1 = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = "(",
                X = drawX,
                Y = centerPosition > 0 ? drawY - centerPosition + _textMeasurer.GetGenericTextHeight(p) / 2 : drawY,
                Size = p.Size
            };
            var operationElement2 = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = ")",
                X = drawX + realBounds.Width() + insBounds.Width,
                Y = centerPosition > 0 ? drawY - centerPosition + _textMeasurer.GetGenericTextHeight(p) / 2 : drawY,
                Size = p.Size
            };

            insBounds.Y = bounds.Y;
            insBounds.X = drawX + (needsParenthesis ? realBounds.Width() : 0);
            var insDrawables = _drawerFactory.GetDrawer(operand).DrawExpression(operand, p, insBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(insDrawables);
            if (needsParenthesis)
            {
                drawableExpressions.Add(new DrawableExpression
                {
                    Elements = new List<DrawableElement> { operationElement1, operationElement2 }
                });
            }
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var name = expression.Name;
            var needsParenthesis = _parenthesisChecker.NeedsParenthesis(expression);
            var exprBounds = _textMeasurer.GetTextBounds(name, p);
            var operand = expression.Operands[0];
            var bounds = _boundsMeasurer.GetOperandBounds(operand, p);
            return new EquationBounds
            {
                Height = bounds.Height,
                Width = (needsParenthesis ? exprBounds.Width() : 0) + bounds.Width,
                CenterOffset = bounds.CenterOffset
            };
        }
    }
}
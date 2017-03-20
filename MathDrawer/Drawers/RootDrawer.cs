using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class RootDrawer : IDrawer
    {
        private const float HeightModifier = 0.25f;
        private const float LeftPaddingModifier = 0.5f;

        private readonly IBoundsMeasurer _boundsMeasurer;
        private readonly IDrawerFactory _drawerFactory;

        public RootDrawer(
            IBoundsMeasurer boundsMeasurer,
            IDrawerFactory drawerFactory
            )
        {
            _boundsMeasurer = boundsMeasurer;
            _drawerFactory = drawerFactory;
        }


        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            var operand = expression.Operands[0];
            var insBounds = _boundsMeasurer.GetOperandBounds(operand, p);
            var drawX = bounds.X;
            var drawY = bounds.Y;
            var operationElement = new DrawableElement
            {
                Type = DrawableType.Root,
                Text = "sqrt",
                X = drawX,
                Y = drawY,
                Size = p.Size,
                Height = insBounds.Height + HeightModifier * p.Size,
                Width = insBounds.Width + LeftPaddingModifier * p.Size
            };
            insBounds.Y = drawY;
            insBounds.X = (int) (drawX + LeftPaddingModifier * p.Size);
            var insDrawables = _drawerFactory.GetDrawer(operand).DrawExpression(operand, p, insBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(insDrawables);
            drawableExpressions.Add(new DrawableExpression
            {
                Elements = new List<DrawableElement> { operationElement }
            });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var operand = expression.Operands[0];
            var bounds = _boundsMeasurer.GetOperandBounds(operand, p);
            return new EquationBounds
            {
                Height = (int)(bounds.Height + p.Size * HeightModifier),
                Width = (int)(bounds.Width + p.Size * LeftPaddingModifier),
                CenterOffset = bounds.CenterOffset
            };
        }
    }
}
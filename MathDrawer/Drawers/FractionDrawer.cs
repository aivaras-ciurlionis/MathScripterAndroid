using System;
using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class FractionDrawer : IDrawer
    {
        private const float SizeModifier = 0.8f;

        private readonly IBoundsMeasurer _boundsMeasurer;
        private readonly IDrawerFactory _drawerFactory;

        public FractionDrawer(IDrawerFactory drawerFactory, 
            IBoundsMeasurer boundsMeasurer)
        {
            _drawerFactory = drawerFactory;
            _boundsMeasurer = boundsMeasurer;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            if (bounds.Width < 0 || bounds.Height < 0)
            {
                var expressionBounds = GetBounds(expression, p);
                bounds.Width = expressionBounds.Width;
                bounds.Height = expressionBounds.Height;
            }

            p.Size *= SizeModifier;
            var topOperand = expression.Operands[0];
            var botOperand = expression.Operands[1];
            var topBounds = _boundsMeasurer.GetOperandBounds(topOperand, p);
            var botBounds = _boundsMeasurer.GetOperandBounds(botOperand, p);
            var drawX = bounds.X;
            topBounds.Y = bounds.Y - 10 - topBounds.Height / 2;
            topBounds.X = drawX + (bounds.Width - topBounds.Width) / 2;
            botBounds.Y = bounds.Y + 10 + botBounds.Height / 2;
            botBounds.X = drawX + (bounds.Width - botBounds.Width) / 2;
            var fractionElement = new DrawableElement
            {
                Height = p.Size * 0.05f,
                Type = DrawableType.Division,
                Size = p.Size,
                Width = bounds.Width,
                X = drawX,
                Y = bounds.Y
            };
            var topDrawables = _drawerFactory.GetDrawer(topOperand).DrawExpression(topOperand, p, topBounds);
            var botDrawables = _drawerFactory.GetDrawer(botOperand).DrawExpression(botOperand, p, botBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(topDrawables);
            drawableExpressions.AddRange(botDrawables);
            drawableExpressions.Add(new DrawableExpression { Elements = new List<DrawableElement> { fractionElement } });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            p.Size *= SizeModifier;
            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];
            var leftBounds = _boundsMeasurer.GetOperandBounds(leftOperand, p);
            var rightBounds = _boundsMeasurer.GetOperandBounds(rightOperand, p);
            return new EquationBounds
            {
                Height = 20 + leftBounds.Height + rightBounds.Height,
                Width = Math.Max(leftBounds.Width, rightBounds.Width)
            };
        }
    }
}
using System;
using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class FractionDrawer : IDrawer
    {
        private readonly IDrawerFactory _drawerFactory = new DrawerFactory();
        private Paint _paint;

        private EquationBounds GetOperandBounds(IExpression operand)
        {
            return _drawerFactory.GetDrawer(operand)
                .GetBounds(operand, _paint);
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, Paint p, EquationBounds bounds)
        {
            _paint = p;
            var originalSize = p.TextSize;
            if (bounds.Width < 0 || bounds.Height < 0)
            {
                var expressionBounds = GetBounds(expression, p);
                bounds.Width = expressionBounds.Width;
                bounds.Height = expressionBounds.Height;
            }

            _paint.TextSize *= 0.8f;

            var topOperand = expression.Operands[0];
            var botOperand = expression.Operands[1];

            var topBounds = GetOperandBounds(topOperand);
            var botBounds = GetOperandBounds(botOperand);

            var drawX = bounds.X;

            topBounds.Y = bounds.Y - 10 - topBounds.Height / 2;
            topBounds.X = drawX + (bounds.Width - topBounds.Width) / 2;

            botBounds.Y = bounds.Y + 10 + botBounds.Height / 2;
            botBounds.X = drawX + (bounds.Width - botBounds.Width) / 2;


            var fractionElement = new DrawableElement
            {
                Height = p.TextSize * 0.05f,
                Type = DrawableType.Division,
                Size = p.TextSize,
                Width = bounds.Width,
                X = drawX,
                Y = bounds.Y
            };

            var topDrawables = _drawerFactory.GetDrawer(topOperand).DrawExpression(topOperand, p, topBounds);
            var botDrawables = _drawerFactory.GetDrawer(botOperand).DrawExpression(botOperand, p, botBounds);
            p.TextSize = originalSize;
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(topDrawables);
            drawableExpressions.AddRange(botDrawables);
            drawableExpressions.Add(new DrawableExpression { Elements = new List<DrawableElement> { fractionElement } });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            _paint = p;
            var originalSize = p.TextSize;
            p.TextSize *= 0.8f;
            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];
            var leftBounds = GetOperandBounds(leftOperand);
            var rightBounds = GetOperandBounds(rightOperand);
            p.TextSize = originalSize;
            return new EquationBounds
            {
                Height = 20 + leftBounds.Height + rightBounds.Height,
                Width = Math.Max(leftBounds.Width, rightBounds.Width)
            };
        }
    }
}
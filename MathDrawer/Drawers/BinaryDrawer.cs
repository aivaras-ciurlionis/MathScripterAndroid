using System;
using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class BinaryDrawer : IDrawer
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
            if (bounds.Width < 0 || bounds.Height < 0)
            {
                var expressionBounds = GetBounds(expression, p);
                bounds.Width = expressionBounds.Width;
                bounds.Height = expressionBounds.Height;
            }

            var expressionName = expression.Name;

            var nameBounds = new Rect();
            p.GetTextBounds(expressionName, 0, expressionName.Length, nameBounds);

            var testBounds = new Rect();
            p.GetTextBounds("a", 0, expressionName.Length, testBounds);

            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];

            var leftBounds = GetOperandBounds(leftOperand);
            var rightBounds = GetOperandBounds(rightOperand);

            var hd = 0;

            var drawX = bounds.X + leftBounds.Width;
            var drawY = bounds.Y + testBounds.Height() / 2;
            var operationElement = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = expressionName,
                X = drawX,
                Y = drawY,
                Size = p.TextSize
            };

            leftBounds.Y = bounds.Y;
            leftBounds.X = drawX - leftBounds.Width;

            rightBounds.Y = bounds.Y;
            rightBounds.X = drawX + nameBounds.Width();

            var leftDrawables = _drawerFactory.GetDrawer(leftOperand).DrawExpression(leftOperand, p, leftBounds);
            var rightDrawables = _drawerFactory.GetDrawer(rightOperand).DrawExpression(rightOperand, p, rightBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(leftDrawables);
            drawableExpressions.AddRange(rightDrawables);
            drawableExpressions.Add(new DrawableExpression { Elements = new List<DrawableElement> { operationElement } });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            _paint = p;
            var name = expression.Name;
            var exprBounds = new Rect();
            p.GetTextBounds(name, 0, name.Length, exprBounds);

            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];
            var leftBounds = GetOperandBounds(leftOperand);
            var rightBounds = GetOperandBounds(rightOperand);
            return new EquationBounds
            {
                Height = Math.Max(leftBounds.Height, rightBounds.Height),
                Width = exprBounds.Width() + leftBounds.Width + rightBounds.Width
            };
        }
    }
}
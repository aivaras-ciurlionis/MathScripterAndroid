using System;
using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class ParenthesisDrawer : IDrawer
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

            const string expressionName = "(";
            var nameBounds = new Rect();
            p.GetTextBounds(expressionName, 0, expressionName.Length, nameBounds);
            var testBounds = new Rect();
            p.GetTextBounds("a", 0, expressionName.Length, testBounds);
            var operand = expression.Operands[0];
            var insBounds = GetOperandBounds(operand);

            var drawX = bounds.X;
            var drawY = bounds.Y + testBounds.Height() / 2;
            var operationElement1 = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = "(",
                X = drawX,
                Y = drawY,
                Size = p.TextSize
            };
            var operationElement2 = new DrawableElement
            {
                Type = DrawableType.Symbolic,
                Text = ")",
                X = drawX + +nameBounds.Width() + insBounds.Width,
                Y = drawY,
                Size = p.TextSize
            };

            insBounds.Y = bounds.Y;
            insBounds.X = drawX + nameBounds.Width();
            var insDrawables = _drawerFactory.GetDrawer(operand).DrawExpression(operand, p, insBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(insDrawables);
            drawableExpressions.Add(new DrawableExpression
            {
                Elements = new List<DrawableElement> { operationElement1, operationElement2 }
            });
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            _paint = p;
            var name = expression.Name;
            var exprBounds = new Rect();
            p.GetTextBounds(name, 0, name.Length, exprBounds);
            var operand = expression.Operands[0];
            var bounds = GetOperandBounds(operand);
            return new EquationBounds
            {
                Height = bounds.Height,
                Width = exprBounds.Width() + bounds.Width
            };
        }
    }
}
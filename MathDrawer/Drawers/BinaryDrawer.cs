using System;
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

        public void DrawExpression(IExpression expression, Paint p, Canvas c, EquationBounds bounds)
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

            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];

            var leftBounds = GetOperandBounds(leftOperand);
            var rightBounds = GetOperandBounds(leftOperand);

            var drawX = bounds.X + leftBounds.Width;
            var h = nameBounds.Height();
            var drawY = bounds.Y + bounds.Height / 2;

            c.DrawText(expressionName, drawX, drawY, p);

            leftBounds.Y = bounds.Y;
            leftBounds.X = drawX - leftBounds.Width;

            rightBounds.Y = bounds.Y;
            rightBounds.X = drawX + (nameBounds.Right - nameBounds.Left);

            _drawerFactory.GetDrawer(leftOperand).DrawExpression(leftOperand, p, c, leftBounds);
            _drawerFactory.GetDrawer(rightOperand).DrawExpression(rightOperand, p, c, rightBounds);
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
                Width = (int)exprBounds.Width() + leftBounds.Width + rightBounds.Width
            };
        }
    }
}
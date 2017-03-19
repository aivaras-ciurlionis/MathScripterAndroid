using System;
using System.Collections.Generic;
using Android.App.Backup;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class FractionDrawer : IDrawer
    {
        private const float SizeModifier = 0.8f;
        private const float GapModifier = 0.25f;

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
            p.Size *= SizeModifier;
            var topOperand = expression.Operands[0];
            var botOperand = expression.Operands[1];
            var topBounds = _boundsMeasurer.GetOperandBounds(topOperand, p);
            var botBounds = _boundsMeasurer.GetOperandBounds(botOperand, p);
            var drawX = bounds.X;
            topBounds.Y = (int) (bounds.Y - p.Size * GapModifier - botBounds.Height);
            topBounds.X = drawX + (bounds.Width - topBounds.Width) / 2;
            botBounds.Y = bounds.Y;
            botBounds.X = drawX + (bounds.Width - botBounds.Width) / 2;
            var fractionElement = new DrawableElement
            {
                Height = p.Size * 0.075f,
                Type = DrawableType.Division,
                Size = p.Size,
                Width = bounds.Width,
                X = drawX,
                Y = bounds.Y - botBounds.Height - p.Size * GapModifier / 2
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
                Height = (int)(p.Size * GapModifier + leftBounds.Height + rightBounds.Height),
                Width = Math.Max(leftBounds.Width, rightBounds.Width),
                CenterOffset = (int) (rightBounds.Height + p.Size * GapModifier / 2)
            };
        }
    }
}
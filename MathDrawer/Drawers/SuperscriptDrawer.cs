using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class SuperscriptDrawer : IDrawer
    {
        private const float SizeModifier = 0.7f;

        private readonly IBoundsMeasurer _boundsMeasurer;
        private readonly IDrawerFactory _drawerFactory;

        public SuperscriptDrawer(IDrawerFactory drawerFactory,
            IBoundsMeasurer boundsMeasurer)
        {
            _drawerFactory = drawerFactory;
            _boundsMeasurer = boundsMeasurer;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            var leftOperand = expression.Operands[0];
            var exponentOperand = expression.Operands[1];
            var leftBounds = _boundsMeasurer.GetOperandBounds(leftOperand, p);
            leftBounds.Y = bounds.Y;
            leftBounds.X = bounds.X;
            var leftDrawables = _drawerFactory.GetDrawer(leftOperand).DrawExpression(leftOperand, p, leftBounds);
            p.Size *= SizeModifier;
            var exponentBounds = _boundsMeasurer.GetOperandBounds(exponentOperand, p);
            exponentBounds.Y = leftBounds.Y - leftBounds.Height / 2;
            exponentBounds.X = bounds.X + leftBounds.Width;
            var rightDrawables = _drawerFactory.GetDrawer(exponentOperand).DrawExpression(exponentOperand, p, exponentBounds);
            var drawableExpressions = new List<DrawableExpression>();
            drawableExpressions.AddRange(leftDrawables);
            drawableExpressions.AddRange(rightDrawables);
            return drawableExpressions;
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var leftOperand = expression.Operands[0];
            var rightOperand = expression.Operands[1];
            var leftBounds = _boundsMeasurer.GetOperandBounds(leftOperand, p);
            p.Size *= SizeModifier;
            var rightBounds = _boundsMeasurer.GetOperandBounds(rightOperand, p);
            return new EquationBounds
            {
                Height = leftBounds.Height + (rightBounds.Height - leftBounds.Height / 2),
                Width = leftBounds.Width + rightBounds.Width,
                CenterOffset = leftBounds.CenterOffset
            };
        }
    }
}
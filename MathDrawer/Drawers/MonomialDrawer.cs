using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathDrawer.Drawers
{
    public class MonomialDrawer : IDrawer
    {

        private static void DrawVariable(IVariable v, int positionX, int positionY, Paint p, Canvas c)
        {
            var originalSize = p.TextSize;
            var exponent = v.Exponent.ToString(CultureInfo.InvariantCulture);
            var nameWidth = p.MeasureText(v.Name);
            var nameBounds = new Rect();
            var expBounds = new Rect();
            p.GetTextBounds(v.Name, 0, v.Name.Length, nameBounds);
            c.DrawText(v.Name, positionX, positionY, p);
            p.TextSize = (float) Math.Round(0.7 * originalSize);
            p.GetTextBounds(exponent, 0, exponent.Length, expBounds);
            var expHeight = Math.Abs(expBounds.Top - expBounds.Bottom);
            c.DrawText(exponent, positionX + nameWidth, (float)(positionY - 0.7 * expHeight), p);
            p.TextSize = originalSize;
        }

        public void DrawExpression(IExpression expression, Paint p, Canvas c, EquationBounds bounds)
        {
            var monomial = expression as Monomial;
            var coefficient = monomial.Coefficient.ToString(CultureInfo.InvariantCulture);
            var coefBounds = new Rect();
            p.GetTextBounds(coefficient, 0, coefficient.Length, coefBounds);
            c.DrawText(coefficient, bounds.X, bounds.Y + bounds.Height / 2, p);
            var posX = bounds.X + coefBounds.Width();
            var posY = bounds.Y + bounds.Height / 2;

            if (monomial.Variables == null)
            {
                return;
            }

            foreach (var variable in monomial.Variables)
            {
                DrawVariable(variable, posX, posY, p, c);
                posX += GetVariableBounds(variable, p).X;
            }
        }

        public EquationBounds GetBounds(IExpression expression, Paint p)
        {
            var monomial = expression as Monomial;
            var coefficient = monomial.Coefficient.ToString(CultureInfo.InvariantCulture);
            var coefBounds = new Rect();
            p.GetTextBounds(coefficient, 0, coefficient.Length, coefBounds);
            if (monomial.IsNumeral())
            {
                return new EquationBounds
                {
                    Height = coefBounds.Height(),
                    Width = coefBounds.Width()
                };
            }

            var boundings = monomial.Variables.Select(variable => GetVariableBounds(variable, p)).ToList();
            return new EquationBounds
            {
                Height = Math.Max(coefBounds.Height(), boundings.Max(b => b.Height)),
                Width = boundings.Sum(b => b.Width) + coefBounds.Width()
            };
        }

        private static EquationBounds GetVariableBounds(IVariable v, Paint p)
        {
            var name = v.Name;
            var exponent = v.Exponent.ToString(CultureInfo.InvariantCulture);
            var nameBounds = new Rect();
            var expBounds = new Rect();
            var originalSize = p.TextSize;
            p.GetTextBounds(name, 0, name.Length, nameBounds);
            p.TextSize = originalSize * 0.7f;
            p.GetTextBounds(exponent, 0, exponent.Length, expBounds);
            p.TextSize = originalSize;
            return new EquationBounds
            {
                Height = (int)(nameBounds.Height() + 0.7 * expBounds.Height()),
                Width = nameBounds.Width() + expBounds.Width()
            };
        }

    }
}
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

        private static IList<DrawableElement> DrawVariable(IVariable v, int positionX, int positionY, Paint p)
        {
            var elements = new List<DrawableElement>();
            var originalSize = p.TextSize;
            var exponent = v.Exponent.ToString(CultureInfo.InvariantCulture);
            var nameWidth = p.MeasureText(v.Name);
          
            var expBounds = new Rect();
            elements.Add(new DrawableElement
            {
                Text = v.Name,
                X = positionX,
                Y = positionY,
                Size = p.TextSize,
                Type = DrawableType.Symbolic
            });
            p.GetTextBounds(exponent, 0, exponent.Length, expBounds);
            var expHeight = Math.Abs(expBounds.Top - expBounds.Bottom);
            elements.Add(new DrawableElement
            {
                Text = exponent,
                X = positionX + nameWidth,
                Y = (float) (positionY - 0.7 * expHeight),
                Size = 0.7f * p.TextSize,
                Type = DrawableType.Symbolic
            });
            return elements;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, Paint p, EquationBounds bounds)
        {
            var elements = new List<DrawableElement>();
            var monomial = expression as Monomial;
            var coefficient = monomial.Coefficient.ToString(CultureInfo.InvariantCulture);
            var coefBounds = new Rect();
            p.GetTextBounds(coefficient, 0, coefficient.Length, coefBounds);

            elements.Add(new DrawableElement
            {
                Size = p.TextSize,
                X = bounds.X,
                Y = bounds.Y + bounds.Height / 2,
                Type = DrawableType.Symbolic,
                Text = coefficient
            });
            var posX = bounds.X + coefBounds.Width();
            var posY = bounds.Y + bounds.Height / 2;

            if (monomial.Variables != null)
            {
                foreach (var variable in monomial.Variables)
                {
                    elements.AddRange(DrawVariable(variable, posX, posY, p));
                    posX += GetVariableBounds(variable, p).X;
                }
            }

            
            var drawable = new DrawableExpression
            {
                Elements = elements
            };
            return new List<DrawableExpression> { drawable };
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
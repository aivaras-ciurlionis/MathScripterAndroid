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
        private const int RoundingPrecision = 3;

        private readonly ITextMeasurer _textMeasurer;
        private readonly IVariableDrawer _variableDrawer;

        public MonomialDrawer(ITextMeasurer textMeasurer,
            IVariableDrawer variableDrawer)
        {
            _textMeasurer = textMeasurer;
            _variableDrawer = variableDrawer;
        }

        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            var elements = new List<DrawableElement>();
            var monomial = expression as Monomial;
            var coefficient = Math.Round(monomial.Coefficient, RoundingPrecision).ToString(CultureInfo.InvariantCulture);
            var hasCoefficient = Math.Abs(monomial.Coefficient - 1) > 0.001;
            var coefBounds = _textMeasurer.GetTextBounds(coefficient, p);
            if (hasCoefficient || monomial.IsNumeral())
            {
                elements.Add(new DrawableElement
                {
                    Size = p.Size,
                    X = bounds.X,
                    Y = bounds.Y,
                    Type = DrawableType.Symbolic,
                    Text = coefficient
                });
            }
            var posX = bounds.X + (hasCoefficient ? coefBounds.Width() : 0);
            if (!monomial.IsNumeral())
            {
                foreach (var variable in monomial.Variables)
                {
                    elements.AddRange(_variableDrawer.DrawVariable(variable, posX, bounds.Y, p));
                    posX += _variableDrawer.GetVariableBounds(variable, p).Width;
                }
            }
            var drawable = new DrawableExpression
            {
                Id = expression.Id,
                Elements = elements
            };
            return new List<DrawableExpression> { drawable };
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            var monomial = expression as Monomial;
            var coefficient = Math.Round(monomial.Coefficient, RoundingPrecision).ToString(CultureInfo.InvariantCulture);
            var coefBounds = _textMeasurer.GetTextBounds(coefficient, p);
            if (monomial.IsNumeral())
            {
                return new EquationBounds
                {
                    Height = coefBounds.Height(),
                    Width = coefBounds.Width()
                };
            }
            var boundings = monomial.Variables.Select(variable => _variableDrawer.GetVariableBounds(variable, p)).ToList();
            var hasCoefficient = Math.Abs(monomial.Coefficient - 1) > 0.001;
            return new EquationBounds
            {
                Height = Math.Max(hasCoefficient ? coefBounds.Height() : 0, boundings.Max(b => b.Height)),
                Width = boundings.Sum(b => b.Width) + (hasCoefficient ? coefBounds.Width() : 0),
                CenterOffset = 0
            };
        }

    }
}
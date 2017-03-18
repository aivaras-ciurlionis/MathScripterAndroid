using System;
using System.Collections.Generic;
using System.Globalization;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class VariableDrawer : IVariableDrawer
    {
        private readonly ITextMeasurer _textMeasurer;
        private const float SizeModifier = 0.7f;

        public VariableDrawer(ITextMeasurer textMeasurer)
        {
            _textMeasurer = textMeasurer;
        }

        public EquationBounds GetVariableBounds(IVariable v, TextParameters p)
        {
            var nameBounds = _textMeasurer.GetTextBounds(v.Name, p);
            p.Size *= SizeModifier;
            var exponent = v.Exponent.ToString(CultureInfo.InvariantCulture);
            var hasExponent = Math.Abs(v.Exponent - 1) > 0.001;
            var expBounds = _textMeasurer.GetTextBounds(exponent, p);
            return new EquationBounds
            {
                Height = (int)(_textMeasurer.GetGenericTextHeight(p) +
                              (hasExponent ? SizeModifier * _textMeasurer.GetGenericTextHeight(p) : 0)),
                Width = nameBounds.Width() + (hasExponent ? expBounds.Width() : 0)
            };
        }

        public IEnumerable<DrawableElement> DrawVariable(IVariable v, int positionX, int positionY, TextParameters p)
        {
            var elements = new List<DrawableElement>();
            var exponent = v.Exponent.ToString(CultureInfo.InvariantCulture);
            var hasExponent = Math.Abs(v.Exponent - 1) > 0.001;
            var nameWidth = _textMeasurer.GetTextBounds(v.Name, p).Width();
            elements.Add(new DrawableElement
            {
                Text = v.Name,
                X = positionX,
                Y = positionY,
                Size = p.Size,
                Type = DrawableType.Symbolic
            });
            if (hasExponent)
            {
                elements.Add(new DrawableElement
                {
                    Text = exponent,
                    X = positionX + nameWidth,
                    Y = positionY - SizeModifier * _textMeasurer.GetGenericTextHeight(p),
                    Size = SizeModifier * p.Size,
                    Type = DrawableType.Symbolic
                });
            }
            return elements;
        }
    }
}
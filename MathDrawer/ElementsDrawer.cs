using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;

namespace MathDrawer
{
    public class ElementsDrawer : IElementsDrawer
    {

        private const float LeftPaddingModifier = 0.5f;

        public void DrawExpressions(IList<DrawableExpression> expressions, Paint p, Canvas c)
        {
            foreach (var drawableExpression in expressions)
            {
                foreach (var element in drawableExpression.Elements)
                {
                    p.Color = Color.Black;
                    switch (element.Type)
                    {
                        case DrawableType.Division:
                            p.StrokeWidth = element.Height;
                            c.DrawLine(element.X, element.Y, element.X + element.Width, element.Y, p);
                            break;
                        case DrawableType.Root:
                            var rootWidth = element.Size * LeftPaddingModifier;
                            p.StrokeWidth = element.Size * 0.075f;
                            c.DrawLine(element.X, element.Y - element.Height / 2, element.X + rootWidth / 2, element.Y, p);
                            c.DrawLine(element.X + rootWidth / 2, element.Y, element.X + rootWidth, element.Y - element.Height, p);
                            c.DrawLine(element.X + rootWidth, element.Y - element.Height, element.X + element.Width, element.Y - element.Height, p);
                            break;
                        case DrawableType.Symbolic:
                            p.TextSize = element.Size;
                            c.DrawText(element.Text, element.X, element.Y, p);
                            break;
                    }

                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;

namespace MathDrawer
{
    public class ElementsDrawer : IElementsDrawer
    {
        public void DrawExpressions(IList<DrawableExpression> expressions, Paint p, Canvas c)
        {
            foreach (var drawableExpression in expressions)
            {
                foreach (var element in drawableExpression.Elements)
                {
                    if (element.Type == DrawableType.Division)
                    {
                        p.StrokeWidth = element.Height;
                        c.DrawLine(element.X, element.Y, element.X+element.Width, element.Y, p);
                        continue;
                    }
                    p.TextSize = element.Size;
                    c.DrawText(element.Text, element.X, element.Y, p);
                }
            }
        }
    }
}
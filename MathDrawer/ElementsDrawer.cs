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
                    p.Color = Color.Black;
                    if (element.Type == DrawableType.Division)
                    {
                        p.StrokeWidth = element.Height;
                        c.DrawLine(element.X, element.Y, element.X+element.Width, element.Y, p);
                        continue;
                    }
                    if (element.Type == DrawableType.Root)
                    {
                        p.Color = element.Color;
                        c.DrawCircle(element.X, element.Y, 5, p);
                        continue;;
                    }
                    p.TextSize = element.Size;
                    c.DrawText(element.Text, element.X, element.Y, p);
                }
            }
        }
    }
}
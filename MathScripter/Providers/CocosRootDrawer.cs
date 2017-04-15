using CocosSharp;
using MathDrawer.Models;

namespace MathScripter.Providers
{
    public class CocosRootDrawer
    {
        public CCNode DrawRoot(DrawableElement element, float maxY)
        {
            //var rootWidth = element.Size * LeftPaddingModifier;
            //p.StrokeWidth = element.Size * 0.075f;
            //c.DrawLine(element.X, element.Y - element.Height / 2 + offsetY, element.X + rootWidth / 2, element.Y + offsetY, p);
            //c.DrawLine(element.X + rootWidth / 2, element.Y + offsetY, element.X + rootWidth, element.Y - element.Height + offsetY, p);
            //c.DrawLine(element.X + rootWidth, element.Y - element.Height + offsetY, element.X + element.Width, element.Y - element.Height + offsetY, p);
            var strokeW = element.Size * 0.075f * 0.5f;
            var rootWidth = element.Size * 0.5f;
            var drawNode = new CCDrawNode
            {
                PositionX = element.X,
                PositionY = maxY - element.Y + strokeW * 5
            };
            drawNode.DrawLine(
                new CCPoint(0, element.Height / 2),
                new CCPoint(rootWidth / 2, 0),
                strokeW,
                CCColor4B.Black
            );

            drawNode.DrawLine(
                new CCPoint(rootWidth / 2, 0),
                new CCPoint(rootWidth, element.Height),
                strokeW,
                CCColor4B.Black
            );

            drawNode.DrawLine(
                new CCPoint(rootWidth, element.Height),
                new CCPoint(element.Width, element.Height),
                strokeW,
                CCColor4B.Black
            );
            return drawNode;
        }
    }
}
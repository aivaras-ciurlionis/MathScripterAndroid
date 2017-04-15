using CocosSharp;
using MathDrawer.Models;

namespace MathScripter.Providers
{
    public class CocosFractionDrawer
    {
        public CCNode DrawFraction(DrawableElement element, float maxY)
        {
            var drawNode = new CCDrawNode
            {
                PositionX = element.X,
                PositionY = maxY - element.Y + element.Height*3,
                Opacity = byte.MaxValue
            };
            drawNode.DrawLine(
                new CCPoint(0, 0),
                new CCPoint(element.Width, 0),
                element.Height / 2,
                CCColor4B.Black
            );
            return drawNode;
        }
    }
}
using CocosSharp;
using MathDrawer.Models;

namespace MathScripter.Providers
{
    public class CocosTextDrawer
    {
        public CCNode DrawText(DrawableElement element, float maxY)
        {
            var drawNode = new CCLabel(element.Text ?? "", "Georgia",
                     element.Size, CCLabelFormat.SystemFont)
            {
                PositionX = element.X,
                PositionY = maxY - element.Y,
                AnchorPoint = CCPoint.AnchorLowerLeft,
                Color = CCColor3B.Black
            };
            return drawNode;
        }
    }
}
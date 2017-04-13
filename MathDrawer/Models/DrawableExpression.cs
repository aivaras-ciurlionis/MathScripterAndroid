using System.Collections.Generic;

namespace MathDrawer.Models
{
    public class DrawableExpression
    {
        public string Id { get; set; }
        public IList<DrawableElement> Elements { get; set; }
    }
}
using System.Collections.Generic;

namespace MathRecongizer.Models
{
    public class TraversePixelResult
    {
        public Dictionary<int, IList<int>> Boundary { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public bool BoundaryHit { get; set; }
    }
}
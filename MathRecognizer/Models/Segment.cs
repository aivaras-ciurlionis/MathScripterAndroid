namespace MathRecognizer.Models
{
    public class Segment
    {
        public byte[] Pixels { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
    }
}
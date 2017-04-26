namespace MathRecognizer.Models
{
    public class NamedSegment
    {
        public string SegmentName { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public override string ToString()
        {
            return $"  {SegmentName}    {MinX}:{MinY} - {MaxX}:{MaxY}";
        }

        public bool HasSuperscript(NamedSegment next)
        {

            var height = MaxY - MinY;
            var heightMult = "-+.,=".Contains(next.SegmentName) ? 0.8 : 0.35;
            return next.MaxY < MaxY - height * heightMult &&
                  next.MaxY < MaxY;
        }

    }
}
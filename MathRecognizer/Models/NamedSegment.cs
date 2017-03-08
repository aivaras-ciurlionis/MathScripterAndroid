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
            return next.MaxY < MaxY - height / 2 &&
                   next.MinY < MinY && next.MaxY < MaxY;
        }

    }
}
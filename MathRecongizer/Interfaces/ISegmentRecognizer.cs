using MathRecongizer.Models;

namespace MathRecongizer.Interfaces
{
    public interface ISegmentRecognizer
    {
        NamedSegment RecognizeSegment(Segment segment);
    }
}
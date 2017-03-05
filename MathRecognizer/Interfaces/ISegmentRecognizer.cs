using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ISegmentRecognizer
    {
        NamedSegment RecognizeSegment(Segment segment);
    }
}
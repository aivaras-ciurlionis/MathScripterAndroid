using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface IRectangleDistanceFinder
    {
        double DistanceBetweenSegments(NamedSegment s1, NamedSegment s2);
    }
}
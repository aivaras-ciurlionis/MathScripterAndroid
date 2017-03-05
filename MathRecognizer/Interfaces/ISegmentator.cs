using System.Collections.Generic;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ISegmentator
    {
        IEnumerable<Segment> GetImageSegments(byte[] pixels, int width, int height);
    }
}
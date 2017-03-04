using System.Collections.Generic;
using MathRecongizer.Models;

namespace MathRecongizer.Interfaces
{
    public interface ISegmentator
    {
        IEnumerable<Segment> GetImageSegments(byte[] pixels);
    }
}
using System.Collections.Generic;
using ImageSharp;
using MathRecognizer.Models;

namespace MathRecognizer.Interfaces
{
    public interface ISegmentsResizer
    {
        IEnumerable<Image> ResizeSegments(IEnumerable<Segment> segments);
        IEnumerable<Segment> ResizeSegmentsPixels(IEnumerable<Segment> segments);
    }
}
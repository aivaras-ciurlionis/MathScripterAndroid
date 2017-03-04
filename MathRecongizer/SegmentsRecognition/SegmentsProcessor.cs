using System.Collections.Generic;
using System.Linq;
using MathRecongizer.Interfaces;
using MathRecongizer.Models;

namespace MathRecongizer.SegmentsRecognition
{
    public class SegmentsProcessor : ISegmentsProcessor
    {
        private readonly INeuralNetwork _network;
        private readonly IIndexMapper _indexMapper;

        public SegmentsProcessor(
            INeuralNetwork network,
            IIndexMapper indexMapper
            )
        {
            _network = network;
            _indexMapper = indexMapper;
        }

        public IEnumerable<NamedSegment> RecognizeSegments(IEnumerable<Segment> segments)
        {
            return (
                from segment in segments
                let segmentName = _indexMapper.GetIndexName(_network.GetPrediction(segment.Pixels))
                select new NamedSegment
                {
                    MaxX = segment.MaxX,
                    MaxY = segment.MaxY,
                    MinX = segment.MinX,
                    MinY = segment.MinY,
                    SegmentName = segmentName
                }).ToList();
        }
    }
}
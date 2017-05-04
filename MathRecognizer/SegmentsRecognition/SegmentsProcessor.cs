using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;
using MathServerConnector.Interfaces;

namespace MathRecognizer.SegmentsRecognition
{
    public class SegmentsProcessor : ISegmentsProcessor
    {
        private readonly INeuralNetwork _network;
        private readonly IIndexMapper _indexMapper;
        private readonly IImageDataUploader _imageDataUploader;

        public SegmentsProcessor(
            INeuralNetwork network,
            IIndexMapper indexMapper, 
            IImageDataUploader imageDataUploader)
        {
            _network = network;
            _indexMapper = indexMapper;
            _imageDataUploader = imageDataUploader;
        }

        public IEnumerable<NamedSegment> RecognizeSegments(IEnumerable<Segment> segments)
        {
            var list = new List<NamedSegment>();
            foreach (var segment in segments)
            {
                var segmentName = _indexMapper.GetIndexName(_network.GetPrediction(segment.Pixels));
                _imageDataUploader.UploadImageData(segment.Pixels, segmentName);
                list.Add(new NamedSegment
                {
                    MaxX = segment.MaxX, MaxY = segment.MaxY, MinX = segment.MinX, MinY = segment.MinY, SegmentName = segmentName
                });
            }
            return list;
        }
    }
}
using System.Collections.Generic;
using Android.Graphics;
using MathRecongizer.Interfaces;

namespace MathRecongizer
{
    public class Recognizer : IRecognizer
    {
        private readonly IImageDecoder _imageDecoder;
        private readonly ISegmentator _segmentator;
        private readonly ISegmentsProcessor _segmentsProcessor;
        private readonly IEquationsBuilder _equationsBuilder;

        public Recognizer(
            IImageDecoder imageDecoder,
            ISegmentator segmentator,
            ISegmentsProcessor segmentsProcessor,
            IEquationsBuilder equationsBuilder
            )
        {
            _imageDecoder = imageDecoder;
            _segmentator = segmentator;
            _segmentsProcessor = segmentsProcessor;
            _equationsBuilder = equationsBuilder;
        }

        public IEnumerable<string> GetEquationsInImage(Bitmap image)
        {
            var imagePixels = _imageDecoder.GetPixels(image);
            var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
            var namedSegments = _segmentsProcessor.RecognizeSegments(segments);
            return _equationsBuilder.GetEquations(namedSegments);
        }

    }
}
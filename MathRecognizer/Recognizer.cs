using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using ImageSharp;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer
{
    public class Recognizer : IRecognizer
    {
        private readonly IImageDecoder _imageDecoder;
        private readonly ISegmentator _segmentator;
        private readonly ISegmentsProcessor _segmentsProcessor;
        private readonly ISegmentsSplitter _segmentsSplitter;

        private readonly ISegmentsResizer _segmentsResizer;

        public Recognizer(
            IImageDecoder imageDecoder,
            ISegmentator segmentator,
            ISegmentsResizer segmentsResizer,
            ISegmentsProcessor segmentsProcessor,
            ISegmentsSplitter segmentsSplitter)
        {
            _imageDecoder = imageDecoder;
            _segmentator = segmentator;
            _segmentsResizer = segmentsResizer;
            _segmentsProcessor = segmentsProcessor;
            _segmentsSplitter = segmentsSplitter;
        }

        //public IEnumerable<NamedSegment> GetEquationsInImage(Bitmap image)
        //{
        //    var imagePixels = _imageDecoder.GetPixels(image);
        //    var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
        //    var resizedSegments = _segmentsResizer.ResizeSegmentsPixels(segments);
        //    return _segmentsProcessor.RecognizeSegments(resizedSegments);
        //}

        //public IEnumerable<Image> GetSegmentsInImage(Bitmap image)
        //{
        //    var imagePixels = _imageDecoder.GetPixels(image);
        //    Console.WriteLine("Image decoded");
        //    var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
        //    return _segmentsResizer.ResizeSegments(segments); 
        //}

        public IEnumerable<NamedSegment> GetEquationsInImage(Image image)
        {
            var imagePixels = _imageDecoder.GetPixels(image);
            var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
            var resizedSegments = _segmentsResizer.ResizeSegmentsPixels(segments);
            var namedSegments = _segmentsProcessor.RecognizeSegments(resizedSegments);
            var equationsInImage = namedSegments as NamedSegment[] ?? namedSegments.ToArray();
            var splitSegments = _segmentsSplitter.SplitSegments(equationsInImage, (image.Height + image.Width) / 2);
            return equationsInImage;
        }
    }
}
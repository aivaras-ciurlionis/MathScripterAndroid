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
        private readonly IEquationsBuilder _equationsBuilder;
        private readonly IPixelsToImageConverter _imageConverter;

        private readonly ISegmentsResizer _segmentsResizer;

        public Recognizer(
            IImageDecoder imageDecoder,
            ISegmentator segmentator,
            ISegmentsResizer segmentsResizer,
            ISegmentsProcessor segmentsProcessor,
            IEquationsBuilder equationsBuilder, 
            IPixelsToImageConverter imageConverter)
        {
            _imageDecoder = imageDecoder;
            _segmentator = segmentator;
            _segmentsResizer = segmentsResizer;
            _segmentsProcessor = segmentsProcessor;
            _equationsBuilder = equationsBuilder;
            _imageConverter = imageConverter;
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

        public IEnumerable<string> GetEquationsInImage(Image image)
        {
            var imagePixels = _imageDecoder.GetPixels(image);
            var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
            var resizedSegments = _segmentsResizer.ResizeSegmentsPixels(segments);
            var namedSegments = _segmentsProcessor.RecognizeSegments(resizedSegments);
            var enumerable = namedSegments as NamedSegment[] ?? namedSegments.ToArray();
            return _equationsBuilder.GetEquations(enumerable, (image.Height + image.Width) / 2);
        }

        public IEnumerable<Image> GetSegmentsInImage(Image image)
        {
            var imagePixels = _imageDecoder.GetPixels(image);
            var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
            var resizedSegments = _segmentsResizer.ResizeSegmentsPixels(segments);
            return resizedSegments.Select(s =>  _imageConverter.GetImage(s.Pixels, 64, 64));


            //var namedSegments = _segmentsProcessor.RecognizeSegments(resizedSegments);
            //var enumerable = namedSegments as NamedSegment[] ?? namedSegments.ToArray();
            //return _equationsBuilder.GetEquations(enumerable, (image.Height + image.Width) / 2);
        }


    }
}
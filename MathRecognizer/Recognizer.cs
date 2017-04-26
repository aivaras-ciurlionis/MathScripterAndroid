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
        private readonly IEquationBuilder _equationBuilder;
        private readonly IPixelsToImageConverter _imageConverter;

        private readonly ISegmentsResizer _segmentsResizer;
        private readonly IExpressionStripper _expressionStripper;

        public Recognizer(
            IImageDecoder imageDecoder,
            ISegmentator segmentator,
            ISegmentsResizer segmentsResizer,
            ISegmentsProcessor segmentsProcessor,
            IEquationBuilder equationBuilder,
            IPixelsToImageConverter imageConverter,
            IExpressionStripper expressionStripper)
        {
            _imageDecoder = imageDecoder;
            _segmentator = segmentator;
            _segmentsResizer = segmentsResizer;
            _segmentsProcessor = segmentsProcessor;
            _equationBuilder = equationBuilder;
            _imageConverter = imageConverter;
            _expressionStripper = expressionStripper;
        }

        public string GetEquationsInImage(Image image)
        {
            var processed = GetProcessedImage(image, 10, 0);
            var imagePixels = processed.Pixels.Select(s => s.R).ToArray();
            var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
            var resizedSegments = _segmentsResizer.ResizeSegmentsPixels(segments);
            var namedSegments = _segmentsProcessor.RecognizeSegments(resizedSegments);
            var enumerable = namedSegments as NamedSegment[] ?? namedSegments.ToArray();
            return _expressionStripper.StripExpression(_equationBuilder.GetEquation(enumerable));
        }

        public Image GetProcessedImage(Image image, int contrast, int brightness)
        {
            var imagePixels = _imageDecoder.GetPixels(image);
            var factor = (float)(259 * (contrast + 255)) / (255 * (259 - contrast));
            var contrasted = new Image(image.Width, image.Height);
            var i = 0;
            foreach (var pixel in imagePixels)
            {
                var br = Math.Min(pixel + brightness, 255);
                var color = (factor * (br - 128) + 128) / 255;
                contrasted.Pixels[i] = new ImageSharp.Color(color, color, color);
                i++;
            }
            return (Image)contrasted;
        }

        public IEnumerable<Image> GetSegmentsInImage(Image image)
        {
            var imagePixels = _imageDecoder.GetPixels(image);
            var segments = _segmentator.GetImageSegments(imagePixels, image.Width, image.Height);
            var resizedSegments = _segmentsResizer.ResizeSegmentsPixels(segments);
            return resizedSegments.Select(s => _imageConverter.GetImage(s.Pixels, 64, 64));
        }

    }
}
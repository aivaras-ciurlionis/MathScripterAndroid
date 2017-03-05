using System.Collections.Generic;
using System.Linq;
using ImageSharp;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.Segmentation
{
    public class SegmentsResizer : ISegmentsResizer
    {
        private const int ResizeSize = 64;

        private readonly IPixelsToImageConverter _imageConverter;
        private readonly ICenterOfMassComputor _centerOfMassComputor;
        private readonly IImageMover _imageMover;
        private readonly IRatioResizer _ratioResizer;

        public SegmentsResizer(
            IPixelsToImageConverter imageConverter,
            ICenterOfMassComputor centerOfMassComputor,
            IImageMover imageMover, 
            IRatioResizer ratioResizer
            )
        {
            _imageConverter = imageConverter;
            _centerOfMassComputor = centerOfMassComputor;
            _imageMover = imageMover;
            _ratioResizer = ratioResizer;
        }

        public IEnumerable<Image> ResizeSegments(IEnumerable<Segment> segments)
        {
            if (segments == null)
            {
                return null;
            }

            var enumSegments = segments as IList<Segment> ?? segments.ToList();
            return enumSegments.Select(ResizeSegment).ToList();
        }

        private Segment ResizeSegmentPixels(Segment segment)
        {
            var tempImage = ResizeSegment(segment);
            segment.Pixels = tempImage.Pixels.Select(p => p.R).ToArray();
            return segment;
        }

        private Image ResizeSegment(Segment segment)
        {
            var width = segment.MaxX - segment.MinX + 20;
            var height = segment.MaxY - segment.MinY + 20;
            var currentCenter = new Models.Point(width / 2, height / 2);
            var massCenter = _centerOfMassComputor.GetCenterOfMass(segment.Pixels, width, height);
            var dx = currentCenter.X - massCenter.X;
            var dy = currentCenter.Y - massCenter.Y;
            segment.Pixels = _imageMover.MovePixels(segment.Pixels, width, height, dx, dy);
            var image = _imageConverter.GetImage(segment.Pixels, width, height);
            return _ratioResizer.ResizeImage(image, ResizeSize);
        }

        public IEnumerable<Segment> ResizeSegmentsPixels(IEnumerable<Segment> segments)
        {
            if (segments == null)
            {
                return null;
            }

            var enumSegments = segments as IList<Segment> ?? segments.ToList();
            return enumSegments.Select(ResizeSegmentPixels).ToList();
        }
    }
}
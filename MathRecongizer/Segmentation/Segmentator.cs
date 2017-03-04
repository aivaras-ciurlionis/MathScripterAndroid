using System.Collections;
using System.Collections.Generic;
using MathRecongizer.Interfaces;
using MathRecongizer.Models;

namespace MathRecongizer.Segmentation
{
    public class Segmentator : ISegmentator
    {
        private int _width;
        private int _height;
        private byte[] _pixels;
        private bool[] _usedPoints;
        private const int Treshold = 80;
        private const int Padding = 10;

        private Point[] Directions =
        {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0)
        };

        private byte? GetPixelAt(Point p)
        {
            return GetPixelAt(p.X, p.Y);
        }

        private byte? GetPixelAt(int x, int y)
        {
            if (x < 0 || y < 0 || x > _width || y > _height)
            {
                return null;
            }

            return _pixels[y * _width + x];
        }

        private void MarkUsed(int x, int y)
        {
            _usedPoints[y * _width + x] = true;
        }

        private bool PointIsUsed(int x, int y)
        {
            return _usedPoints[y * _width + x];
        }

        private Segment ProcessIfNotUsed(int x, int y)
        {
            return (PointIsUsed(x, y) || GetPixelAt(x, y) < Treshold) ? null : processPixel(x, y);
        }

        private TraversePixelResult TraverseChar(int x, int y)
        {
            var boundary = new Dictionary<int, IList<int>>();
            var currentPoint = new Point(-1, -1);
            var startPoint = new Point(x, y);
            var directionI = 0;
            var minY = 0;
            var maxY = -1;
            var minX = 1000;
            var maxX = -1;
            var boundaryHit = false;
            while (!currentPoint.Equals(startPoint))
            {
                var directionP = Directions[(directionI + 1) % 4];
                var currentDirection = Directions[directionI];

                if (currentPoint.X < 0)
                {
                    currentPoint = startPoint.Clone();
                }

                var pointP = currentPoint + directionP;
                var nextPoint = currentPoint + currentDirection;

                if (boundary.ContainsKey(currentPoint.Y))
                {
                    boundary[currentPoint.Y].Add(currentPoint.X);
                }
                else
                {
                    boundary.Add(currentPoint.Y, new List<int> { currentPoint.X });
                }

                var nextValue = GetPixelAt(nextPoint);

                if (nextValue == null)
                {
                    boundaryHit = true;
                    do
                    {
                        currentPoint += Directions[(directionI + 1) % 4];
                        nextValue = GetPixelAt(currentPoint + Directions[(directionI + 1) % 4]);
                    } while (nextValue > Treshold);
                }

                var nearValue = GetPixelAt(pointP);

                if (nearValue <= Treshold)
                {
                    currentPoint += Directions[(directionI + 1) % 4];
                }
                else if (nextValue > Treshold)
                {
                    directionI -= 1;
                    directionI %= 4;
                }
                else
                {
                    currentPoint += currentDirection;
                }

                if (currentPoint.X < minX)
                {
                    minX = currentPoint.X;
                }
                if (currentPoint.X > maxX)
                {
                    maxX = currentPoint.X;
                }
                if (currentPoint.Y < minY)
                {
                    minY = currentPoint.Y;
                }
                if (currentPoint.Y > maxY)
                {
                    maxY = currentPoint.Y;
                }
            }
            return new TraversePixelResult
            {
                Boundary = boundary,
                MaxX = maxX,
                MaxY = maxY,
                MinX = minX,
                MinY = minY,
                BoundaryHit = boundaryHit
            };
        }

        private BoundaryRowResult ProcessBoundaryRow(int y, IEnumerable<int> row)
        {

        }

        private byte[] CopyBoundaryValues(TraversePixelResult pixelResult)
        {
            var pixels = new byte[(pixelResult.MaxX - pixelResult.MinX + Padding * 2) *
                (pixelResult.MaxY - pixelResult.MinY + Padding * 2)];
            foreach (var boundaryRow in pixelResult.Boundary)
            {
                var result = ProcessBoundaryRow(boundaryRow.Key, boundaryRow.Value);
                foreach (var pointValue in result.PointValues)
                {
                    pixels[pointValue.Point.Y * _width + pointValue.Point.X] = pointValue.Value;
                }
            }
            return pixels;
        }

        private Segment processPixel(int x, int y)
        {
            var traverseResult = TraverseChar(x, y);
            return traverseResult.BoundaryHit ? null :
                new Segment
                {
                    MaxX = traverseResult.MaxX,
                    MinX = traverseResult.MinX,
                    MinY = traverseResult.MinY,
                    MaxY = traverseResult.MaxY,
                    Pixels = CopyBoundaryValues(traverseResult)
                };
        }

        public IEnumerable<Segment> GetImageSegments(byte[] pixels, int width, int height)
        {
            _width = width;
            _height = height;
            _pixels = pixels;
            _usedPoints = new bool[width * height];
            var segments = new List<Segment>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var segment = ProcessIfNotUsed(x, y);
                    if (segment != null)
                    {
                        segments.Add(segment);
                    }
                }
            }
            return segments;
        }
    }
}
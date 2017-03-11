using System;
using System.Collections.Generic;
using System.Linq;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.Segmentation
{
    public class Segmentator : ISegmentator
    {
        private int _width;
        private int _height;
        private byte[] _pixels;
        private bool[] _usedPoints;
        private const int Treshold = 100;
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
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                return null;
            }
            var p = _pixels[y * _width + x];
            return p;
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
            return (PointIsUsed(x, y) || GetPixelAt(x, y) < Treshold) ? null : ProcessPixel(x - 1, y);
        }

        private TraversePixelResult TraverseChar(int x, int y)
        {
            Console.WriteLine($"traversing char at {x}:{y}");
            var boundary = new Dictionary<int, IList<int>>();
            var currentPoint = new Point(-1, -1);
            var startPoint = new Point(x, y);
            var directionI = 0;
            var minY = 1000;
            var maxY = -1;
            var minX = 1000;
            var maxX = -1;
            var boundaryHit = false;
            var movesCount = 0;

            boundary.Add(startPoint.Y, new List<int> { startPoint.X });

            while (!currentPoint.IsNear(startPoint) || movesCount < 4)
            {
                movesCount++;
                if (directionI < 0)
                {
                    directionI = 3;
                }
                var directionP = Directions[(directionI + 1) % 4];
                var currentDirection = Directions[directionI];

                //Console.WriteLine($"Current point {currentPoint.X}:{currentPoint.Y}; D {directionI}");

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
                    // Console.WriteLine("Boundary Hit");
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
                    //Console.WriteLine("Going perpendicular");
                    directionI += 1;
                    directionI %= 4;
                    currentPoint += Directions[directionI];
                }
                else if (nextValue > Treshold)
                {
                    //Console.WriteLine("Going back");
                    directionI -= 1;
                }
                else
                {
                    //Console.WriteLine("Going straight");
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

        private IEnumerable<PointValue> SavePoints(int y, int fromX, int toX)
        {
            var points = new List<PointValue>();
            for (var x = fromX; x < toX; x++)
            {
                MarkUsed(x, y);
                points.Add(new PointValue
                {
                    Point = new Point(x, y),
                    Value = GetPixelAt(x, y) ?? 0
                });
            }
            return points;
        }

        private BoundaryRowResult ProcessBoundaryRow(int y, IList<int> row)
        {
            var orderedRow = row.OrderBy(r => r);
            var lastX = orderedRow.First();
            var remaining = orderedRow.Skip(1);
            var result = new BoundaryRowResult
            {
                PointValues = new List<PointValue>()
            };
            foreach (var x in remaining)
            {
                if (x == lastX)
                {
                    continue;
                }
                if (x - lastX == 1 || GetPixelAt(lastX + 1, y) > Treshold)
                {
                    result.PointValues.AddRange(SavePoints(y, lastX, x));
                }
                lastX = x;
            }
            return result;
        }

        private byte[] CopyBoundaryValues(TraversePixelResult pixelResult)
        {
            var boundaryWidth = pixelResult.MaxX - pixelResult.MinX + 2 * Padding;
            var boundaryHeight = pixelResult.MaxY - pixelResult.MinY + 2 * Padding;
            var pixels = new byte[boundaryWidth * boundaryHeight];
            foreach (var boundaryRow in pixelResult.Boundary)
            {
                var result = ProcessBoundaryRow(boundaryRow.Key, boundaryRow.Value);
                foreach (var pointValue in result.PointValues)
                {
                    var adjustedY = pointValue.Point.Y - pixelResult.MinY + Padding;
                    var adjustedX = pointValue.Point.X - pixelResult.MinX + Padding;
                    var i = adjustedY * boundaryWidth + adjustedX;
                    if (i < pixels.Length)
                    {
                        pixels[i] = pointValue.Value;
                    }
                }
            }
            return pixels;
        }

        private bool SegmentExists(Segment s1, IEnumerable<Segment> segments)
        {
            if (s1.MaxX - s1.MinX < 6 && s1.MaxY - s1.MinY < 6)
            {
                return true;
            }

            return segments.Any(s => s.MinY == s1.MinY &&
                                     s.MaxY == s1.MaxY &&
                                     s.MinX == s1.MinX &&
                                     s.MaxX == s1.MaxX);
        }

        private Segment ProcessPixel(int x, int y)
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
            Console.WriteLine("Segmentating");
            var segments = new List<Segment>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var segment = ProcessIfNotUsed(x, y);
                    if (segment != null && !SegmentExists(segment, segments))
                    {
                        segments.Add(segment);
                    }
                }
            }
            return segments;
        }

        public byte[] GetMarked()
        {
            return _usedPoints.Select(s => s ? (byte)255 : (byte)0).ToArray();
        }


    }
}
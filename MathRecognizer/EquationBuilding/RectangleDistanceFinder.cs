using System;
using MathRecognizer.Interfaces;
using MathRecognizer.Models;

namespace MathRecognizer.EquationBuilding
{
    public class RectangleDistanceFinder : IRectangleDistanceFinder
    {
        private static double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public double DistanceBetweenSegments(NamedSegment s1, NamedSegment s2)
        {
            var left = s2.MaxX < s1.MinX;
            var right = s1.MaxX < s2.MinX;
            var bottom = s2.MaxY < s1.MinY;
            var top = s1.MaxY < s2.MinY;

            if (top && left)
            {
                return Distance(s1.MinX, s1.MaxY, s2.MaxX, s2.MinY);
            }
            if (left && bottom)
            {
                return Distance(s1.MinX, s1.MinY, s2.MaxX, s2.MaxY);
            }
            if (bottom && right)
            {
                return Distance(s1.MaxX, s1.MinY, s2.MinX, s2.MaxY);
            }
            if (right && top)
            {
                return Distance(s1.MaxX, s1.MaxY, s2.MinX, s2.MinY);
            }

            if (left)
            {
                return s1.MinX - s2.MaxX;
            }

            if (right)
            {
                return s2.MinX - s1.MaxX;
            }

            if (bottom)
            {
                return s1.MinY - s2.MaxY;
            }

            return s2.MinY - s1.MaxY;
        }
    }
}
namespace MathRecongizer.Models
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var otherPoint = obj as Point;
            if (otherPoint == null)
            {
                return false;
            }
            return otherPoint.X == X && otherPoint.Y == Y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public Point Clone()
        {
            return new Point(X, Y);
        }

    }
}
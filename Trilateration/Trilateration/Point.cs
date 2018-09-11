using System;

namespace Trilateration
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Distance { get; }
        public double DistanceSquare => Distance * Distance;

        public Point(double x, double y, double distance = 0)
        {
            X = x;
            Y = y;
            Distance = distance;
        }

        public static Point operator /(Point point, double value) 
            => new Point(point.X / value, point.Y / value);

        public static Point operator *(Point point, double value) 
            => new Point(point.X * value, point.Y * value);

        public static Point operator -(Point a, Point b) 
            => new Point(a.X - b.X, a.Y - b.Y);

        public static Point operator +(Point a, Point b) 
            => new Point(a.X + b.X, a.Y + b.Y);

        public static double operator *(Point a, Point b)
            => a.X * b.X + a.Y * b.Y;
        
        public double DistanceFrom(Point other) 
            => Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y,2));

        public double Dist(Point expected)
            => DistanceFrom(expected);
    }
}
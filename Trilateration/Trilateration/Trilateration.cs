using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Trilateration
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Distance { get; set; }

        public Point(double x, double y, double distance = 0)
        {
            X = x;
            Y = y;
            Distance = distance;
        }
    }


    public class Trilateration
    {
        // 3 4 = 9
        // 5 12 = 13
        // 7 24 = 25

        [Theory]
        [InlineData(3, 4, 5,    5, 12, 13,   7, 24, 25,   0, 0)]
        public void should_pass(
            double x1,
            double y1,
            double dist1,

            double x2,
            double y2,
            double dist2,

            double x3,
            double y3,
            double dist3,

            double exp1,
            double exp2
        )
        {
            var p1 = new Point(x1, y1, dist1);
            var p2 = new Point(x2, y2, dist2);
            var p3 = new Point(x3, y3, dist3);

            var result = Calculate(p1, p2, p3);

            result.Should().BeEquivalentTo(new Point(exp1, exp2));
        }

        private static Point Calculate(Point p1, Point p2, Point p3)
        {
            var points = new List<Point> {p1, p2, p3};
            return Calculate(points);
        }

        private static Point Calculate(List<Point> points)
        {
            double top = 0;
            double bottom = 0;

            for (var i = 0; i < 3; i++)
            {
                var c = points[i];
                Point c2 = null;
                Point c3 = null;
                if (i == 0)
                {
                    c2 = points[1];
                    c3 = points[2];
                }
                else if (i == 1)
                {
                    c2 = points[0];
                    c3 = points[2];
                }
                else
                {
                    c2 = points[0];
                    c3 = points[1];
                }

                var d = c2.X - c3.X;

                var v1 = c.X * c.X + c.Y * c.Y - c.Distance * c.Distance;
                top += d * v1;

                var v2 = c.Y * d;
                bottom += v2;
            }

            var y = top / (2 * bottom);
            var vc1 = points[0];
            var vc2 = points[1];
            top = vc2.Distance * vc2.Distance
                  + vc1.X * vc1.X + vc1.Y * vc1.Y 
                  - vc1.Distance * vc1.Distance 
                  - vc2.X * vc2.X 
                  - vc2.Y * vc2.Y -
                  2 * (vc1.Y - vc2.Y) * y;
            bottom = vc1.X - vc2.X;
            var x = top / (2 * bottom);

            return new Point(x, y, 0);
        }
    }
}
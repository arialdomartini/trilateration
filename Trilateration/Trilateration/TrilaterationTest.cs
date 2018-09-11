﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace Trilateration
{
    public class TrilaterationTest
    {
        [Theory]
        [InlineData(3, 4, 5, 5, 12, 13, 7, 24, 25, 0, 0)]
        [InlineData(1, 0, 1, 0, 1, 1, 1, 1, 1.4142135, 0, 0)]
        [InlineData(13, 14, 5, 15, 22, 13, 17, 34, 25, 10, 10)]
        [InlineData(-3, -4, 5, 5, 12, 13, 7, 24, 25, 0, 0)]
        [InlineData(88, 105, 137, 60, 91, 109, 15, 112, 113, 0, 0)]
        [InlineData(98, 115, 137, 70, 101, 109, 25, 122, 113, 10, 10)]
        [InlineData(88 + 10, 105, 137, 60 + 10, 91, 109, 15 + 10, 112, 113, 10, 0)]
        [InlineData(88 + 10, 105 + 10, 137, 60 + 10, 91 + 10, 109, 15 + 10, 112 + 10, 113, 10, 10)]
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

//            var result = Calculate(p1, p2, p3);
            var result = Trilateration.Calculate2(p1, p2, p3);

            var expected = new Point(exp1, exp2);
            result.Dist(expected).Should().BeLessOrEqualTo(0.1);
//            result.Should().Be(expected);
        }

    }
 
    public class Trilateration
    {
        public static Point Calculate2(Point p1, Point p2, Point p3)
        {
            var ex = new double[2];
            var ey = new double[2];
            var p3p1 = new double[2];

            double jval = 0;
            double ival = 0;
            double p3p1i = 0;

            double t1;
            double t2;
            double t3;

            double t;
            double d;


            double temp = 0;
            {
                t = p2.X - p1.X;
                temp += t * t;
            }
            {
                t = p2.Y - p1.Y;
                temp += t * t;
            }

            d = Math.Sqrt(temp);

            ex[0] = (p2.X - p1.X) / Math.Sqrt(temp);
            ex[1] = (p2.Y - p1.Y) / (Math.Sqrt(temp));
            p3p1[0] = p3.X - p1.X;
            p3p1[1] = p3.Y - p1.Y;

            {
                t1 = ex[0];
                t2 = p3p1[0];
                ival += t1 * t2;
            }
            {
                t1 = ex[1];
                t2 = p3p1[1];
                ival += t1 * t2;
            }

            {
                t = p3.X - p1.X - ex[0] * ival;
                p3p1i += t * t;
            }
            {
                t = p3.Y - p1.Y - ex[1] * ival;
                p3p1i += t * t;
            }
            
            ey[0] = (p3.X - p1.X - ex[0] * ival) / Math.Sqrt(p3p1i);
            ey[1] = (p3.Y - p1.Y - ex[1] * ival) / Math.Sqrt(p3p1i);

            {
                t1 = ey[0];
                t2 = p3p1[0];
                jval += t1 * t2;
            }{
                t1 = ey[1];
                t2 = p3p1[1];
                jval += t1 * t2;
            }

            var xval = (Math.Pow(p1.Distance, 2) - Math.Pow(p2.Distance, 2) + Math.Pow(d, 2)) / (2 * d);
            var yval = ((Math.Pow(p1.Distance, 2) - Math.Pow(p3.Distance, 2) + Math.Pow(ival, 2) + Math.Pow(jval, 2)) /
                           (2 * jval)) - (ival / jval) * xval;

            t1 = p1.X;
            t2 = ex[0] * xval;
            t3 = ey[0] * yval;

            var result = new Point();

            result.X = t1 + t2 + t3;
            t1 = p1.Y;
            t2 = ex[1] * xval;
            t3 = ey[1] * yval;
            result.Y = t1 + t2 + t3;

            return result;
        }


        private static Point Calculate(List<Point> points)
        {
            double top = 0;
            double bottom = 0;

            for (var i = 0; i < 3; i++)
            {
                var c = points[i];
                var c2 = i == 0 ? points[1] : points[0];
                var c3 = i != 2 ? points[1] : points[2];

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
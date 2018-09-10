using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Trilateration
{
    public struct Point
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

        public double Dist(Point other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y,2));
        }
    }


    public class Trilateration
    {
        // 3 4 = 9
        // 5 12 = 13
        // 7 24 = 25

        [Theory]
        [InlineData(3, 4, 5,    5, 12, 13,   7, 24, 25,   0, 0)]
        [InlineData(1, 0, 1,    0, 1, 1,   1, 1, 1.4142135,   0, 0)]
        [InlineData(13, 14, 5,    15, 22, 13,   17, 34, 25,   10, 10)]
        [InlineData(-3, -4, 5,    5, 12, 13,   7, 24, 25,   0, 0)]
        [InlineData(88, 105, 137,    60, 91, 109,   15, 112, 113,   0, 0)]
        
        [InlineData(98, 115, 137,    70, 101, 109,   25, 122, 113,   10, 10)]
        [InlineData(88+10, 105, 137,    60+10, 91, 109,   15+10, 112, 113,   10, 0)]
        [InlineData(88+10, 105+10, 137,    60+10, 91+10, 109,   15+10, 112+10, 113,   10, 10)]
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
            var result = Calculate2(p1, p2, p3);

            var expected = new Point(exp1, exp2);
            result.Dist(expected).Should().BeLessOrEqualTo(0.1);
//            result.Should().Be(expected);
        }

        private static Point Calculate(Point p1, Point p2, Point p3)
        {
            var points = new List<Point> {p1, p2, p3};
            return Calculate(points);
        }


        private static Point Calculate2(
            Point ponto1,
            Point ponto2,
            Point ponto3)
        {

            var distance1 = ponto1.Distance;
            var distance2 = ponto2.Distance;
            var distance3 = ponto3.Distance;
        //DECLARACAO DE VARIAVEIS
        Point retorno = new Point();
        double[] P1   = new double[2];
        double[] P2   = new double[2];
        double[] P3   = new double[2];
        double[] ex   = new double[2];
        double[] ey   = new double[2];
        double[] p3p1 = new double[2];
        double jval  = 0;
        double temp  = 0;
        double ival  = 0;
        double p3p1i = 0;
        double triptx;
        double xval;
        double yval;
        double t1;
        double t2;
        double t3;
        double t;
        double exx;
        double d;
        double eyy;

        //TRANSFORMA OS PONTOS EM VETORES
        //PONTO 1
        P1[0] = ponto1.X;
        P1[1] = ponto1.Y;
        //PONTO 2
        P2[0] = ponto2.X;
        P2[1] = ponto2.Y;
        //PONTO 3
        P3[0] = ponto3.X;
        P3[1] = ponto3.Y;


        for (int i = 0; i < P1.Length; i++) {
            t1   = P2[i];
            t2   = P1[i];
            t    = t1 - t2;
            temp += (t*t);
        }
        d = Math.Sqrt(temp);
        for (int i = 0; i < P1.Length; i++) {
            t1    = P2[i];
            t2    = P1[i];
            exx   = (t1 - t2)/(Math.Sqrt(temp));
            ex[i] = exx;
        }
        for (int i = 0; i < P3.Length; i++) {
            t1      = P3[i];
            t2      = P1[i];
            t3      = t1 - t2;
            p3p1[i] = t3;
        }
        for (int i = 0; i < ex.Length; i++) {
            t1 = ex[i];
            t2 = p3p1[i];
            ival += (t1*t2);
        }
        for (int  i = 0; i < P3.Length; i++) {
            t1 = P3[i];
            t2 = P1[i];
            t3 = ex[i] * ival;
            t  = t1 - t2 -t3;
            p3p1i += (t*t);
        }
        for (int i = 0; i < P3.Length; i++) {
            t1 = P3[i];
            t2 = P1[i];
            t3 = ex[i] * ival;
            eyy = (t1 - t2 - t3)/Math.Sqrt(p3p1i);
            ey[i] = eyy;
        }
        for (int i = 0; i < ey.Length; i++) {
            t1 = ey[i];
            t2 = p3p1[i];
            jval += (t1*t2);
        }
        xval = (Math.Pow(distance1, 2) - Math.Pow(distance2, 2) + Math.Pow(d, 2))/(2*d);
        yval = ((Math.Pow(distance1, 2) - Math.Pow(distance3, 2) + Math.Pow(ival, 2) + Math.Pow(jval, 2))/(2*jval)) - ((ival/jval)*xval);

        t1 = ponto1.X;
        t2 = ex[0] * xval;
        t3 = ey[0] * yval;
        triptx = t1 + t2 + t3;
        retorno.X = triptx;
        t1 = ponto1.Y;
        t2 = ex[1] * xval;
        t3 = ey[1] * yval;
        triptx = t1 + t2 + t3;
        retorno.Y = triptx;

        return retorno;
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
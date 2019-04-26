using System;
using System.Collections.Generic;
using System.Drawing;

namespace Goryokaku
{
    class GoryokakuGenerator
    {
        private const double PAI = 3.141592653589793238462643383279;

        private static readonly double DELTA12 = ToRadian(12);
        private static readonly double DELTA18 = ToRadian(18);
        private static readonly double DELTA24 = ToRadian(24);
        private static readonly double DELTA36 = ToRadian(36);
        private static readonly double DELTA48 = ToRadian(48);
        private static readonly double DELTA72 = ToRadian(72);

        private const double DEFAULT_R = 120;

        public static void Prepare(Goryokaku go, double cx, double cy, double rate, double startTheta, string halfMoon)
        {
            double r = DEFAULT_R;
            double r2 = r + 10;
            double r21 = r2 - 40;
            double rh1 = r + 15;
            double rh2 = r + 6;
            double theta = startTheta;

            r *= rate;
            r2 *= rate;
            r21 *= rate;
            rh1 *= rate;
            rh2 *= rate;

            for (int i = 0; i < 5; i++)
            {
                // ★★★ 外側の五芒星を描くための座標
                int zx1 = (int)(Math.Cos(theta) * r2 + cx);
                int zy1 = (int)(Math.Sin(theta) * r2 + cy);
                int zx2 = (int)(Math.Cos(theta + DELTA36) * r21 + cx);
                int zy2 = (int)(Math.Sin(theta + DELTA36) * r21 + cy);
                int zx3 = (int)(Math.Cos(theta + DELTA72) * r2 + cx);
                int zy3 = (int)(Math.Sin(theta + DELTA72) * r2 + cy);

                go.PointList1.Add(new Point(zx1, zy1));
                go.PointList1.Add(new Point(zx2, zy2));
                go.PointList1.Add(new Point(zx3, zy3));

                // ★★★ 半月堡
                if (halfMoon[i] == '1')
                {
                    List<Point> pointList4a = new List<Point>();
                    List<Point> pointList4b = new List<Point>();
                    double vx1 = zx2 - zx1;
                    double vy1 = zy2 - zy1;
                    double vx2 = zx3 - zx2;
                    double vy2 = zy3 - zy2;

                    double xx1 = zx1 + vx1 * 0.6;
                    double yy1 = zy1 + vy1 * 0.6;
                    double xx2 = Math.Cos(theta + DELTA36) * rh1 + cx;
                    double yy2 = Math.Sin(theta + DELTA36) * rh1 + cy;
                    double xx3 = zx3 - vx2 * 0.6;
                    double yy3 = zy3 - vy2 * 0.6;

                    //Console.WriteLine("{0}, {1} と {2}, {3} の中点 = {4}, {5}", zx1, zy1, zx2, zy2, xx1, yy1);

                    pointList4a.Add(new Point((int)xx1, (int)yy1));
                    pointList4a.Add(new Point((int)xx2, (int)yy2));
                    pointList4a.Add(new Point((int)xx3, (int)yy3));
                    go.PointLists.Add(pointList4a);

                    xx1 = zx1 + vx1 * 0.7;
                    yy1 = zy1 + vy1 * 0.7;
                    xx2 = Math.Cos(theta + DELTA36) * rh2 + cx;
                    yy2 = Math.Sin(theta + DELTA36) * rh2 + cy;
                    xx3 = zx3 - vx2 * 0.7;
                    yy3 = zy3 - vy2 * 0.7;
                    pointList4b.Add(new Point((int)xx1, (int)yy1));
                    pointList4b.Add(new Point((int)xx2, (int)yy2));
                    pointList4b.Add(new Point((int)xx3, (int)yy3));
                    go.PointLists.Add(pointList4b);
                }

                // ★★★ へこみのある五芒星
                MakePointList(go.PointList2, theta, DEFAULT_R, cx, cy, 0.75, true, rate);
                MakePointList(go.PointList3, theta, DEFAULT_R - 10, cx, cy, 0.6, false, rate);

                theta += DELTA72;
            }
        }

        public static void MakePointList(List<Point> pointList, double theta, double r, double cx, double cy, double rate, bool m, double rate2)
        {
            double rm40 = r - 40;
            double rm35 = r - 35;

            r *= rate2;
            rm40 *= rate2;
            rm35 *= rate2;


            // 五芒星を描くための座標
            int x1 = (int)(Math.Cos(theta) * r + cx);
            int y1 = (int)(Math.Sin(theta) * r + cy);
            int x2 = (int)(Math.Cos(theta + DELTA36) * rm40 + cx);
            int y2 = (int)(Math.Sin(theta + DELTA36) * rm40 + cy);
            int x3 = (int)(Math.Cos(theta + DELTA72) * r + cx);
            int y3 = (int)(Math.Sin(theta + DELTA72) * r + cy);

            // 五芒星を少し欠けさせる
            int dx1 = x2 - x1;
            int dy1 = y2 - y1;
            int dx2 = x3 - x2;
            int dy2 = y3 - y2;

            int xx1 = (int)(x1 + dx1 * rate);
            int yy1 = (int)(y1 + dy1 * rate);
            int xx2 = (int)(x3 - dx2 * rate);
            int yy2 = (int)(y3 - dy2 * rate);

            // M字部分両肩
            int mx1, mx2, my1, my2;
            if (m)
            {
                mx1 = (int)(Math.Cos(theta + DELTA24) * rm40 + cx);
                my1 = (int)(Math.Sin(theta + DELTA24) * rm40 + cy);
                mx2 = (int)(Math.Cos(theta + DELTA48) * rm40 + cx);
                my2 = (int)(Math.Sin(theta + DELTA48) * rm40 + cy);
            }
            else
            {
                mx1 = (int)(Math.Cos(theta + DELTA18) * rm35 + cx);
                my1 = (int)(Math.Sin(theta + DELTA18) * rm35 + cy);
                mx2 = (int)(Math.Cos(theta + ToRadian(54)) * rm35 + cx);
                my2 = (int)(Math.Sin(theta + ToRadian(54)) * rm35 + cy);
            }

            pointList.Add(new Point(x1, y1));
            pointList.Add(new Point(xx1, yy1));
            pointList.Add(new Point(mx1, my1));
            if (m)
            {
                // M字部分中心
                int mcx = (int)(Math.Cos(theta + DELTA36) * rm35 + cx);
                int mcy = (int)(Math.Sin(theta + DELTA36) * rm35 + cy);
                pointList.Add(new Point(mcx, mcy));
            }
            pointList.Add(new Point(mx2, my2));
            pointList.Add(new Point(xx2, yy2));
            pointList.Add(new Point(x3, y3));
        }

        private static double ToRadian(int degree)
        {
            return degree * PAI / 180;
        }
    }
}

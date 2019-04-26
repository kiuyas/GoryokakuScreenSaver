using System.Collections.Generic;
using System.Drawing;

namespace Goryokaku
{
    class Goryokaku
    {
        public Pen DrawPen { get; set; }
        public List<Point> PointList1 { set; get; }
        public List<Point> PointList2 { set; get; }
        public List<Point> PointList3 { set; get; }
        public List<List<Point>> PointLists { set; get; }

        public Goryokaku(Pen p)
        {
            DrawPen = p;
            PointList1 = new List<Point>();
            PointList2 = new List<Point>();
            PointList3 = new List<Point>();
            PointLists = new List<List<Point>>();
        }
    }
}

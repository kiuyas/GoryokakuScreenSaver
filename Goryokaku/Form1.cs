using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Goryokaku
{
    public partial class Form1 : Form
    {
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private List<Goryokaku> goryokakuList;

        private int w, h;

        public bool IsPreviewMode { get; set; }

        public Form1()
        {
            InitializeComponent();
            Initialize();    
        }

        public Form1(Rectangle Bounds)
        {
            InitializeComponent();
            Initialize();
            this.Bounds = Bounds;
        }

        public Form1(IntPtr PreviewHandle)
        {
            InitializeComponent();

            SetParent(this.Handle, PreviewHandle);
            SetWindowLong(this.Handle, -16,
                 new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;
            this.Location = new Point(0, 0);
            IsPreviewMode = true;

            Initialize();

            w = this.Size.Width;
            h = this.Size.Height;
        }

        private void Initialize()
        {
            Cursor.Hide();
            this.WindowState = FormWindowState.Maximized;
            w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            goryokakuList = new List<Goryokaku>();

            Pen[] pp = { Pens.Blue, Pens.Red, Pens.Lime, Pens.Magenta, Pens.Cyan, Pens.Yellow, Pens.White};
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                Goryokaku go = new Goryokaku(pp[random.Next(pp.Length)]);
                int cx = random.Next(this.w);
                int cy = random.Next(this.h);
                double rate = random.NextDouble() + 0.2;
                double startTheta = random.NextDouble() * 6.28;
                string halfMoon = "";
                for (int j = 0; j < 5; j++)
                {
                    halfMoon += random.Next(100) > 50 ? "1" : "0";
                }
                GoryokakuGenerator.Prepare(go, cx, cy, rate, startTheta, halfMoon);
                goryokakuList.Add(go);
            }
            Refresh();
            timer1.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (goryokakuList != null)
            {
                foreach (Goryokaku go in goryokakuList)
                {
                    DrawStar(go.PointList1, g, go.DrawPen);
                    DrawStar(go.PointList2, g, go.DrawPen);
                    DrawStar(go.PointList3, g, go.DrawPen);
                    DrawHalfMoon(go.PointLists, g, go.DrawPen);
                }
            }
        }

        private void DrawStar(List<Point> pointList, Graphics g, Pen p)
        {
            if (pointList != null && pointList.Count > 0)
            {
                g.DrawPolygon(p, pointList.ToArray());
            }
        }

        private void DrawHalfMoon(List<List<Point>> pointLists, Graphics g, Pen p)
        {
            foreach (List<Point> list in pointLists)
            {
                g.DrawLines(p, list.ToArray());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public new void Dispose()
        {
            base.Dispose();
            Cursor.Show();
        }

        #region User Input

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //** プレビューを実行していないとき
            if (!IsPreviewMode) // プレビュー画面用に無効にする
            {
                System.Environment.Exit(0);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //** レビューを実行していないとき
            if (!IsPreviewMode) // プレビュー画面用に無効にする
            {
                System.Environment.Exit(0);
            }
        }

        // XとY int.MaxValueのとOriginalLoctionを始める
        // カーソルがその位置にすることは不可能なので。
        // この変数がまだ設定されている場合
        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //** プレビューを行っていない場合は、このif文を取り出す
            if (!IsPreviewMode) // プレビューのとき終了関数を無効にする
            {
                //originallocationが設定されているかどうかを確認
                if (OriginalLocation.X == int.MaxValue &
                    OriginalLocation.Y == int.MaxValue)
                {
                    OriginalLocation = e.Location;
                }
                // マウスが20 pixels 以上動いたかどうかを監視
                // 動いた場合はアプリケーションを終了
                if (Math.Abs(e.X - OriginalLocation.X) > 20 |
                    Math.Abs(e.Y - OriginalLocation.Y) > 20)
                {
                    System.Environment.Exit(0);
                }
            }
        }
        #endregion
    }
}

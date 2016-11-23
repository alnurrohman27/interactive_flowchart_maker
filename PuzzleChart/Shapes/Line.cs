using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Line : PuzzleObject
    {
        private const double EPSILON = 3.0;
        public Point start_point { get; set; }
        public Point end_point { get; set; }

        private Pen pen;

        public Line()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
        }

        public Line(Point startpoint) :
            this()
        {
            this.start_point = startpoint;
        }

        public Line(Point startpoint, Point endpoint) :
            this(startpoint)
        {
            this.end_point = endpoint;
        }
        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.start_point = new Point(this.start_point.X + xAmount, this.start_point.Y + yAmount);
            this.end_point = new Point(this.end_point.X + xAmount, this.end_point.Y + yAmount);
        }

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.start_point, this.end_point);
            }
        }

        public override void RenderOnEditingView()
        {
            RenderOnStaticView();
            pen.Color = Color.Blue;
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.Solid;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.start_point, this.end_point);
            }
        }

        public override void RenderOnPreview()
        {
            this.pen = new Pen(Color.Red);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.DashDotDot;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.start_point, this.end_point);
            }
        }
        public double GetSlope()
        {
            double m = (double)(end_point.Y - start_point.Y) / (double)(end_point.X - start_point.X);
            return m;
        }
        public override bool Intersect(int xTest, int yTest)
        {
            double m = GetSlope();
            double b = end_point.Y - m * end_point.X;
            double y_point = m * xTest + b;

            if (Math.Abs(yTest - y_point) < EPSILON)
            {
                Debug.WriteLine("Object " + ID + " is selected.");
                return true;
            }
            return false;
        }

        public override bool Add(PuzzleObject obj)
        {
            return false;
        }

        public override bool Remove(PuzzleObject obj)
        {
            return false;
        }
    }
}

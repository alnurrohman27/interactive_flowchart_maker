using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Line : StatefulPuzzleObject
    {
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

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.graphics != null)
            {
                this.graphics.SmoothingMode = SmoothingMode.AntiAlias;
                this.graphics.DrawLine(pen, this.start_point, this.end_point);
            }
        }

        public override void RenderOnEditingView()
        {
            RenderOnStaticView();
        }

        public override void RenderOnPreview()
        {
            this.pen = new Pen(Color.Red);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.DashDotDot;

            if (this.graphics != null)
            {
                this.graphics.SmoothingMode = SmoothingMode.AntiAlias;
                this.graphics.DrawLine(pen, this.start_point, this.end_point);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Line : PuzzleObject
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

        public override void Draw()
        {
            this.graphics.DrawLine(this.pen, start_point, end_point);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Oval : PuzzleObject
    {
        
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        private Pen pen;

        public Oval()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
        }

        public Oval(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public Oval(int x, int y, int width, int Height) : this(x, y)
        {
            this.width = width;
            this.height = Height;
        }

        public override void Draw()
        {
            this.graphics.DrawEllipse(pen, x, y, width, height);
        }
    }
}


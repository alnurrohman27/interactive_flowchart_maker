using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Parallelogram : PuzzleObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Point[] my_point_array = new Point[4];

        private Pen pen;

        public Parallelogram()
        {

            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

        }

        public Parallelogram(int x, int y) : this()
        {
            this.x = x;
            this.y = y;

        }

        public Parallelogram(int x, int y, int width, int height) : this(x, y)
        {
            this.width = width;
            this.height = height;


        }

        public override void Draw()
        {
            //my_point_array = { new Point(x + width / 2, y), new Point(x, y + height / 2), new Point(x + width / 2, y + height), new Point(x + width, y + height / 2) };
            my_point_array[0] = new Point(x + width / 3, y);
            my_point_array[1] = new Point(x, y + height);
            my_point_array[2] = new Point(x + width, y + height);
            my_point_array[3] = new Point(x + width + width/3, y  );

            this.graphics.DrawPolygon(pen, my_point_array);
        }
    }
}

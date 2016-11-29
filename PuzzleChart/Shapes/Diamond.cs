using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Diamond : PuzzleObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Point[] my_point_array = new Point[4];

        private Pen pen;
        private Font font;

        public Diamond()
        {

            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

        }

        public Diamond(int x, int y) : this()
        {
            this.x = x;
            this.y = y;

        }

        public Diamond(int x, int y, int width, int height) : this(x, y)
        {
            this.width = width;
            this.height = height;


        }
        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawDiamond(pen, x, y, width, height);

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
                string text = "Decision";
                GetGraphics().DrawString(text, font, Brushes.Black, rectangle, stringFormat);
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

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawDiamond(pen, x, y, width, height);
            }
        }
        public void DrawDiamond(Pen pen, int x, int y, int width, int height)
        {
            //my_point_array = { new Point(x + width / 2, y), new Point(x, y + height / 2), new Point(x + width / 2, y + height), new Point(x + width, y + height / 2) };
            my_point_array[0] = new Point(x + width / 2, y);
            my_point_array[1] = new Point(x, y + height / 2);
            my_point_array[2] = new Point(x + width / 2, y + height);
            my_point_array[3] = new Point(x + width, y + height / 2);

            this.GetGraphics().DrawPolygon(pen, my_point_array);
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.x += xAmount;
            this.y += yAmount;
        }
        private bool pnpoly(int nvert, float testx, float testy)
        {

            int[] vertx = new int[4];
            int[] verty = new int[4];
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                vertx[i] = my_point_array[i].X;
                verty[i] = my_point_array[i].Y;
            }
            bool c = false;
            int j = 0;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((verty[i] > testy) != (verty[j] > testy)) &&
                    (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                    c = !c;
            }
            return c;
        }
        public override bool Intersect(int xTest, int yTest)
        {
            return pnpoly(4, xTest, yTest);
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PuzzleChart.Shapes
{
    public class Diamond : Vertex, IOpenSave
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Point[] my_point_array = new Point[5];

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
            my_point_array[4] = new Point(x + width / 2, y);

            this.GetGraphics().DrawPolygon(pen, my_point_array);
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.x += xAmount;
            this.y += yAmount;

            BroadcastUpdate(xAmount, yAmount);
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

        bool LineIntersectProcess(Point start_line, Point end_line, Point start_shape, Point end_shape, out Point intersection)
        {
            float p0_x = start_line.X, p0_y = start_line.Y, p1_x = end_line.X, p1_y = end_line.Y,
                  p2_x = start_shape.X, p2_y = start_shape.Y, p3_x = end_shape.X, p3_y = end_shape.Y;
            float i_x = 0, i_y = 0;
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected0
                i_x = p0_x + (t * s1_x);
                i_y = p0_y + (t * s1_y);
                intersection = new Point((int)i_x, (int)i_y);
                return true;
            }
            intersection = new Point(0, 0);
            return false; // No collision
        }

        public override Point LineIntersect(Point start_point, Point end_point)
        {
            Point intersection;
            int counter = 0;


            for (counter = 0; counter < 4; counter++)
            {
                if (LineIntersectProcess(start_point, end_point, my_point_array[counter], my_point_array[counter + 1], out intersection))
                    break;
            }

            if (counter == 1)
                return my_point_array[1];
            else if (counter == 2)
                return my_point_array[3];
            else if (counter == 0 || counter == 3)
                return my_point_array[0]; 

            return new Point(0, 0);
        }

        public void serialize(string path, int id)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("\t<Diamond>");
                sw.WriteLine("\t\t<ID>");
                sw.WriteLine("\t\t\t" + id.ToString());
                sw.WriteLine("\t\t</ID>");
                sw.WriteLine("\t\t<X>");
                sw.WriteLine("\t\t\t" + x.ToString());
                sw.WriteLine("\t\t</X>");
                sw.WriteLine("\t\t<Y>");
                sw.WriteLine("\t\t\t" + y.ToString());
                sw.WriteLine("\t\t</Y>");
                sw.WriteLine("\t\t<Height>");
                sw.WriteLine("\t\t\t" + height.ToString());
                sw.WriteLine("\t\t</Height>");
                sw.WriteLine("\t\t<Width>");
                sw.WriteLine("\t\t\t" + width.ToString());
                sw.WriteLine("\t\t</Width>");
                sw.WriteLine("\t</Diamond>");
            }
        }

        public PuzzleObject unserialize(string path)
        {
            throw new NotImplementedException();
        }
    }
}

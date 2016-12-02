using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class Rectangle : Vertex, IOpenSave
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Point[] my_point_array = new Point[5];

        private Pen pen;
        private Font font;

        public Rectangle()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
        }
        public void AddPointArray()
        {
            my_point_array[0] = new Point(x, y);
            my_point_array[1] = new Point(x + width, y);
            my_point_array[2] = new Point(x + width, y + height);
            my_point_array[3] = new Point(x, y + height);
            my_point_array[4] = new Point(x, y);
        }
        public Rectangle(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public Rectangle(int x, int y, int width, int Height) : this(x, y)
        {
            this.width = width;
            this.height = Height;
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.x += xAmount;
            this.y += yAmount;

            BroadcastUpdate(xAmount, yAmount);
        }

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawRectangle(pen, x, y, width, height);

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
                string text = "Process";
                GetGraphics().DrawString(text, font, Brushes.Black, rectangle, stringFormat);
            }
        }

        public override void RenderOnEditingView()
        {
            this.pen.Color = Color.Black;
            this.pen.Color = Color.Blue;
            this.pen.DashStyle = DashStyle.Solid;
            AddPointArray();
            GetGraphics().DrawRectangle(this.pen, x, y, width, height);

            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
            string text = "Process";
            GetGraphics().DrawString(text, font, Brushes.Black, rectangle, stringFormat);
        }

        public override void RenderOnPreview()
        {
            this.pen = new Pen(Color.Red);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.DashDotDot;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawRectangle(pen, x, y, width, height);
            }
        }

        public override bool Intersect(int xTest, int yTest)
        {
            if ((xTest >= x && xTest <= x + width) && (yTest >= y && yTest <= y + height))
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

        bool LineIntersectProcess(Point start_line,Point end_line, Point start_shape, Point end_shape, out Point intersection)
        {
            float p0_x = start_line.X, p0_y = start_line.Y, p1_x = end_line.X,p1_y = end_line.Y,
                  p2_x = start_shape.X, p2_y = start_shape.Y,p3_x = end_shape.X, p3_y = end_shape.Y;
            float i_x =0, i_y=0;
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

            for(int i = 0; i < 4; i++)
            {
                if (LineIntersectProcess(start_point, end_point, my_point_array[i], my_point_array[i + 1], out intersection))
                    return intersection;
            }
            return new Point(0, 0);
        }

        public void serialize(string path, int id)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("\t<Rectangle>");
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
                sw.WriteLine("\t</Rectangle>");
            }
        }

        public PuzzleObject unserialize(string path)
        {
            throw new NotImplementedException();
        }
    }
}

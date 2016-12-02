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

namespace PuzzleChart.Shapes
{
    public class Parallelogram : Vertex, IOpenSave
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Point[] my_point_array = new Point[5];

        private Pen pen;
        private Font font;

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

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawParallelogram(pen, x, y, width, height);

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
                string text = "Data";
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
                this.DrawParallelogram(pen, x, y, width, height);
            }
        }

        public void DrawParallelogram(Pen pen, int x, int y, int width, int height)
        {
            my_point_array[0] = new Point(x + width / 3, y);
            my_point_array[1] = new Point(x, y + height);
            my_point_array[2] = new Point(x + width, y + height);
            my_point_array[3] = new Point(x + width + width/3, y  );
            my_point_array[4] = new Point(x + width / 3, y);

            this.GetGraphics().DrawPolygon(pen, my_point_array);
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.x += xAmount;
            this.y += yAmount;

            BroadcastUpdate(xAmount, yAmount);
        }
        private bool pnpoly(int nvert,float testx, float testy)
        {
 
            int[] vertx = new int[4];
            int[] verty = new int[4];
            int i = 0;
            for ( i = 0; i < 4; i++)
            {
                vertx[i] = my_point_array[i].X;
                verty[i] = my_point_array[i].Y;
            }
            bool c = false;
            int j= 0;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((verty[i] > testy) != (verty[j] > testy)) && (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
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

            for (int i = 0; i < 4; i++)
            {
                if (LineIntersectProcess(start_point, end_point, my_point_array[i], my_point_array[i + 1], out intersection))
                    return intersection;
            }
            return new Point(0, 0);
        }

        public void Serialize(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement xmlFile = doc.Element("puzzle_object");

            List<Edge> listEdges = GetEdges();
            foreach (Edge edgeObj in listEdges)
            {
                Line lineObj = (Line)edgeObj;

                if (lineObj.GetEndPointVertex() != null)
                {
                    xmlFile.Add(new XElement("Parallelogram",
                       new XElement("id", this.ID.ToString()),
                       new XElement("x", x.ToString()),
                       new XElement("y", y.ToString()),
                       new XElement("width", width.ToString()),
                       new XElement("height", height.ToString()),
                       new XElement("start_point", lineObj.start_point.ToString()),
                       new XElement("end_point", lineObj.end_point.ToString()),
                       new XElement("start_vertex", lineObj.GetStartPointVertex().ID.ToString()),
                       new XElement("end_vertex", lineObj.GetEndPointVertex().ID.ToString())
                    ));
                }
                else
                {
                    xmlFile.Add(new XElement("Parallelogram",
                       new XElement("id", this.ID.ToString()),
                       new XElement("x", x.ToString()),
                       new XElement("y", y.ToString()),
                       new XElement("width", width.ToString()),
                       new XElement("height", height.ToString()),
                       new XElement("start_point", lineObj.start_point.ToString()),
                       new XElement("end_point", lineObj.end_point.ToString()),
                       new XElement("start_vertex", lineObj.GetStartPointVertex().ID.ToString())
                    ));
                }
            }
            doc.Save(path);
        }

        public PuzzleObject Unserialize(string path)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public string text { get; set; }
        public SolidBrush fontColor { get; set; }
        public StringFormat stringFormat { get; set; }
        public Point[] my_point_array = new Point[5];
        public Pen pen;
        public Font font;
        public SolidBrush myBrush;
        public DataTable table;

        public Parallelogram()
        {

            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
            myBrush = new SolidBrush(Color.Purple);
            stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
            text = "Data";
            fontColor = new SolidBrush(Color.Black);
            table = new DataTable("Object");
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(int));
            this.translate.Add(this.transMem);

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
                GetGraphics().DrawString(text, font, fontColor, rectangle, stringFormat);
            }
        }

        public override void RenderOnEditingView()
        {
            this.pen.Color = Color.Blue;
            this.pen.DashStyle = DashStyle.Solid;
            pen.Width = 2f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawParallelogram(pen, x, y, width, height);

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
                GetGraphics().DrawString(text, font, fontColor, rectangle, stringFormat);
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
            this.GetGraphics().FillPolygon(myBrush, my_point_array);
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            transMem.xBefore = this.x;
            transMem.yBefore = this.y;
            transMem.xAmount += xAmount;
            transMem.yAmount += yAmount;

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
            XElement xmlFile = doc.Root;

            if (doc.Root.LastNode != null)
            {
                xmlFile = (XElement)doc.LastNode;
            }

            xmlFile.Add(new XElement("parallelogram",
                new XElement("id", this.ID.ToString()),
                new XElement("x", x.ToString()),
                new XElement("y", y.ToString()),
                new XElement("width", width.ToString()),
                new XElement("height", height.ToString())
            ));

            List<Edge> listEdges = GetEdges();
            foreach (Edge edgeObj in listEdges)
            {
                Line lineObj = (Line)edgeObj;

                if (lineObj.GetEndPointVertex() != null)
                {
                    xmlFile.Add(new XElement("edge",
                        new XElement("id", lineObj.ID.ToString()),
                        new XElement("start_point", new XAttribute("x", lineObj.start_point.X.ToString()), new XAttribute("y", lineObj.start_point.Y.ToString())),
                        new XElement("end_point", new XAttribute("x", lineObj.end_point.X.ToString()), new XAttribute("y", lineObj.end_point.Y.ToString())),
                        new XElement("start_vertex", lineObj.GetStartPointVertex().ID.ToString()),
                        new XElement("end_vertex", lineObj.GetEndPointVertex().ID.ToString())
                    ));
                }
                else
                {
                    xmlFile.Add(new XElement("edge",
                        new XElement("id", lineObj.ID.ToString()),
                        new XElement("start_point", new XAttribute("x", lineObj.start_point.X.ToString()), new XAttribute("y", lineObj.start_point.Y.ToString())),
                        new XElement("end_point", new XAttribute("x", lineObj.end_point.X.ToString()), new XAttribute("y", lineObj.end_point.Y.ToString())),
                        new XElement("start_vertex", lineObj.GetStartPointVertex().ID.ToString())
                    ));
                }
            }
            doc.Save(path);
        }

        public List<PuzzleObject> Unserialize(string path)
        {
            List<PuzzleObject> listObj = new List<PuzzleObject>();
            Parallelogram parallelogramObj = null;
            int x = 0, y = 0, width = 0, height = 0, flag = 0;
            bool loopFlag = true;
            string id = null;
            XmlTextReader reader = new XmlTextReader(path);
            reader.MoveToContent();
            try
            {
                if (reader.Name == "puzzle_object")
                {
                    while (reader.Read())
                    {
                        loopFlag = true;
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                if (reader.Name == "parallelogram")
                                {
                                    while (reader.Read() && loopFlag)
                                    {
                                        switch (reader.NodeType)
                                        {

                                            case XmlNodeType.Element: // The node is an element.
                                                if (reader.Name == "id")
                                                    flag = 1;
                                                else if (reader.Name == "x")
                                                    flag = 2;
                                                else if (reader.Name == "y")
                                                    flag = 3;
                                                else if (reader.Name == "width")
                                                    flag = 4;
                                                else if (reader.Name == "height")
                                                    flag = 5;
                                                break;
                                            case XmlNodeType.Text:
                                                if (flag == 1)
                                                {
                                                    id = reader.Value;
                                                }

                                                else if (flag == 2)
                                                {
                                                    x = Int32.Parse(reader.Value);
                                                }

                                                else if (flag == 3)
                                                {
                                                    y = Int32.Parse(reader.Value);
                                                }

                                                else if (flag == 4)
                                                {
                                                    width = Int32.Parse(reader.Value);
                                                }

                                                else if (flag == 5)
                                                {
                                                    height = Int32.Parse(reader.Value);
                                                }
                                                break;
                                            case XmlNodeType.EndElement:
                                                if (reader.Name == "parallelogram")
                                                {
                                                    loopFlag = false;
                                                }
                                                break;
                                        }
                                    }
                                    if (x > 0 && y > 0 && width > 0 && height > 0)
                                    {
                                        Console.WriteLine("Data x: " + x + ", y: " + y + ", width: " + width + ", height: " + height);
                                        parallelogramObj = new Parallelogram(x, y, width, height);
                                        parallelogramObj.ID = new Guid(id);
                                        listObj.Add(parallelogramObj);
                                    }
                                }
                                break;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {

            }
            reader.Close();
            return listObj;
        }

        public override void TranslateUndoRedo(bool undoRedo)
        {
            if (undoRedo)
            {
                if (!transMem.flag)
                {
                    int xAmount = translate[translate_count].xAmount;
                    int yAmount = translate[translate_count].yAmount;

                    //transMem.xBefore = this.x;
                    //transMem.yBefore = this.y;

                    this.x -= xAmount;
                    this.y -= yAmount;

                    translate[translate_count].xAmountRedo = xAmount;
                    translate[translate_count].yAmountRedo = yAmount;
                    translate[translate_count].xAmount -= xAmount;
                    translate[translate_count].yAmount -= yAmount;

                    Debug.WriteLine("xNow: " + this.x + " yNow: " + this.y + " xAmount: " + transMem.xAmount + " yAmount: " + transMem.yAmount);
                    Debug.WriteLine("xAmountRedo: " + transMem.xAmountRedo + " yAmountRedo: " + transMem.yAmountRedo);

                    BroadcastUpdate(-transMem.xAmountRedo, -transMem.yAmountRedo);

                    transMem.flag = true;
                }
            }
            else
            {
                if (transMem.flag)
                {
                    int xAmount = transMem.xAmountRedo;
                    int yAmount = transMem.yAmountRedo;

                    this.x += xAmount;
                    this.y += yAmount;

                    transMem.xAmount = xAmount;
                    transMem.yAmount = yAmount;
                    transMem.xAmountRedo -= xAmount;
                    transMem.yAmountRedo -= yAmount;

                    Debug.WriteLine("xNow: " + this.x + " yNow: " + this.y + " xAmount: " + transMem.xAmount + " yAmount: " + transMem.yAmount);
                    Debug.WriteLine("xAmountRedo: " + transMem.xAmountRedo + " yAmountRedo: " + transMem.yAmountRedo);

                    BroadcastUpdate(transMem.xAmount, transMem.yAmount);
                    transMem.flag = false;
                }
            }

        }
    }
}

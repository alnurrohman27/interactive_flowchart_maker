using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.Linq;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Api.Shapes
{
    public class Oval : Vertex, IOpenSave
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

        public Oval()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);

            myBrush = new SolidBrush(Color.Yellow);
            fontColor = new SolidBrush(Color.Black);

            text = "Start/End";
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

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);

                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawEllipse(pen, x, y, width, height);
                this.GetGraphics().FillEllipse(myBrush, rectangle);

                GetGraphics().DrawString(text, font, fontColor, rectangle, stringFormat);
            }
        }

        public override void RenderOnEditingView()
        {
            this.pen = new Pen(Color.Blue);
            this.pen.DashStyle = DashStyle.Solid;
            pen.Width = 2f;

            if (this.GetGraphics() != null)
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);

                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawEllipse(pen, x, y, width, height);
                this.GetGraphics().FillEllipse(myBrush, rectangle);

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
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);

                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawEllipse(pen, x, y, width, height);
                this.GetGraphics().FillEllipse(myBrush, rectangle);
            }
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.x += xAmount;
            this.y += yAmount;

            BroadcastUpdate(xAmount, yAmount);
        }

        public override void Untranslate(int x, int y, int xAmount, int yAmount)
        {
            this.x -= xAmount;
            this.y -= yAmount;

            BroadcastUpdate(-xAmount, -yAmount);
        }

        public bool Contains(Point location)
        {
            Point center = new Point(x + width/2, y + height/2);

            double x_radius = width / 2;
            double y_radius = height / 2;


            if (x_radius <= 0.0 || y_radius <= 0.0)
                return false;
            /* This is a more general form of the circle equation
             *
             * X^2/a^2 + Y^2/b^2 <= 1
             */

            Point normalized = new Point(location.X - center.X,
                                         location.Y - center.Y);

            return ((double)(normalized.X * normalized.X) / (x_radius * x_radius)) + ((double)(normalized.Y * normalized.Y) / (y_radius * y_radius)) <= 1.0;
        }
        public override bool Intersect(int xTest, int yTest)
        {
            return Contains(new Point(xTest, yTest));
        }

        public override bool Add(PuzzleObject obj)
        {
            return false;
        }

        public override bool Remove(PuzzleObject obj)
        {
            return false;
        }

        public override Point LineIntersect(Point start_point, Point end_point)
        {
            throw new NotImplementedException();
        }

        public void Serialize(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement xmlFile = doc.Root;

            if (doc.Root.LastNode != null)
            {
                xmlFile = (XElement)doc.LastNode;
            }

            xmlFile.Add(new XElement("oval",
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
            Oval ovalObj = null;
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
                                if (reader.Name == "oval")
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
                                                if (reader.Name == "oval")
                                                {
                                                    loopFlag = false;
                                                }
                                                break;
                                        }


                                    }
                                    if (x > 0 && y > 0 && width > 0 && height > 0)
                                    {
                                        Console.WriteLine("Data x: " + x + ", y: " + y + ", width: " + width + ", height: " + height);
                                        ovalObj = new Oval(x, y, width, height);
                                        ovalObj.ID = new Guid(id);
                                        listObj.Add(ovalObj);
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

    }
}


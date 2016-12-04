using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace PuzzleChart.Shapes
{
    public class Line : Edge, IOpenSave
    {
        private const double EPSILON = 3.0;
        public Point start_point { get; set; }
        public Point end_point { get; set; }
        public Guid id_start_point_vertex { get; set; }
        public Guid id_end_point_vertex { get; set; }

        private Pen pen;
        private Vertex start_point_vertex;
        private Vertex end_point_vertex;

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
        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.start_point = new Point(this.start_point.X + xAmount, this.start_point.Y + yAmount);
            this.end_point = new Point(this.end_point.X + xAmount, this.end_point.Y + yAmount);
        }

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.start_point, this.end_point);
            }
        }

        public override void RenderOnEditingView()
        {
            pen.Color = Color.Blue;
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.Solid;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.start_point, this.end_point);
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
                this.GetGraphics().DrawLine(pen, this.start_point, this.end_point);
            }
        }
        public double GetSlope()
        {
            double m = (double)(end_point.Y - start_point.Y) / (double)(end_point.X - start_point.X);
            return m;
        }
        public override bool Intersect(int xTest, int yTest)
        {
            double m = GetSlope();
            double b = end_point.Y - m * end_point.X;
            double y_point = m * xTest + b;

            if (Math.Abs(yTest - y_point) < EPSILON)
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

        public void AddVertex(Vertex vertex,bool start_or_end)
        {
            if (start_or_end)
            {
                start_point_vertex = vertex;
                id_start_point_vertex = vertex.ID;
            }
            else
            {
                end_point_vertex = vertex;
                id_end_point_vertex = vertex.ID;
            }
        }
       

        public override void Update(IObservable vertex,int deltaX, int deltaY)
        {
            if(vertex == start_point_vertex)
                start_point = new Point(this.start_point.X + deltaX, this.start_point.Y + deltaY);
            else if (vertex == end_point_vertex)
                end_point = new Point(this.end_point.X + deltaX, this.end_point.Y + deltaY); 
        }

        public override Point LineIntersect(Point start_point, Point end_point)
        {
            throw new NotImplementedException();
        }

        public void Serialize(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement xmlFile = doc.Element("puzzle_object");

            if (start_point_vertex == null)
            {
                xmlFile.Add(new XElement("line", 
                    new XElement("id", this.ID.ToString()),
                    new XElement("start_point", new XAttribute("x", start_point.X.ToString()), new XAttribute("y", start_point.Y.ToString())),
                    new XElement("end_point", new XAttribute("x", end_point.X.ToString()), new XAttribute("y", end_point.Y.ToString()))
                ));
            }
            doc.Save(path);

        }

        public List<PuzzleObject> Unserialize(string path)
        {
            List<PuzzleObject> listObj = new List<PuzzleObject>();
            Line lineObj = null;  
            Point startPoint = new Point(), endPoint = new Point();
            string id = null;
            bool loopFlag = true;
            int flag = 0;
            XmlTextReader reader = new XmlTextReader(path);
            reader.MoveToContent();
            try
            {
                if (reader.Name == "puzzle_object")
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "line")
                        {
                            loopFlag = true;
                            while (reader.Read() && loopFlag)
                            {
                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element: // The node is an element.
                                        if (reader.Name == "id")
                                            flag = 1;
                                        else if (reader.Name == "start_point")
                                        {
                                            flag = 2;
                                            int x = 0, y = 0;

                                            reader.MoveToAttribute(0);
                                            x = Int32.Parse(reader.Value);
                                            reader.MoveToNextAttribute();
                                            y = Int32.Parse(reader.Value);

                                            reader.MoveToElement();
                                            startPoint = new Point(x, y);
                                        }
                                        else if (reader.Name == "end_point")
                                        {
                                            flag = 3;
                                            int x = 0, y = 0;

                                            reader.MoveToAttribute(0);
                                            x = Int32.Parse(reader.Value);
                                            reader.MoveToNextAttribute();
                                            y = Int32.Parse(reader.Value);

                                            reader.MoveToElement();
                                            endPoint = new Point(x, y);
                                        }
                                        break;
                                    case XmlNodeType.Text:
                                        if (flag == 1)
                                            id = reader.Value;
                                        break;
                                    case XmlNodeType.EndElement:
                                        if (reader.Name == "line")
                                            loopFlag = false;
                                        break;
                                }
                            }
                            if (id != null)
                            {
                                Console.WriteLine("Edge ID: " + id + " startPoint: " + startPoint.ToString() + ", endPoint: " + endPoint.ToString());
                                lineObj = new Line(startPoint, endPoint);
                                lineObj.ID = new Guid(id);
                                listObj.Add(lineObj);
                            }
                        }
                        else if (reader.Name == "edge")
                        {
                            loopFlag = true;
                            //Debug.WriteLine("Masuk Elemen " + reader.Name);
                            string startVertex = null, endVertex = null;
                            while (reader.Read() && loopFlag)
                            {
                                reader.MoveToContent();
                                //Debug.WriteLine("Masuk Elemen " + reader.Name);
                                if (reader.Name == "id")
                                {
                                    bool tempFlag = true;
                                    while (reader.Read() && tempFlag)
                                    {
                                        switch (reader.NodeType)
                                        {
                                            case XmlNodeType.Element: // The node is an element.
                                                //Debug.WriteLine("Masuk Elemen " + reader.Name);
                                                break;
                                            case XmlNodeType.Text:
                                                id = reader.Value;
                                                //Debug.WriteLine("Value: " + reader.Value);
                                                break;
                                            case XmlNodeType.EndElement:
                                                tempFlag = false;
                                                break;
                                        }
                                    }
                                }
                                else if (reader.Name == "start_point")
                                {
                                    int x = 0, y = 0;

                                    reader.MoveToFirstAttribute();
                                    x = Int32.Parse(reader.Value);
                                    reader.MoveToNextAttribute();
                                    y = Int32.Parse(reader.Value);

                                    reader.MoveToElement();
                                    startPoint = new Point(x, y);

                                    //Debug.WriteLine("Value: " + startPoint);
                                }
                                else if (reader.Name == "end_point")
                                {
                                    int x = 0, y = 0;

                                    reader.MoveToFirstAttribute();
                                    x = Int32.Parse(reader.Value);
                                    reader.MoveToNextAttribute();
                                    y = Int32.Parse(reader.Value);

                                    reader.MoveToElement();
                                    endPoint = new Point(x, y);

                                    //Debug.WriteLine("Value: " + endPoint);
                                }
                                else if (reader.Name == "start_vertex")
                                {
                                    bool tempFlag = true;
                                    while (reader.Read() && tempFlag)
                                    {
                                        switch (reader.NodeType)
                                        {
                                            case XmlNodeType.Element: // The node is an element.
                                                //Debug.WriteLine("Masuk Elemen " + reader.Name);
                                                break;
                                            case XmlNodeType.Text:
                                                startVertex = reader.Value;
                                                //Debug.WriteLine("Value: " + reader.Value);
                                                break;
                                            case XmlNodeType.EndElement:
                                                tempFlag = false;
                                                break;
                                        }
                                    }
                                }
                                else if (reader.Name == "end_vertex")
                                {
                                    bool tempFlag = true;
                                    while (reader.Read() && tempFlag)
                                    {
                                        switch (reader.NodeType)
                                        {
                                            case XmlNodeType.Element: // The node is an element.
                                                //Debug.WriteLine("Masuk Elemen " + reader.Name);
                                                break;
                                            case XmlNodeType.Text:
                                                endVertex = reader.Value;
                                                //Debug.WriteLine("Value: " + reader.Value);
                                                break;
                                            case XmlNodeType.EndElement:
                                                tempFlag = false;
                                                break;
                                        }
                                    }
                                }
                                else if (reader.Name == "edge")
                                    loopFlag = false;
                                
                            }
                            if (id != null)
                            {
                                lineObj = new Line(startPoint, endPoint);
                                lineObj.ID = new Guid(id);
                                lineObj.id_start_point_vertex = new Guid(startVertex);
                                lineObj.id_end_point_vertex = new Guid(endVertex);
                                listObj.Add(lineObj);
                                Console.WriteLine("Edge ID: " + id + ", startPoint: " + startPoint.ToString() + ", endPoint: " + endPoint.ToString() + ", startVertex: " + lineObj.id_start_point_vertex + ", endVertex: " + lineObj.id_end_point_vertex);
                            }
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Message: " + ex);
            }
            reader.Close();

            

            return listObj;
        }

        public Vertex GetStartPointVertex()
        {
            return this.start_point_vertex;
        }

        public Vertex GetEndPointVertex()
        {
            return this.end_point_vertex;
        }
    }
}

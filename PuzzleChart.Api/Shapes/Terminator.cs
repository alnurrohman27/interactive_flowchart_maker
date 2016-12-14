using PuzzleChart.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace PuzzleChart.Api.Shapes
{
    public class Terminator : Vertex, IOpenSave
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

        public Terminator()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
            font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
            text = "Start/End";

            stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            myBrush = new SolidBrush(Color.Green);
            fontColor = new SolidBrush(Color.Black);
        }

        public Terminator(int x, int y) : this()
        {
            this.x = x;
            this.y = y;

        }

        public Terminator(int x, int y, int width, int height) : this(x, y)
        {
            this.width = width;
            this.height = height;
        }

        public void DrawTerminator(Pen pen)
        {
            my_point_array[0] = new Point(x, y);
            my_point_array[1] = new Point(x + width, y);
            my_point_array[2] = new Point(x + width, y + height);
            my_point_array[3] = new Point(x, y + height);
            my_point_array[4] = new Point(x, y);

            GraphicsPath path = new GraphicsPath();
            int diameter = (int)Math.Sqrt((x * width) + (y * height));
            Size size = new Size(diameter, diameter);
            System.Drawing.Rectangle arc = new System.Drawing.Rectangle(my_point_array[4], size);
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);

            if (diameter == 0)
            {
                path.AddRectangle(rectangle);
                this.GetGraphics().DrawPath(pen, path);
                this.GetGraphics().FillPath(myBrush, path);
            }
            else if(diameter > 0)
            {
                // top left arc  
                path.AddArc(arc, 180, 90);

                // top right arc  
                arc.X = rectangle.Right - diameter;
                path.AddArc(arc, 270, 90);

                // bottom right arc  
                arc.Y = rectangle.Bottom - diameter;
                path.AddArc(arc, 0, 90);

                // bottom left arc 
                arc.X = rectangle.Left;
                path.AddArc(arc, 90, 90);

                path.CloseFigure();
                this.GetGraphics().DrawPath(pen, path);
                this.GetGraphics().FillPath(myBrush, path);
            }
        }

        public override bool Add(PuzzleObject obj)
        {
            return false;
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

        public override Point LineIntersect(Point start_point, Point end_point)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(PuzzleObject obj)
        {
            return false;
        }

        public override void RenderOnEditingView()
        {
            this.pen.Color = Color.Blue;
            this.pen.DashStyle = DashStyle.Solid;
            pen.Width = 2f;
            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawTerminator(pen);

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
                this.DrawTerminator(pen);
            }
        }

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawTerminator(pen);

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
                GetGraphics().DrawString(text, font, fontColor, rectangle, stringFormat);
            }
        }

        public void Serialize(string path)
        {
            throw new NotImplementedException();
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            throw new NotImplementedException();
        }

        public List<PuzzleObject> Unserialize(string path)
        {
            throw new NotImplementedException();
        }

        public override void Untranslate(int x, int y, int xAmount, int yAmount)
        {
            throw new NotImplementedException();
        }
    }
}

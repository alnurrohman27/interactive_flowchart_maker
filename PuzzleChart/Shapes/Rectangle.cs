using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Rectangle : Vertex
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

        private Point LineIntersectProcess(double A1,double B1,double C1, Point start_point,Point end_point)
        {
            double x1 = start_point.X,
                y1 = start_point.Y,
                x2 = end_point.X,
                y2 = end_point.Y;

            double A2 = end_point.Y - start_point.Y,
                B2 = start_point.X - end_point.X,
                C2 = A2 * start_point.X + B2 * start_point.Y;

            double det = A1 * B2 - A2 * B1;
            if (det == 0)
            {
                return new Point(0,0);
            }
            else
            {
                double x = (B2 * C1 - B1 * C2) / det;
                double y = (A1 * C2 - A2 * C1) / det;
                return new Point((int)x, (int)y);
            }
        }

        public override Point LineIntersect(Point start_point, Point end_point)
        {
            double A = end_point.Y - start_point.Y,
                B = start_point.X - end_point.X,
                C = A * start_point.X + B * start_point.Y;
            Point intersection;
            for (int counter = 0;counter < 4; counter++)
            {
                intersection = LineIntersectProcess(A, B, C, my_point_array[counter], my_point_array[counter + 1]);
                if(intersection.X != 0 && intersection.Y!= 0)
                {
                    return intersection;
                }
            }
            return new Point(0, 0);
        }
    }
}

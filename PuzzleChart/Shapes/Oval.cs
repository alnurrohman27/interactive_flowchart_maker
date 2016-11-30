using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Oval : Vertex
    {
        
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        private Pen pen;
        private Font font;

        public Oval()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
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
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawEllipse(pen, x, y, width, height);

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
                string text = "Start/End";
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
                this.GetGraphics().DrawEllipse(pen, x, y, width, height);
            }
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.x += xAmount;
            this.y += yAmount;


            BroadcastUpdate(xAmount, yAmount);
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
    }
}


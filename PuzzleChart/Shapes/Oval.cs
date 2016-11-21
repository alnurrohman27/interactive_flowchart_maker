using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    public class Oval : PuzzleObject
    {
        
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        private Pen pen;

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
            throw new NotImplementedException();
        }

        public override bool Intersect(int xTest, int yTest)
        {
            throw new NotImplementedException();
        }
    }
}


using System.Windows.Forms;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Tools
{
    public class RectangleTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private Rectangle rectangle;

        public Cursor cursor
        {
            get
            {
                return Cursors.Arrow;
            }
        }

        public ICanvas target_canvas
        {
            get
            {
                return this.canvas;
            }

            set
            {
                this.canvas = value;
            }
        }

        public RectangleTool()
        {
            this.Name = "Rectangle tool";
            this.ToolTipText = "Rectangle tool";
            //this.Image = IconSet.bounding_box;
            //Author: Agung 108
            //Class: Linetool
            //Date : 10/31/2016
            //Image still null
            this.Image = IconSet.rectangle;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rectangle = new Rectangle(e.X, e.Y);
                rectangle.width = 0;
                rectangle.height = 0;
                canvas.AddPuzzleObject(rectangle);
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.rectangle != null)
                {
                    int width = e.X - this.rectangle.x;
                    int height = e.Y - this.rectangle.y;

                    if (width > 0 && height > 0)
                    {
                        this.rectangle.width = width;
                        this.rectangle.height = height;
                    }
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rectangle.width = e.X - this.rectangle.x;
                rectangle.height = e.Y - this.rectangle.y;

                rectangle.Select();
            }
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseDownAndKeys(object sender, MouseEventArgs e)
        {

        }
    }
}

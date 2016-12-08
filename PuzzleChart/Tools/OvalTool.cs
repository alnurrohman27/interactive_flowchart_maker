using System.Linq;
using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Shapes;

namespace PuzzleChart.Tools
{
    public class OvalTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private Oval oval;

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

        public OvalTool()
        {
            this.Name = "Oval tool";
            this.ToolTipText = "Oval tool";
            //this.Image = IconSet.bounding_box;
            //Author: Agung 108
            //Class: Linetool
            //Date : 10/31/2016
            //Image still null
            this.Image = IconSet.oval;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                oval = new Oval(e.X, e.Y);
                oval.width = 0;
                oval.height = 0;
                canvas.AddPuzzleObject(oval);
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.oval != null)
                {
                    int width = e.X - this.oval.x;
                    int height = e.Y - this.oval.y;

                    if (width > 0 && height > 0)
                    {
                        this.oval.width = width;
                        this.oval.height = height;
                    }
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                oval.width = e.X - this.oval.x;
                oval.height = e.Y - this.oval.y;
                oval.Select();
                //oval.Deselect();
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

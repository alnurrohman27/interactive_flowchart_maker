using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Shapes;

namespace PuzzleChart.Tools
{
    public class ParallelogramTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private Parallelogram parallelogram;

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

        public ParallelogramTool()
        {
            this.Name = "Parallelogram tool";
            this.ToolTipText = "Parallelogram tool";
            //this.Image = IconSet.bounding_box;
            //Author: Agung 108
            //Class: Linetool
            //Date : 10/31/2016
            //Image still null
            this.Image = IconSet.jajargenjang;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                parallelogram = new Parallelogram(e.X, e.Y);
                parallelogram.width = 0;
                parallelogram.height = 0;
                canvas.AddPuzzleObject(parallelogram);
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.parallelogram != null)
                {
                    int width = e.X - this.parallelogram.x;
                    int height = e.Y - this.parallelogram.y;

                    if (width > 0 && height > 0)
                    {
                        this.parallelogram.width = width;
                        this.parallelogram.height = height;
                    }
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                parallelogram.width = e.X - this.parallelogram.x;
                parallelogram.height = e.Y - this.parallelogram.y;
                parallelogram.Select();
                //parallelogram.Deselect();
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

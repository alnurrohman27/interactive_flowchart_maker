using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PuzzleChart.Shapes;

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
            this.Image = null;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.parallelogram = new Parallelogram(e.X, e.Y);
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
                this.canvas.AddPuzzleObject(this.parallelogram);
            }
        }
    }
}

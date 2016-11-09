using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PuzzleChart.Shapes;

namespace PuzzleChart.Tools
{
    public class DiamondTool : ToolStripButton, ITool 
    {
        private ICanvas canvas;
        private Diamond diamond;

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

        public DiamondTool()
        {
            this.Name = "Diamond tool";
            this.ToolTipText = "Diamond tool";
            //this.Image = IconSet.bounding_box;
            //Author: Agung 108
            //Class: Linetool
            //Date : 10/31/2016
            //Image still null
            this.Image = IconSet.Decision;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.diamond = new Diamond(e.X, e.Y);
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.diamond != null)
                {
                    int width = e.X - this.diamond.x;
                    int height = e.Y - this.diamond.y;

                    if (width > 0 && height > 0)
                    {
                        this.diamond.width = width;
                        this.diamond.height = height;
                    }
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.canvas.AddPuzzleObject(this.diamond);
            }
        }
    }
}

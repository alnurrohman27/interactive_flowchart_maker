using System.Windows.Forms;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Api.Interfaces;

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
            this.Image = IconSet.diamond;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                diamond = new Diamond(e.X, e.Y);
                diamond.width = 0;
                diamond.height = 0;
                canvas.AddPuzzleObject(diamond);
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
                diamond.width = e.X - this.diamond.x;
                diamond.height = e.Y - this.diamond.y;
                diamond.Select();
                //diamond.Deselect();
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

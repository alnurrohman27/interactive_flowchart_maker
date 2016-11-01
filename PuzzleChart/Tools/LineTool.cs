using PuzzleChart.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.Tools
{
    public class LineTool : ToolStripButton,ITool
    {
        private ICanvas canvas;
        private Line line_segment;

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

        public LineTool()
        {
            this.Name = "Line tool";
            this.ToolTipText = "Line tool";
            //Author: Agung 108
            //Class: Linetool
            //Date : 10/31/2016
            //Image still null
            this.Image = null; 
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            line_segment = new Line(new System.Drawing.Point(e.X, e.Y));
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            //to be implemented
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            line_segment.end_point = new System.Drawing.Point(e.X, e.Y);
            canvas.AddPuzzleObject(line_segment);
        }
    }
}

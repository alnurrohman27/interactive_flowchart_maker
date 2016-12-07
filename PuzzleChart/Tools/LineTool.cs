using PuzzleChart.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Vertex start_object, end_object;
  
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

            //Author: Reza 140
            //Class: Linetool
            //Date : 11/9/2016
            //Add Icom line
            this.Image = IconSet.line;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                start_object = null;
                end_object = null;
                line_segment = new Line(new System.Drawing.Point(e.X, e.Y));
                line_segment.end_point = new System.Drawing.Point(e.X, e.Y);
                canvas.AddPuzzleObject(line_segment);
                if (canvas.GetObjectAt(e.X,e.Y) is Vertex && canvas.GetObjectAt(e.X, e.Y) != null && !(canvas.GetObjectAt(e.X,e.Y) is Line))
                {
                    start_object = (Vertex)canvas.GetObjectAt(e.X, e.Y);
                }
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.line_segment != null)
                {
                    line_segment.end_point = new System.Drawing.Point(e.X, e.Y);
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {

            if (this.line_segment != null)
            {
                if (e.Button == MouseButtons.Left)
                {   
                    line_segment.end_point = new Point(e.X, e.Y);
                    line_segment.Select();

                    if (canvas.GetObjectAt(e.X, e.Y) != null && !(canvas.GetObjectAt(e.X,e.Y) is Line)) 
                    {
                        end_object = (Vertex)canvas.GetObjectAt(e.X, e.Y);
                    }
                    if (start_object != null)
                    {
                        start_object.Subscribe(line_segment);
                        line_segment.AddVertex(start_object, true);
                        line_segment.start_point = start_object.LineIntersect(line_segment.start_point, line_segment.end_point);

                    }
                    if (end_object != null && end_object != start_object)
                    {
                        end_object.Subscribe(line_segment);
                        line_segment.AddVertex(end_object, false);
                        line_segment.end_point = end_object.LineIntersect(line_segment.start_point, line_segment.end_point);

                    }

                }

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

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
        private Vertex startingObject, endingObject;
  
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
                line_segment = new Line(new System.Drawing.Point(e.X, e.Y));
                line_segment.end_point = new System.Drawing.Point(e.X, e.Y);
                canvas.AddPuzzleObject(line_segment);
                if (canvas.GetObjectAt(e.X,e.Y) is Vertex)
                {
                    startingObject = (Vertex)canvas.GetObjectAt(e.X, e.Y);
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
                    Point P = new Point();
                    line_segment.end_point = new Point(e.X, e.Y);
                    line_segment.Select();
                    if (canvas.GetObjectAt(e.X, e.Y) is Vertex)
                    {
                        endingObject = (Vertex)canvas.GetObjectAt(e.X, e.Y);
                    }
                    if (startingObject != null)
                    {
                        //P = startingObject.GetIntersectionPoint(line_segment.start_point, line_segment.end_point);
                        startingObject.Subscribe(line_segment);
                        line_segment.AddVertex(startingObject);
                        //line_segment.start_point = new Point(P.X, P.Y);
                    }
                    if (endingObject != null)
                    {
                        //P = endingObject.GetIntersectionPoint(line_segment.start_point, line_segment.end_point);
                        endingObject.Subscribe(line_segment);
                        line_segment.AddVertex(endingObject);
                        //line_segment.end_point = new Point(P.X, P.Y);
                    }
                }
 /*               else if (e.Button == MouseButtons.Right)
                {
                    canvas.RemoveDrawingObject(this.line_segment);
                }
   */         

            }
        }
    }
}

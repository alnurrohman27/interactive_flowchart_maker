using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.Tools
{
    public class SelectionTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private PuzzleObject selected_object;
        private int xInitial;
        private int yInitial;

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

        public SelectionTool()
        {
            this.Name = "Selection tool";
            this.ToolTipText = "Selection tool";
            this.Image = null;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            this.xInitial = e.X;
            this.yInitial = e.Y;

            if (e.Button == MouseButtons.Left && canvas != null)
            {
                canvas.DeselectAllObjects();
                canvas.SelectObjectAt(e.X, e.Y);
                selected_object = canvas.SelectObjectAt(e.X, e.Y);
            }

        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && canvas != null)
            {
                if (selected_object != null)
                {
                    int xAmount = e.X - xInitial;
                    int yAmount = e.Y - yInitial;
                    xInitial = e.X;
                    yInitial = e.Y;

                    selected_object.Translate(e.X, e.Y, xAmount, yAmount);
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Text text = new Text();
            //text.Value = "Untitled";
            //selectedObject.Add(text);
            //Debug.WriteLine("selection tool double click");
        }

        public void ToolKeyUp(object sender, KeyEventArgs e)
        {

        }

        public void ToolKeyDown(object sender, KeyEventArgs e)
        {

        }

        public void ToolHotKeysDown(object sender, Keys e)
        {

        }
    }
}

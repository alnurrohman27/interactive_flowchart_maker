using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.Tools
{
    public class TerminatorTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private Terminator terminator;

        public Cursor cursor
        {
            get
            {
                return this.cursor;
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

        public TerminatorTool()
        {
            this.Name = "Terminator Tool";
            this.ToolTipText = "Terminator Tool";
            this.Image = IconSet.start_end;
            this.CheckOnClick = true;
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                terminator = new Terminator(e.X, e.Y);
                terminator.width = 0;
                terminator.height = 0;
                InsertCommand cmd = new InsertCommand(canvas, terminator);
                cmd.Execute();
            }
        }

        public void ToolMouseDownAndKeys(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.terminator != null)
                {
                    int width = e.X - this.terminator.x;
                    int height = e.Y - this.terminator.y;

                    if (width > 0 && height > 0)
                    {
                        this.terminator.width = width;
                        this.terminator.height = height;
                    }
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                terminator.width = e.X - this.terminator.x;
                terminator.height = e.Y - this.terminator.y;
                terminator.Select();
            }
        }
    }
}

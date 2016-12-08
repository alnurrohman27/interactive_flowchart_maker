using System;
using System.Windows.Forms;
using PuzzleChart.Api;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Forms;
using System.Drawing;
using System.Diagnostics;

namespace PuzzleChart.SelectionTool
{
    public class SelectionTool : ToolStripButton, ITool, IPlugin
    {
        private ICanvas canvas;
        private PuzzleObject selected_object;
        private int xInitial;
        private int yInitial;
        private FormTextDialog textBox;

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

        public IPluginHost Host
        {
            get
            {
                return this.Host;
            }

            set
            {
                this.Host = value;
            }
        }

        public SelectionTool()
        {
            this.Name = "Selection tool";
            this.ToolTipText = "Selection tool";
            this.Image = IconSet.arrow;
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

                if (selected_object != null)
                {
                    selected_object.transMem = new TranslateMemory();
                    selected_object.translate.Add(selected_object.transMem);
                    selected_object.translate_count++;
                }

            }
            else if (e.Button == MouseButtons.Left && canvas != null && Control.ModifierKeys == Keys.Control)
            {
                canvas.SelectObjectAt(e.X, e.Y);
                selected_object = canvas.SelectObjectAt(e.X, e.Y);

                if (selected_object != null)
                {
                    selected_object.transMem = new TranslateMemory();
                    selected_object.translate.Add(selected_object.transMem);
                    selected_object.translate_count++;
                }
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
            try
            {
                if (selected_object != null)
                {
                    PuzzleObject obj = selected_object;
                    if (obj is Line == false)
                    {
                        Control control = new Control();
                        Graphics newGraph = control.CreateGraphics();

                        textBox = new FormTextDialog(obj);
                        textBox.Name = "Text Box";
                        textBox.Width = 320;
                        textBox.Height = 350;
                        if (obj is Diamond)
                            textBox.Width = 400;
                        else if (obj is Oval)
                        {
                            textBox.Width = 200;
                            textBox.Height = 150;
                        }
                        else
                            textBox.Width = 320;
                        textBox.ShowDialog();

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Message: " + ex);
            }
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

        public void ToolMouseDownAndKeys(object sender, MouseEventArgs e)
        {

        }
    }
}

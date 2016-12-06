using PuzzleChart.Form;
using PuzzleChart.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PuzzleChart.Tools
{
    public class TextBoxTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
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

        public TextBoxTool()
        {
            this.Name = "Text Box Tool";
            this.ToolTipText = "Text Box Tool";
            this.Image = IconSet.text_box;
            this.CheckOnClick = true;
        }

        public void ShowTextBoxDialog(List<PuzzleObject> listObj)
        {
            try
            {
                foreach(PuzzleObject obj in listObj)
                {
                    if(obj is Line == false)
                    {
                        Control control = new Control();
                        Graphics newGraph = control.CreateGraphics();

                        textBox = new FormTextDialog(obj);
                        textBox.Name = "Text Box";
                        textBox.Width = 320;
                        if(obj is Diamond)
                            textBox.Width = 400;
                        else
                            textBox.Width = 320;
                        textBox.Height = 350;
                        textBox.ShowDialog();
                        
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error Message: " + ex);
            }
            
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}

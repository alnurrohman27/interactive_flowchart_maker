using PuzzleChart.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.Tools
{
    public class FillColorTool : ToolStripButton, IColor
    {
        private ICanvas canvas;
        private ColorDialog colorDialog;

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

        public ColorDialog ColorDialog
        {
            get
            {
                return this.ColorDialog;
            }

            set
            {
                this.colorDialog = value;
            }
        }

        public FillColorTool()
        {
            this.Name = "Fill Color Tool";
            this.ToolTipText = "Fill Color Tool";
            this.Image = IconSet.fill_color;
            this.CheckOnClick = true;
            colorDialog = new ColorDialog();
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

        public void ShowColorBox(List<PuzzleObject> listObj)
        {
            colorDialog.AllowFullOpen = false;
            colorDialog.AnyColor = true;
            colorDialog.SolidColorOnly = false;
            colorDialog.Color = Color.White;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                foreach(PuzzleObject obj in listObj)
                {
                    Control control = new Control();
                    Graphics newGraph = control.CreateGraphics();
                    if(obj is Diamond)
                    {
                        Diamond obj2 = (Diamond)obj;
                        obj2.myBrush = new SolidBrush(colorDialog.Color);
                        if(obj2.GetGraphics() != null)
                        {
                            newGraph.DrawPolygon(obj2.pen, obj2.my_point_array);
                            newGraph.FillPolygon(obj2.myBrush, obj2.my_point_array);
                            obj2.SetGraphics(newGraph);
                            obj2.Draw();
                        }

                    }
                    else if (obj is Parallelogram)
                    {
                        Parallelogram obj2 = (Parallelogram)obj;
                        obj2.myBrush = new SolidBrush(colorDialog.Color);
                        if (obj2.GetGraphics() != null)
                        {
                            newGraph.DrawPolygon(obj2.pen, obj2.my_point_array);
                            newGraph.FillPolygon(obj2.myBrush, obj2.my_point_array);
                            obj2.SetGraphics(newGraph);
                            obj2.Draw();
                        }
                    }
                    else if (obj is Shapes.Rectangle)
                    {
                        Shapes.Rectangle obj2 = (Shapes.Rectangle)obj;
                        obj2.myBrush = new SolidBrush(colorDialog.Color);
                        if (obj2.GetGraphics() != null)
                        {
                            newGraph.DrawPolygon(obj2.pen, obj2.my_point_array);
                            newGraph.FillPolygon(obj2.myBrush, obj2.my_point_array);
                            obj2.SetGraphics(newGraph);
                            obj2.Draw();
                        }
                    }

                }
            }
        }
    }
}

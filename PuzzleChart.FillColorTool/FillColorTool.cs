using PuzzleChart.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.FillColorTool
{
    public class FillColorTool : ToolStripButton, ITool, IPlugin
    {
        private ICanvas canvas;
        private ColorDialog colorDialog;
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
            this.xInitial = e.X;
            this.yInitial = e.Y;

            if (e.Button == MouseButtons.Left && canvas != null)
            {
                canvas.DeselectAllObjects();
                canvas.SelectObjectAt(e.X, e.Y);
                selected_object = canvas.SelectObjectAt(e.X, e.Y);

                colorDialog.AllowFullOpen = false;
                colorDialog.AnyColor = true;
                colorDialog.SolidColorOnly = false;
                colorDialog.Color = Color.White;

                if (selected_object != null)
                {
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        Control control = new Control();
                        Graphics newGraph = control.CreateGraphics();
                        if (selected_object is Diamond)
                        {
                            Diamond obj2 = (Diamond)selected_object;
                            obj2.myBrush = new SolidBrush(colorDialog.Color);
                            if (obj2.GetGraphics() != null)
                            {
                                newGraph.DrawPolygon(obj2.pen, obj2.my_point_array);
                                newGraph.FillPolygon(obj2.myBrush, obj2.my_point_array);
                                obj2.SetGraphics(newGraph);
                                obj2.Draw();
                            }

                        }
                        else if (selected_object is Parallelogram)
                        {
                            Parallelogram obj2 = (Parallelogram)selected_object;
                            obj2.myBrush = new SolidBrush(colorDialog.Color);
                            if (obj2.GetGraphics() != null)
                            {
                                newGraph.DrawPolygon(obj2.pen, obj2.my_point_array);
                                newGraph.FillPolygon(obj2.myBrush, obj2.my_point_array);
                                obj2.SetGraphics(newGraph);
                                obj2.Draw();
                            }
                        }
                        else if (selected_object is Shapes.Rectangle)
                        {
                            Shapes.Rectangle obj2 = (Shapes.Rectangle)selected_object;
                            obj2.myBrush = new SolidBrush(colorDialog.Color);
                            if (obj2.GetGraphics() != null)
                            {
                                newGraph.DrawPolygon(obj2.pen, obj2.my_point_array);
                                newGraph.FillPolygon(obj2.myBrush, obj2.my_point_array);
                                obj2.SetGraphics(newGraph);
                                obj2.Draw();
                            }
                        }

                        else if (selected_object is Oval)
                        {
                            Oval obj2 = (Oval)selected_object;
                            obj2.myBrush = new SolidBrush(colorDialog.Color);
                            if (obj2.GetGraphics() != null)
                            {
                                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(obj2.x, obj2.y, obj2.width, obj2.height);

                                newGraph.DrawEllipse(obj2.pen, obj2.x, obj2.y, obj2.width, obj2.height);
                                newGraph.FillEllipse(obj2.myBrush, rectangle);
                                obj2.SetGraphics(newGraph);
                                obj2.Draw();
                            }
                        }
                    }
                }
            }
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

        public void ToolMouseDownAndKeys(object sender, MouseEventArgs e)
        {

        }
    }
}

﻿using System;
using System.Windows.Forms;
using PuzzleChart.Api;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Forms;
using PuzzleChart.Commands;
using System.Drawing;
using System.Diagnostics;
using PuzzleChart.Api.State;

namespace PuzzleChart.Tools
{
    public class SelectionTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private PuzzleObject selected_object;
        private int xInitial, xTranslation;
        private int yInitial, yTranslation;
        private FormTextDialog textBox;

        public Cursor cursor
        {
            get
            {
                return Cursors.Hand;
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
            Cursor.Current = cursor;
            this.xInitial = e.X;
            this.yInitial = e.Y;
            xTranslation = 0;
            yTranslation = 0;

            if (e.Button == MouseButtons.Left && canvas != null)
            {
                canvas.DeselectAllObjects();
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

                    xTranslation += xAmount;
                    yTranslation += yAmount;

                    foreach (PuzzleObject obj in canvas.GetAllObjects())
                    {
                        if (obj.State is EditState)
                            obj.Translate(xInitial, yInitial, xAmount, yAmount);
                    }
                    

                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if(this.selected_object != null)
            {
                if(xTranslation != 0 && yTranslation != 0)
                {
                    Debug.WriteLine("Translation Done");
                    TranslationCommand translateCmd = new TranslationCommand(this.canvas, this.selected_object, this.xInitial, this.yInitial, this.xTranslation, this.yTranslation);
                    translateCmd.Execute();
                }
            }
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
            if (e.Button == MouseButtons.Left && canvas != null && Control.ModifierKeys == Keys.Control)
            {
                canvas.SelectObjectAt(e.X, e.Y);
                
                selected_object = canvas.SelectObjectAt(e.X, e.Y);
            }
        }
    }
}

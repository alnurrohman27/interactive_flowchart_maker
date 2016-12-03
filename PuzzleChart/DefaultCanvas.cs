﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using PuzzleChart.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PuzzleChart
{
    public class DefaultCanvas : Control,ICanvas
    {
        private ITool activeTool;
        private List<PuzzleObject> puzzle_objects;
        private List<PuzzleObject> memory_stack;
        private PuzzleObject temp;

        public DefaultCanvas()
        {
            this.puzzle_objects = new List<PuzzleObject>();
            this.memory_stack = new List<PuzzleObject>();
            this.DoubleBuffered = true;

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            this.Paint += DefaultCanvas_Paint;
            this.MouseDown += DefaultCanvas_MouseDown;
            this.MouseUp += DefaultCanvas_MouseUp;
            this.MouseMove += DefaultCanvas_MouseMove;
        }


        public void RemovePuzzleObject(PuzzleObject puzzle_object)
        {
            this.puzzle_objects.Remove(puzzle_object);
        }
        public ITool GetActiveTool()
        {
            return this.activeTool;
        }
        private void DefaultCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseMove(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseUp(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseDown(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_Paint(object sender, PaintEventArgs e)
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                obj.SetGraphics(e.Graphics);
                obj.Draw();
            }
        }

        public void Repaint()
        {
            this.Invalidate();
            this.Update();
        }

        public void SetActiveTool(ITool tool)
        {
            this.activeTool = tool;
        }

        public void SetBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        public void AddPuzzleObject(PuzzleObject puzzle_object)
        {
            this.puzzle_objects.Add(puzzle_object);
        }

        public void Undo()
        {
            var last = puzzle_objects.Count - 1;
            if(last >= 0)
            {
                this.temp = puzzle_objects[puzzle_objects.Count - 1];
                puzzle_objects.RemoveAt(puzzle_objects.Count - 1);
                memory_stack.Add(temp);
                Debug.WriteLine("Undo is selected");
                this.Repaint();
            }        
           
        }

        public void Redo()
        {
            var last = memory_stack.Count - 1;
            if(last >= 0)
            {
                this.temp = memory_stack[memory_stack.Count - 1];
                memory_stack.RemoveAt(memory_stack.Count - 1);
                puzzle_objects.Add(temp);
                Debug.WriteLine("Redo is selected");
                this.Repaint();
            }
        }

        public PuzzleObject GetObjectAt(int x, int y)
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                if (obj.Intersect(x, y))
                {
                    return obj;
                }
            }
            return null;
        }

        public PuzzleObject SelectObjectAt(int x, int y)
        {
            PuzzleObject obj = GetObjectAt(x, y);
            if (obj != null)
            {
                obj.Select();
            }

            return obj;
        }

        public void DeselectAllObjects()
        {
            foreach (PuzzleObject drawObj in puzzle_objects)
            {
                drawObj.Deselect();
            }
        }

        public void Save()
        {
            Debug.WriteLine("Save File is selected");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Interactive Puzzle Document (*.ipd)|*.ipd";
            saveFileDialog1.Title = "Save an Document";
            saveFileDialog1.ShowDialog();

            try
            {
                if (saveFileDialog1.FileName != "")
                {
                    string name = saveFileDialog1.FileName;

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    XmlWriter writer = XmlWriter.Create(name, settings);
                    writer.WriteStartDocument();
                    writer.WriteStartElement("puzzle_object");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                    
                    foreach (PuzzleObject obj in puzzle_objects)
                    {
                        if (obj is IOpenSave)
                        {
                            if (obj is Diamond)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Diamond");
                                Diamond diamondObj = (Diamond)obj;
                                diamondObj.Serialize(name);
                            }
                            else if (obj is Shapes.Rectangle)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Rectangle");
                                Shapes.Rectangle rectangleObj = (Shapes.Rectangle)obj;
                                rectangleObj.Serialize(name);
                            }
                            else if (obj is Oval)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Oval");
                                Oval ovalObj = (Oval)obj;
                                ovalObj.Serialize(name);
                            }
                            else if (obj is Parallelogram)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Parallelogram");
                                Parallelogram parallelogramObj = (Parallelogram)obj;
                                parallelogramObj.Serialize(name);
                            }
                            else if (obj is Line)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Line");
                                Line lineObj = (Line)obj;
                                lineObj.Serialize(name);
                            }

                        }
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: File being used by another process or corrupt");
            }
            

        }

        public void Open()
        {
            Debug.WriteLine("Open File is selected");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Interactive Puzzle Document (*.ipd)|*.ipd";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.puzzle_objects.Clear();
                    List<PuzzleObject> listObj = new List<PuzzleObject>();

                    Diamond diamondObj = new Diamond();
                    listObj = diamondObj.Unserialize(openFileDialog1.FileName);
                    foreach(PuzzleObject obj in listObj)
                    {
                        puzzle_objects.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Oval ovalObj = new Oval();
                    listObj = ovalObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        puzzle_objects.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Parallelogram parallelogramObj = new Parallelogram();
                    listObj = parallelogramObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        puzzle_objects.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Shapes.Rectangle rectangleObj = new Shapes.Rectangle();
                    listObj = rectangleObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        puzzle_objects.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Line lineObj = new Line();
                    listObj = lineObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        puzzle_objects.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    this.Repaint();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk");
                }
            }
        }
    }
}

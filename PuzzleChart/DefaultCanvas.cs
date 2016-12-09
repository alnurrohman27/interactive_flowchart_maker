using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using PuzzleChart.Api;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Api.State;
using System.IO;

namespace PuzzleChart
{
    public class DefaultCanvas : Control, ICanvas
    {
        private ITool activeTool;
        private List<PuzzleObject> puzzle_objects;
        private Stack<ICommand> undoStack;
        private Stack<ICommand> redoStack;
        private List<PuzzleObject> listCopiedItems;
        public bool saved;

        public bool KeyPreview { get; private set; }

        public DefaultCanvas()
        {
            this.puzzle_objects = new List<PuzzleObject>();
            this.listCopiedItems = new List<PuzzleObject>();

            this.redoStack = new Stack<ICommand>();
            this.undoStack = new Stack<ICommand>(); 

            this.DoubleBuffered = true;
            this.saved = false;

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            this.Paint += DefaultCanvas_Paint;
            this.MouseDown += DefaultCanvas_MouseDown;
            this.MouseUp += DefaultCanvas_MouseUp;
            this.MouseMove += DefaultCanvas_MouseMove;
            this.MouseDoubleClick += DefaultCanvas_MouseDoubleClick;
            this.KeyDown += DefaultCanvas_KeyDown;

        }

        public ICommand PopUndoStack()
        {
            if (undoStack.Count > 0)
            {
                ICommand command = undoStack.Pop();
                redoStack.Push(command);
                return command;
            }
            else
                return null;

        }

        public void PushUndoStack(ICommand command)
        {
            undoStack.Push(command);
        }

        public ICommand PopRedoStack()
        {
            if (redoStack.Count > 0)
            {
                ICommand command = redoStack.Pop();
                undoStack.Push(command);
                return command;
            }
            else
                return null;
        }

        public void PushRedoStack(ICommand command)
        {
            redoStack.Push(command);
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
                if(Control.ModifierKeys == Keys.None)
                    this.activeTool.ToolMouseDown(sender, e);
                else if(Control.ModifierKeys == Keys.Control)
                    this.activeTool.ToolMouseDownAndKeys(sender, e);
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

        private void DefaultCanvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.activeTool.ToolMouseDoubleClick(sender, e);
        }

        private void DefaultCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.A)
            {
                SelectAllObj();
            }
        }

        private void SelectAllObj ()
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                if(obj.State is StaticState)
                {
                    PuzzleState editState = new EditState();
                    obj.ChangeState(editState);
                    Repaint();
                }
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

        public List<PuzzleObject> GetAllObjects()
        {
            return this.puzzle_objects;
        }

        public void DeselectAllObjects()
        {
            foreach (PuzzleObject drawObj in puzzle_objects)
            {
                drawObj.Deselect();
            }
        }

        public List<PuzzleObject> GetSelectedObjects()
        {
            List<PuzzleObject> listObj = new List<PuzzleObject>();
            foreach(PuzzleObject obj in this.puzzle_objects)
            {
                if (obj.State is EditState)
                    listObj.Add(obj);
            }
            return listObj;
        }

        public void SetCopiedItems(List<PuzzleObject> listCopiedItems)
        {
            this.listCopiedItems = listCopiedItems;
        }

        public List<PuzzleObject> GetCopiedItems()
        {
            return this.listCopiedItems;
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
                            else if (obj is Api.Shapes.Rectangle)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Rectangle");
                                Api.Shapes.Rectangle rectangleObj = (Api.Shapes.Rectangle)obj;
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
                    this.saved = true;
                    this.Name = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: File being used by another process or corrupt");
                Debug.WriteLine("Error Message: " + ex);
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

                    Api.Shapes.Rectangle rectangleObj = new Api.Shapes.Rectangle();
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

                    for (int i = 0; i < listObj.Count; i++)
                    {
                        Line temp1 = (Line)listObj[i];
                        for (int j = 1; j < listObj.Count; j++)
                        {
                            Line temp2 = (Line)listObj[j];
                            if (temp1.ID == temp2.ID && i != j)
                            {
                                listObj.RemoveAt(j);
                            }
                        }
                    }

                    foreach (Line obj in listObj)
                    {
                        Line tempObj = new Line(obj.start_point, obj.end_point);
                        tempObj.ID = obj.ID;

                        foreach (PuzzleObject obj2 in puzzle_objects)
                        {
                            Vertex allObj;
                            if (obj2 is Vertex)
                            {
                                allObj = (Vertex)obj2;
                                if (allObj.ID == obj.id_start_point_vertex)
                                {
                                    tempObj.AddVertex(allObj, true);
                                }
                                if (allObj.ID == obj.id_end_point_vertex)
                                {
                                    tempObj.AddVertex(allObj, false);
                                }
                                allObj.Subscribe(tempObj);
                            }
                        }
                        puzzle_objects.Add(tempObj);
                        tempObj.Select();
                        tempObj.Deselect();
                    }
                    listObj.Clear();
                    this.saved = true;
                    this.Name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    this.Repaint();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: File being used by another process or corrupt");
                    Debug.WriteLine("Error Message: " + ex);
                }
            }
        }

        public void PasteObject()
        {
            
        }


    }
}


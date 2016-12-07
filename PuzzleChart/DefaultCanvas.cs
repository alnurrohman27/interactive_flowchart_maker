using System;
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
using PuzzleChart.State;
using PuzzleChart.Tools;

namespace PuzzleChart
{
    public class DefaultCanvas : Control,ICanvas
    {
        private ITool activeTool;
        private List<PuzzleObject> puzzle_objects;
        private List<PuzzleObject> memory_stack;
        private List<PuzzleObject> memory_delete;
        private List<PuzzleObject> memory_copy;
        private int countDelete, countRecover;
        private PuzzleObject temp;

        public bool KeyPreview { get; private set; }

        public DefaultCanvas()
        {
            this.puzzle_objects = new List<PuzzleObject>();
            this.memory_stack = new List<PuzzleObject>();
            this.memory_delete = new List<PuzzleObject>();
            this.memory_copy = new List<PuzzleObject>();
            this.countDelete = 0;
            this.countRecover = 0;
            this.DoubleBuffered = true;

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            this.Paint += DefaultCanvas_Paint;
            this.MouseDown += DefaultCanvas_MouseDown;
            this.MouseUp += DefaultCanvas_MouseUp;
            this.MouseMove += DefaultCanvas_MouseMove;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(DefaultCanvas_KeyDown);
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
                this.countRecover = 0;
                this.countDelete = 0;
                this.memory_delete = new List<PuzzleObject>();
                this.memory_stack = new List<PuzzleObject>();
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
            if(this.activeTool is FillColorTool)
            {
                FillColorTool fillColorTool = (FillColorTool)activeTool;
                List<PuzzleObject> listObj = new List<PuzzleObject>();
                foreach(PuzzleObject obj in puzzle_objects)
                {
                    if (obj.State is EditState)
                        listObj.Add(obj);
                }
                fillColorTool.ShowColorBox(listObj);
            }
            else if(this.activeTool is FontColorTool)
            {
                FontColorTool fontColorTool = (FontColorTool)activeTool;
                List<PuzzleObject> listObj = new List<PuzzleObject>();
                foreach (PuzzleObject obj in puzzle_objects)
                {
                    if (obj.State is EditState)
                        listObj.Add(obj);
                }
                fontColorTool.ShowColorBox(listObj);
            }
            else if(this.activeTool is TextBoxTool)
            {
                TextBoxTool textBoxTool = (TextBoxTool)activeTool;
                List<PuzzleObject> listObj = new List<PuzzleObject>();
                foreach (PuzzleObject obj in puzzle_objects)
                {
                    if (obj.State is EditState)
                        listObj.Add(obj);
                }
                textBoxTool.ShowTextBoxDialog(listObj);
            }
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
            if(last >= 0 && this.countDelete == 0 && this.countRecover == 0)
            {
                bool flagEdit = false;
                foreach (PuzzleObject obj in puzzle_objects)
                {
                    if (obj.State is EditState)
                    {
                        this.temp = obj;
                        flagEdit = true;
                    }
                }

                if (!flagEdit)
                    {
                    this.temp = puzzle_objects[puzzle_objects.Count - 1];
                }

                if (temp.transMem != null && temp.transMem.flag == false && temp is Oval == false && temp is Line == false)
                {
                    temp.TranslateUndoRedo(true);
                    memory_stack.Add(temp);
                }
                else
                {
                    puzzle_objects.RemoveAt(puzzle_objects.Count - 1);
                    memory_stack.Add(temp);
                }
                Debug.WriteLine("Undo is selected");
                this.Repaint();
            }   
            else if(this.countDelete > 0)
            {
                Debug.WriteLine("Undo is selected");
                var i = this.memory_delete.Count - 1;
                if (memory_delete[i] is Line)
                {
                    Line tempLine = (Line)memory_delete[i];
                    foreach(PuzzleObject obj in puzzle_objects)
                    {
                        Vertex tempVertex;
                        if (tempLine.id_start_point_vertex == obj.ID || tempLine.id_end_point_vertex == obj.ID)
                        {
                            tempVertex = (Vertex)obj;
                            tempVertex.Subscribe(tempLine);
                            if (tempLine.id_start_point_vertex == tempVertex.ID)
                                tempLine.AddVertex(tempVertex, true);
                            else
                                tempLine.AddVertex(tempVertex, false);
                        }
                    }
                }

                puzzle_objects.Add(memory_delete[i]);
                memory_delete.RemoveAt(i);
                countDelete--;
                countRecover++;
                Repaint();
            }
        }

        public void Redo()
        {
            var last = memory_stack.Count - 1;
            
            if (last >= 0 && this.countDelete == 0 && this.countRecover == 0)
            {
                this.temp = memory_stack[memory_stack.Count - 1];

                if (temp.transMem != null && temp is Oval == false && temp is Line == false)
                {
                    temp.TranslateUndoRedo(false);
                    memory_stack.Remove(temp);
                }
                else
                {
                    memory_stack.RemoveAt(memory_stack.Count - 1);
                    puzzle_objects.Add(temp);
                }
                Debug.WriteLine("Redo is selected");
                this.Repaint();
            }
            else if (this.countRecover > 0)
            {
                Debug.WriteLine("Redo is selected");
                var i = this.puzzle_objects.Count - 1;
                if (puzzle_objects[i] is Line)
                {
                    Line tempLine = (Line)puzzle_objects[i];
                    foreach (PuzzleObject obj in puzzle_objects)
                    {
                        Vertex tempVertex;
                        if (tempLine.id_start_point_vertex == obj.ID || tempLine.id_end_point_vertex == obj.ID)
                        {
                            tempVertex = (Vertex)obj;
                            tempVertex.Subscribe(tempLine);
                            if (tempLine.id_start_point_vertex == tempVertex.ID)
                                tempLine.AddVertex(tempVertex, true);
                            else
                                tempLine.AddVertex(tempVertex, false);
                        }
                    }
                }
                this.memory_delete.Add(this.puzzle_objects[i]);
                this.puzzle_objects.RemoveAt(i);
                this.countDelete++;
                this.countRecover--;
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

                    this.Repaint();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: File being used by another process or corrupt");
                    Debug.WriteLine("Error Message: " + ex);
                }
            }
        }

        public void DeleteObject()
        {
            List<PuzzleObject> tempObj = new List<PuzzleObject>();
            foreach (PuzzleObject obj in puzzle_objects)
            {
                if (obj.State is EditState)
                {
                    if(obj is Line)
                    {
                        Line tempLine = (Line)obj;
                        foreach (PuzzleObject obj2 in puzzle_objects)
                        {
                            Vertex tempVertex;
                            if(obj2 is Line == false && tempLine.id_start_point_vertex == obj2.ID || obj2 is Line == false && tempLine.id_end_point_vertex == obj2.ID)
                            {
                                tempVertex = (Vertex)obj2;
                                tempVertex.Unsubscribe(tempLine);
                                if (obj2.ID == tempLine.id_start_point_vertex)
                                    tempLine.RemoveVertex(true);
                                else
                                    tempLine.RemoveVertex(false);
                            }
                        }
                    }
                    tempObj.Add(obj);
                }
            }

            foreach (PuzzleObject obj in tempObj)
            {
                Console.WriteLine("Delete object: " + obj.ID);
                this.countDelete++;
                this.memory_delete.Add(obj);
                this.RemovePuzzleObject(obj);
            }
            this.Repaint();
        }

        public void CopyObject()
        {
            List<PuzzleObject> tempObj = new List<PuzzleObject>();
            memory_copy.Clear();
            bool flagCopy = false;
            foreach (PuzzleObject obj in puzzle_objects)
            {
                if (obj.State is EditState)
                {
                    tempObj.Add(obj);
                    flagCopy = true;
                }
            }
            if(flagCopy)
                this.memory_copy.Clear();
            foreach (PuzzleObject obj in tempObj)
            {
                Console.WriteLine("Copy object to clipboard: " + obj.ID);
                this.memory_copy.Add(obj);
            }
        }

        public void PasteObject()
        {
            List<Line> listTempLine = new List<Line>();
            StaticState staticState = new StaticState();
            List<CopyMemory> listCopyMem = new List<CopyMemory>();

            foreach (PuzzleObject obj in memory_copy)
            {
                Console.WriteLine("Copy object to canvas: " + obj.ID);
                CopyMemory copyMemory = new CopyMemory();
                if(obj is Line)
                {
                    Line temp = (Line)obj;
                    Line drawObj = new Line(temp.start_point, temp.end_point);
                    if (temp.id_start_point_vertex != null)
                        drawObj.id_start_point_vertex = temp.id_start_point_vertex;
                    if(temp.id_end_point_vertex != null)
                        drawObj.id_end_point_vertex = temp.id_end_point_vertex;
                    listTempLine.Add(drawObj);
                }
                else if (obj is Diamond)
                {
                    Diamond temp = (Diamond)obj;
                    Diamond drawObj = new Diamond(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Diamond");
                    listCopyMem.Add(copyMemory);
                    puzzle_objects.Add(drawObj);
                }
                else if (obj is Oval)
                {
                    Oval temp = (Oval)obj;
                    Oval drawObj = new Oval(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Oval");
                    listCopyMem.Add(copyMemory);
                    this.puzzle_objects.Add(drawObj);
                }
                else if(obj is Parallelogram)
                {
                    Parallelogram temp = (Parallelogram)obj;
                    Parallelogram drawObj = new Parallelogram(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Parallelogram");
                    listCopyMem.Add(copyMemory);
                    this.puzzle_objects.Add(drawObj);
                }
                else if (obj is Shapes.Rectangle)
                {
                    Shapes.Rectangle temp = (Shapes.Rectangle)obj;
                    Shapes.Rectangle drawObj = new Shapes.Rectangle(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Rectangle");
                    listCopyMem.Add(copyMemory);
                    this.puzzle_objects.Add(drawObj);
                }
                obj.ChangeState(staticState);
            }
            foreach (Line obj in listTempLine)
            {
                Line drawObj = new Line(obj.start_point, obj.end_point);
                foreach (PuzzleObject obj3 in puzzle_objects)
                {
                    foreach(CopyMemory copyMem in listCopyMem)
                    {
                        Vertex obj2 = null;
                        if (obj3.ID == copyMem.ID)
                        {
                            obj2 = (Vertex)obj3;
                            obj2.ID = copyMem.ID;
                            if (obj.id_start_point_vertex == copyMem.before_copied && obj3 is Vertex)
                            {
                                drawObj.AddVertex(obj2, true);
                                drawObj.id_start_point_vertex = obj2.ID;
                                Debug.WriteLine("Edge ID: " + copyMem.before_copied + " Vertex: " + obj2.ID);
                            }
                            if (obj.id_end_point_vertex == copyMem.before_copied && obj3 is Vertex)
                            {
                                drawObj.AddVertex(obj2, false);
                                drawObj.id_end_point_vertex = obj2.ID;
                                Debug.WriteLine("Edge ID: " + copyMem.before_copied + " Vertex: " + obj2.ID);
                            }
                            if (obj2 != null)
                                obj2.Subscribe(drawObj);
                        }
                    }
                    
                }
                puzzle_objects.Add(drawObj);
            }
            listTempLine.Clear();
            Debug.WriteLine("Count: " + puzzle_objects.Count);
            this.Repaint();
        }

        // Hot keys handler
        void DefaultCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)       // Ctrl-Z Undo
            {
                Undo();
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }

            if (e.Control && e.KeyCode == Keys.Y)       // Ctrl-Y Redo
            {
                Redo();
                e.SuppressKeyPress = true;  
            }

            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                Save();
                e.SuppressKeyPress = true;  
            }

            if (e.Control && e.KeyCode == Keys.O)       // Ctrl-O Open
            {
                Open();
                e.SuppressKeyPress = true;
            }
        }
    }
}


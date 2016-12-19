using System;
using PuzzleChart.Api.Interfaces;
using System.Diagnostics;
using System.Windows.Forms;
using PuzzleChart.Api;
using PuzzleChart.Api.Shapes;
using System.Collections.Generic;
using System.IO;

namespace PuzzleChart.Commands
{
    public class OpenCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;
        public OpenCommand(ICanvas canvas, IEditor editor)
        {
            this.canvas = canvas;
            this.editor = editor;
        }

        public void Execute()
        {
            Debug.WriteLine("Open File is selected");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Interactive Puzzle Document (*.ipd)|*.ipd";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<PuzzleObject> listPuzzleObject = canvas.GetAllObjects();
                    listPuzzleObject.Clear();
                    List<PuzzleObject> listObj = new List<PuzzleObject>();

                    Diamond diamondObj = new Diamond();
                    listObj = diamondObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        listPuzzleObject.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Oval ovalObj = new Oval();
                    listObj = ovalObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        listPuzzleObject.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Parallelogram parallelogramObj = new Parallelogram();
                    listObj = parallelogramObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        listPuzzleObject.Add(obj);
                        obj.Select();
                        obj.Deselect();
                    }
                    listObj.Clear();

                    Api.Shapes.Rectangle rectangleObj = new Api.Shapes.Rectangle();
                    listObj = rectangleObj.Unserialize(openFileDialog1.FileName);
                    foreach (PuzzleObject obj in listObj)
                    {
                        listPuzzleObject.Add(obj);
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

                        foreach (PuzzleObject obj2 in listPuzzleObject)
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
                        listPuzzleObject.Add(tempObj);
                        tempObj.Select();
                        tempObj.Deselect();
                    }
                    listObj.Clear();
                    canvas.Saved = true;
                    canvas.Name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    canvas.Repaint();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: File being used by another process or corrupt");
                    Debug.WriteLine("Error Message: " + ex);
                }
            }

            DefaultEditor defEditor = (DefaultEditor)editor;
            defEditor.SelectedTab.Text = this.canvas.Name;
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }

}

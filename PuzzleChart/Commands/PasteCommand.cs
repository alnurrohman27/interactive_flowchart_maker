using System;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Shapes;
using System.Collections.Generic;
using PuzzleChart.Api;
using PuzzleChart.Api.State;
using System.Diagnostics;

namespace PuzzleChart.Commands
{
    public class PasteCommand : ICommand
    {
        private ICanvas canvas;
        private StaticState staticState;
        private List<Vertex> vertexMemory;
        private List<CopyMemory> listCopyMemory;
        private List<Line> listTempLine;
        private List<Line> listNewLine;

        public PasteCommand(ICanvas canvas)
        {
            this.canvas = canvas;
            this.staticState = new StaticState();
            this.vertexMemory = new List<Vertex>();
            this.listCopyMemory = new List<CopyMemory>();
            this.listTempLine = new List<Line>();
            this.listNewLine = new List<Line>();
        }
        public void Execute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            List<PuzzleObject> memoryCopy = defCanvas.GetCopyItem();
            foreach (PuzzleObject obj in memoryCopy)
            {
                Console.WriteLine("Paste object(s) to canvas: " + obj.ID);
                CopyMemory copyMemory = new CopyMemory();
                if (obj is Line)
                {
                    Line temp = (Line)obj;
                    Line drawObj = new Line(temp.start_point, temp.end_point);
                    if (temp.id_start_point_vertex != null)
                        drawObj.id_start_point_vertex = temp.id_start_point_vertex;
                    if (temp.id_end_point_vertex != null)
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

                    defCanvas.AddPuzzleObject(drawObj);
                    vertexMemory.Add(drawObj);
                    listCopyMemory.Add(copyMemory);
                    drawObj.ChangeState(staticState);
                }
                else if (obj is Oval)
                {
                    Oval temp = (Oval)obj;
                    Oval drawObj = new Oval(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Oval");

                    defCanvas.AddPuzzleObject(drawObj);
                    vertexMemory.Add(drawObj);
                    listCopyMemory.Add(copyMemory);
                    drawObj.ChangeState(staticState);
                }
                else if (obj is Parallelogram)
                {
                    Parallelogram temp = (Parallelogram)obj;
                    Parallelogram drawObj = new Parallelogram(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Parallelogram");
                     
                    defCanvas.AddPuzzleObject(drawObj);
                    vertexMemory.Add(drawObj);
                    listCopyMemory.Add(copyMemory);
                    drawObj.ChangeState(staticState);
                }
                else if (obj is Api.Shapes.Rectangle)
                {
                    Api.Shapes.Rectangle temp = (Api.Shapes.Rectangle)obj;
                    Api.Shapes.Rectangle drawObj = new Api.Shapes.Rectangle(temp.x, temp.y, temp.width, temp.height);
                    copyMemory.ID = drawObj.ID;
                    copyMemory.before_copied = obj.ID;
                    copyMemory.setObjectName("Rectangle");

                    defCanvas.AddPuzzleObject(drawObj);
                    vertexMemory.Add(drawObj);
                    listCopyMemory.Add(copyMemory);
                    drawObj.ChangeState(staticState);
                }
            }
            foreach (Line obj in listTempLine)
            {
                Line drawObj = new Line(obj.start_point, obj.end_point);
                foreach (Vertex obj2 in vertexMemory)
                {
                    Debug.WriteLine("Copy Mem: " + listCopyMemory.Count);
                    foreach (CopyMemory copyMem in listCopyMemory)
                    {
                        Debug.WriteLine("Line ID: " + obj.ID + " Vertex ID: " + obj2.ID + " Copy Mem: " + copyMem.ID);
                        if (obj2.ID == copyMem.ID)
                        {
                            if (obj.id_start_point_vertex == copyMem.before_copied )
                            {
                                drawObj.AddVertex(obj2, true);
                                drawObj.id_start_point_vertex = obj2.ID;
                                obj.id_start_point_vertex = obj2.ID;
                                Debug.WriteLine("Edge ID: " + copyMem.before_copied + " Vertex: " + obj2.ID);
                            }
                            if (obj.id_end_point_vertex == copyMem.before_copied)
                            {
                                drawObj.AddVertex(obj2, false);
                                drawObj.id_end_point_vertex = obj2.ID;
                                obj.id_end_point_vertex = obj2.ID;
                                Debug.WriteLine("Edge ID: " + copyMem.before_copied + " Vertex: " + obj2.ID);
                            }
                            if (obj2 != null)
                                obj2.Subscribe(drawObj);
                        }
                    }
                }
                listNewLine.Add(drawObj);
                drawObj.ChangeState(staticState);
                defCanvas.AddPuzzleObject(drawObj);
            }
            defCanvas.Repaint();
            defCanvas.PushUndoStack(this);
        }

        public void Unexecute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            for (int i = 0; i < vertexMemory.Count; i++)
            {
                for (int j = 0; j < listNewLine.Count; j++)
                {
                    if (listNewLine[j].id_start_point_vertex == vertexMemory[i].ID)
                    {
                        Debug.WriteLine("Undo Copying Edge, id: " + listNewLine[j].ID);
                        listNewLine[j].RemoveVertex(true);
                        vertexMemory[i].Unsubscribe(listNewLine[j]);
                    }
                    else if (listNewLine[j].id_end_point_vertex == vertexMemory[i].ID)
                    {
                        Debug.WriteLine("Undo Copying Edge, id: " + listNewLine[j].ID);
                        listNewLine[j].RemoveVertex(false);
                        vertexMemory[i].Unsubscribe(listNewLine[j]);
                    }
                    defCanvas.RemovePuzzleObject(listNewLine[j]);
                }
                foreach (CopyMemory copymem in listCopyMemory)
                {
                    if (copymem.ID == vertexMemory[i].ID)
                    {
                        Debug.WriteLine("Undo Copying Vertex, id: " + vertexMemory[i].ID);
                        defCanvas.RemovePuzzleObject(vertexMemory[i]);
                    }

                }
            }
            listTempLine.Clear();
            listNewLine.Clear();
            vertexMemory.Clear();
            defCanvas.Repaint();
            defCanvas.PushRedoStack(this);
        }
    }
}

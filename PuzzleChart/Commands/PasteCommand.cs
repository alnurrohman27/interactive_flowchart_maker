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

        public PasteCommand(ICanvas canvas)
        {
            this.canvas = canvas;
            this.staticState = new StaticState();
            this.vertexMemory = new List<Vertex>();
            this.listCopyMemory = new List<CopyMemory>();
            this.listTempLine = new List<Line>();
        }
        public void Execute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            List<PuzzleObject> memoryCopy = defCanvas.GetCopiedItems();
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
                }
                obj.ChangeState(staticState);
            }
            foreach (Line obj in listTempLine)
            {
                Line drawObj = new Line(obj.start_point, obj.end_point);

                foreach (Vertex obj2 in vertexMemory)
                {
                    foreach (CopyMemory copyMem in listCopyMemory)
                    {
                        if (obj2.ID == copyMem.ID)
                        {
                            if (obj.id_start_point_vertex == copyMem.before_copied )
                            {
                                drawObj.AddVertex(obj2, true);
                                drawObj.id_start_point_vertex = obj2.ID;
                                Debug.WriteLine("Edge ID: " + copyMem.before_copied + " Vertex: " + obj2.ID);
                            }
                            if (obj.id_end_point_vertex == copyMem.before_copied)
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
                defCanvas.AddPuzzleObject(drawObj);
            }
            defCanvas.Repaint();
        }

        public void Unexecute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            for (int i = 0; i < vertexMemory.Count; i++)
            {
                foreach(CopyMemory copyMem in listCopyMemory)
                {
                    if (copyMem.ID == vertexMemory[i].ID)
                    {
                        Console.WriteLine("Undo Copying object, ID: " + vertexMemory[i]);
                        defCanvas.RemovePuzzleObject(vertexMemory[i]);
                    }

                }
            }
        }
    }
}

using PuzzleChart.Api;
using PuzzleChart.Api.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace PuzzleChart.Commands
{
    public class InsertCommand : ICommand
    {
        private ICanvas canvas;
        private PuzzleObject obj;

        public InsertCommand(ICanvas canvas, PuzzleObject obj)
        {
            this.canvas = canvas;
            this.obj = obj;
        }

        public void Execute()
        {
            Debug.WriteLine("Insert Command is Executed");
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            defCanvas.AddPuzzleObject(obj);
            defCanvas.Repaint();
            defCanvas.PushUndoStack(this);
        }

        public void Unexecute()
        {
            Debug.WriteLine("Insert Command is Unexecuted");
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            defCanvas.RemovePuzzleObject(obj);
            defCanvas.Repaint();
            defCanvas.PushRedoStack(this);
        }
    }
}

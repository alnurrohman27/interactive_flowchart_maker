using PuzzleChart.Api;
using PuzzleChart.Api.Interfaces;
using System.Collections.Generic;
using System;
using PuzzleChart.Api.State;

namespace PuzzleChart.Commands
{
    public class DeleteCommand : ICommand
    {
        private ICanvas canvas;
        private List<PuzzleObject> listObj;
        private List<PuzzleObject> removedObj;

        public DeleteCommand(ICanvas canvas)
        {
            this.canvas = canvas;
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            this.listObj = defCanvas.GetSelectedObjects();
            this.removedObj = new List<PuzzleObject>();
        }

        public void Execute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            for(int i = 0; i < listObj.Count; i++)
            {
                this.removedObj.Add(listObj[i]);
                defCanvas.RemovePuzzleObject(listObj[i]);
            }
            defCanvas.Repaint();
            defCanvas.PushUndoStack(this);
        }

        public void Unexecute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            for (int i = 0; i < removedObj.Count; i++)
            {
                defCanvas.AddPuzzleObject(removedObj[i]);
            }
            removedObj.Clear();
            defCanvas.Repaint();
            defCanvas.PushRedoStack(this);
        }
    }
}
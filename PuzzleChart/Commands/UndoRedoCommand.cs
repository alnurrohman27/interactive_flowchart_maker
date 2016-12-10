using PuzzleChart.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Commands
{
    public class UndoRedoCommand : ICommand
    {
        private ICanvas canvas;
        
        public UndoRedoCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Undo()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            ICommand command = defCanvas.PopUndoStack();
            if(command != null)
            {
                Debug.WriteLine("Undo Command is executed");
                command.Unexecute();
                defCanvas.PushRedoStack(this);
            }
        }

        public void Redo()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            ICommand command = defCanvas.PopRedoStack();
            if(command != null)
            {
                Debug.WriteLine("Redo Command is executed");
                command.Execute();
                defCanvas.PushUndoStack(this);
            }
        }

        public void Execute()
        {
            Redo();
        }

        public void Unexecute()
        {
            Undo();
        }
    }
}

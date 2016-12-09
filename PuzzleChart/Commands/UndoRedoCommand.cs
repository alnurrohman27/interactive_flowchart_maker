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
                command.Unexecute();
        }

        public void Redo()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            ICommand command = defCanvas.PopRedoStack();
            if(command != null)
                command.Execute();
        }

        public void Execute()
        {
            Debug.WriteLine("Redo Command is executed");
            Redo();
        }

        public void Unexecute()
        {
            Debug.WriteLine("Undo Command is executed");
            Undo();
        }
    }
}

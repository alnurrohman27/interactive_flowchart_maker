using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class UndoCommand : ICommand
    {
        private ICanvas canvas;
        //Author: Reza 140
        //Class: Redo
        //Date : 11/9/2016
        //Membuat class dasar Undo untuk command pattern
        public UndoCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Execute()
        {
            canvas.Undo();
        }
    }
}

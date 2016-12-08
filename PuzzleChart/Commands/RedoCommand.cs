using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    class RedoCommand : ICommand
    {
        private ICanvas canvas;
        //Author: Reza 140
        //Class: Redo
        //Date : 11/9/2016
        //Membuat class dasar Redo untuk command pattern
        public RedoCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Execute()
        {
            canvas.Redo();
        }
    }
}

using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class CopyCommand : ICommand
    {
        private ICanvas canvas;

        public CopyCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }
        public void Execute()
        {
            this.canvas.CopyObject();
        }
    }
}

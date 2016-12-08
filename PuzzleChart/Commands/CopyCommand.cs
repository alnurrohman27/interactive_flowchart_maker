using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Api
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

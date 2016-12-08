using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class OpenCommand : ICommand
    {
        private ICanvas canvas;
        public OpenCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Execute()
        {
            this.canvas.Open();
        }
    }

}

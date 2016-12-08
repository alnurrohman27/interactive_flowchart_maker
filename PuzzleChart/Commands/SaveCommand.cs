using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class SaveCommand : ICommand
    {
        private ICanvas canvas;
        public SaveCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }
        public void Execute()
        {
            this.canvas.Save();
        }
    }
}

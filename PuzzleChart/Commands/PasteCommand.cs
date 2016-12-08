using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class PasteCommand : ICommand
    {
        ICanvas canvas;
        public PasteCommand(ICanvas canvas)
        {
            this.canvas = canvas;
        }
        public void Execute()
        {
            this.canvas.PasteObject();
        }
    }
}

using PuzzleChart.Api.Interfaces;
using System.Diagnostics;

namespace PuzzleChart.Commands
{
    public class CloseFileCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;

        public CloseFileCommand(ICanvas canvas, IEditor editor)
        {
            this.canvas = canvas;
            this.editor = editor;
        }
        public void Execute()
        {
            this.editor.RemoveCanvas(canvas);
        }

        public void Unexecute()
        {

        }
    }
}

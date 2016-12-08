using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class OpenCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;
        public OpenCommand(ICanvas canvas, IEditor editor)
        {
            this.canvas = canvas;
            this.editor = editor;
        }

        public void Execute()
        {
            this.canvas.Open();
            DefaultEditor defEditor = (DefaultEditor)editor;
            defEditor.SelectedTab.Text = this.canvas.Name;
        }
    }

}

using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Commands
{
    public class SaveCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;
        public SaveCommand(ICanvas canvas, IEditor editor)
        {
            this.canvas = canvas;
            this.editor = editor;
        }
        public void Execute()
        {
            this.canvas.Save();
            DefaultEditor defEditor = (DefaultEditor)editor;
            defEditor.SelectedTab.Text = this.canvas.Name; 
        }
    }
}

using PuzzleChart.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Commands
{
    public class NewFileCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;

        public NewFileCommand(IEditor editor)
        {
            this.canvas = new DefaultCanvas();
            this.editor = editor;
        }

        public void Execute()
        {
            DefaultEditor edit = (DefaultEditor)editor;
            this.canvas.Name = "Untitled - " + (edit.newTabCount);
            this.editor.AddCanvas(canvas);
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}

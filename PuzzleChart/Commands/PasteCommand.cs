using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

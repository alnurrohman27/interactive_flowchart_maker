using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

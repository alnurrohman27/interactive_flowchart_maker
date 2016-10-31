using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public interface ICanvas
    {
        void SetActiveTool(ITool tool);
        void Repaint();

        void AddPuzzleObject(PuzzleObject puzzle_object);
    }
}

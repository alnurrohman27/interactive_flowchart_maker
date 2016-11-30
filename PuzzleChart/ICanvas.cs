using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleChart.Tools;

namespace PuzzleChart
{
    public interface ICanvas
    {
        ITool GetActiveTool();
        void SetActiveTool(ITool tool);
        void Repaint();

        void AddPuzzleObject(PuzzleObject puzzle_object);
        void RemovePuzzleObject(PuzzleObject puzzle_object);
        PuzzleObject GetObjectAt(int x, int y);
        PuzzleObject SelectObjectAt(int x, int y);
        void DeselectAllObjects();

        void Undo();
        void Redo();
        void Save();
        void Open();
    }
}

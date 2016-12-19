using System;
using System.Collections.Generic;

namespace PuzzleChart.Api.Interfaces
{
    public interface ICanvas
    {
        ITool GetActiveTool();
        String Name { get; set; }
        Boolean Saved { get; set; }
        void SetActiveTool(ITool tool);
        void Repaint();

        List<PuzzleObject> GetAllObjects();
        void AddPuzzleObject(PuzzleObject puzzle_object);
        void RemovePuzzleObject(PuzzleObject puzzle_object);
        PuzzleObject GetObjectAt(int x, int y);
        PuzzleObject SelectObjectAt(int x, int y);
        void SelectAllObject();
        void DeselectAllObjects();
        void SetCopyItem(List<PuzzleObject> listObj);
        List<PuzzleObject> GetCopyItem();

    }
}

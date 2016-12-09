using System;
using System.Collections.Generic;

namespace PuzzleChart.Api.Interfaces
{
    public interface ICanvas
    {
        ITool GetActiveTool();
        String Name { get; set; }
        void SetActiveTool(ITool tool);
        void Repaint();

        List<PuzzleObject> GetAllObjects();
        void SetCopiedItems(List<PuzzleObject> listCopiedItems);
        List<PuzzleObject> GetCopiedItems();
        void AddPuzzleObject(PuzzleObject puzzle_object);
        void RemovePuzzleObject(PuzzleObject puzzle_object);
        PuzzleObject GetObjectAt(int x, int y);
        PuzzleObject SelectObjectAt(int x, int y);
        void DeselectAllObjects();

        void Open();
        void Save();
    }
}

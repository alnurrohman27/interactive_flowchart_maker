namespace PuzzleChart.Api.Interfaces
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
        void DeleteObject();
        void CopyObject();
        void PasteObject();
    }
}

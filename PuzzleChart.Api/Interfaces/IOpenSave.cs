using System.Collections.Generic;

namespace PuzzleChart.Api.Interfaces
{
    public interface IOpenSave
    {
        void Serialize(string path);
        List<PuzzleObject> Unserialize(string path);
    }
}

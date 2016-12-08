using System;

namespace PuzzleChart.Api.Interfaces
{
    public interface IToolbarItem
    {
        String Name { get; set; }
        void SetCommand(ICommand command);
    }
}

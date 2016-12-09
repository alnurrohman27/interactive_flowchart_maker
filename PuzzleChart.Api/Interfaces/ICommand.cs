namespace PuzzleChart.Api.Interfaces
{
    public interface ICommand
    {
        void Execute();
        void Unexecute();
    }
}

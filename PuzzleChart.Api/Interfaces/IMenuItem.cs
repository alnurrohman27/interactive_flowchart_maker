namespace PuzzleChart.Api.Interfaces
{
    public interface IMenuItem
    {
        string Text { get; set; }
        void AddMenuItem(IMenuItem menu_item);
        void AddSeparator();
        void SetCommand(ICommand command);
    }
}

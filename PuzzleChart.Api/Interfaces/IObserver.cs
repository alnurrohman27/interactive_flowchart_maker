namespace PuzzleChart.Api.Interfaces
{
    public interface IObserver
    {
        void Update(IObservable Vertex,int deltaX, int deltaY);
    }
}

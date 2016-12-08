using PuzzleChart.Api.Interfaces;
namespace PuzzleChart.Api
{
    public abstract class Edge : PuzzleObject, IObserver
    {
        public abstract void Update(IObservable Vertex, int deltaX, int deltaY);
    }
}

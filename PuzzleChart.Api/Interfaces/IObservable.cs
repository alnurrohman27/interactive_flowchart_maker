namespace PuzzleChart.Api.Interfaces
{
    public interface IObservable
    {
        void Subscribe(Edge O);
        void Unsubscribe(IObserver O);

    }
}

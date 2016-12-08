using System.Collections.Generic;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.Api
{
    public abstract class Vertex: PuzzleObject,IObservable
    {
        private List<Edge> edges;
        public Vertex()
        {
            edges = new List<Edge>();
            
        }
        
        public void BroadcastUpdate(int x, int y)
        {
            foreach(var edge in edges)
            {
                edge.Update(this, x, y);
            }
        }

        public void Subscribe(Edge O)
        {
            edges.Add(O);
        }

        public void Unsubscribe (IObserver O)
        {
            edges.Remove((Edge) O);
        }
        
        public List<Edge> GetEdges()
        {
            return this.edges;
        }
    }
}

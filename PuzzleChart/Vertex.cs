using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public abstract class Vertex: PuzzleObject,IObservable
    {
        private List<Edge> edges;
        public Vertex()
        {
            edges = new List<Edge>();
        }

        public void Subscribe(IObserver O)
        {
            edges.Add((Edge) O);
        }

        public void Unsubscribe (IObserver O)
        {
            edges.Remove((Edge) O);
        }
    }
}

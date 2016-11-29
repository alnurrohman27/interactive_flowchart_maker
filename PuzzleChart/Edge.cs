using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public abstract class Edge : PuzzleObject, IObserver
    {
        private Vertex vertex1, vertex2;
        public abstract void Update(int deltaX, int deltaY);
        public abstract void Update(IObservable Vertex, int deltaX, int deltaY);
    }
}

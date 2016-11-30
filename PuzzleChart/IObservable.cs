using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public interface IObservable
    {
        void Subscribe(Edge O);
        void Unsubscribe(IObserver O);

    }
}

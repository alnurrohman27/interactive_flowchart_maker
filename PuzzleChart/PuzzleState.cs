using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public abstract class PuzzleState
    {
        public PuzzleState State
        {
            get
            {
                return this.state;
            }
        }
        private PuzzleState state;

        public abstract void Draw(PuzzleObject obj);
        
        public virtual void Deselect(PuzzleObject obj)
        {

        }
        public virtual void Select(PuzzleObject obj)
        {

        }
    }

}

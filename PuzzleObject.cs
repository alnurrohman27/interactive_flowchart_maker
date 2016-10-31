using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public abstract class PuzzleObject
    {
        public Guid id { get; set; }
        public Graphics graphics { get; set; }
        
        public PuzzleObject()
        {
            id = Guid.NewGuid();
        }
        public abstract void Draw();
    }
}

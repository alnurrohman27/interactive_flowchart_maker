using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart
{
    public interface ITool
    {
        String Name { get; set; }
        Cursor cursor { get; }
        ICanvas target_canvas { get; set; }

        void ToolMouseDown(object sender, MouseEventArgs e);
        void ToolMouseUp(object sender, MouseEventArgs e);
        void ToolMouseMove(object sender, MouseEventArgs e);
    }
}

using System;
using System.Windows.Forms;

namespace PuzzleChart.Api.Interfaces
{
    public interface ITool
    {
        String Name { get; set; }
        Cursor cursor { get; }
        ICanvas target_canvas { get; set; }

        void ToolMouseDown(object sender, MouseEventArgs e);
        void ToolMouseUp(object sender, MouseEventArgs e);
        void ToolMouseMove(object sender, MouseEventArgs e);
        void ToolMouseDoubleClick(object sender, MouseEventArgs e);
        void ToolMouseDownAndKeys(object sender, MouseEventArgs e);
    }
}

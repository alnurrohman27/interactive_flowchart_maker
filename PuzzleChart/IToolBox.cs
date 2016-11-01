using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public delegate void ToolSelectedEventHandler(ITool tool);

    public interface IToolBox
    {
        event ToolSelectedEventHandler tool_selected;
        void AddTool(ITool tool);
        void RemoveTool(ITool tool);
        void AddSeparator();
        ITool active_tool { get; }
    }
}

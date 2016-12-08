using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Api.Interfaces
{
    public interface IEditor
    {

        IToolBox ToolBox { get; set; }
        void AddCanvas(ICanvas canvas);
        void SelectCanvas(ICanvas canvas);
        ICanvas GetSelectedCanvas();
        void RemoveCanvas(ICanvas canvas);
        void RemoveSelectedCanvas();

    }
}

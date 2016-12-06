using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart
{
    public interface IColor : ITool
    {
        ColorDialog ColorDialog { get; set; }

        void ShowColorBox(List<PuzzleObject> listObj);
    }
}

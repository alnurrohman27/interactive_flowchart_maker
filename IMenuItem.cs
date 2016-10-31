using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public interface IMenuItem
    {
        string Text { get; set; }
        void AddMenuItem(IMenuItem menu_item);
        void AddSeparator();
        void SetCommand(ICommand command);
    }
}

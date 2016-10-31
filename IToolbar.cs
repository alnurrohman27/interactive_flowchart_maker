using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public interface IToolbar
    {
        void AddToolbarItem(IToolbarItem toolbar_item);
        void AddSeparator();
    }
}

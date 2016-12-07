using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public interface IPlugin
    {
        String Name { get; set; }
        IPluginHost Host { get; set; }
    }
}

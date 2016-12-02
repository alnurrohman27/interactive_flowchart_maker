using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PuzzleChart
{
    public interface IOpenSave
    {
        void Serialize(string path);
        PuzzleObject Unserialize(string path);
    }
}

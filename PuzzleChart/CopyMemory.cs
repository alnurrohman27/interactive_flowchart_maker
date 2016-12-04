using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public class CopyMemory
    {
        public Guid before_copied { get; set; }
        public Guid ID { get; set; }

        private string objectName;

        public CopyMemory()
        {
            objectName = null;
        }

        public string getObjectName()
        {
            return this.objectName;
        }

        public void setObjectName(string name)
        {
            this.objectName = name;
        }
    }
}

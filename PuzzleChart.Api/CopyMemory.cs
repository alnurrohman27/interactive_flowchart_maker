using System;

namespace PuzzleChart.Api
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

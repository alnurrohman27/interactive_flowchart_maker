using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.ToolbarItems
{
    public class Copy : ToolStripButton, IToolbarItem
    {
        private ICommand command;
        public Copy()
        {
            this.Name = "Copy";
            this.Image = IconSet.copy;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.Click += CopyClick;
        }
        public void CopyClick(object sender, EventArgs e)
        {
            if (command != null)
            {
                this.command.Execute();
            }
        }
        public void SetCommand(ICommand command)
        {
            this.command = command;
        }
    }
}

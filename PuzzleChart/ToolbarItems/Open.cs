using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.ToolbarItems
{
    public class Open : ToolStripButton, IToolbarItem
    {
        private ICommand command;
        public Open()
        {
            this.Name = "Open";
            this.Image = IconSet.open;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.Click += OpenClick;
        }
        public void OpenClick(object sender, EventArgs e)
        {
            if(command != null)
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
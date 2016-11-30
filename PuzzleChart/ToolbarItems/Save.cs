using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.ToolbarItems
{
    public class Save : ToolStripButton, IToolbarItem
    {
        private ICommand command;
        public Save()
        {
            this.Name = "Save";
            this.Image = IconSet.save;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;

            this.Click += SaveClick;
        }
        public void SaveClick(object sender, EventArgs e)
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

using System;
using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;

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

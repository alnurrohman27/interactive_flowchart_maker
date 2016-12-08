using System;
using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart.ToolbarItems
{
    public class Paste : ToolStripButton, IToolbarItem
    {
        private ICommand command;
        public Paste()
        {
            this.Name = "Paste";
            this.Image = IconSet.paste;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.Click += PasteClick;
        }
        public void PasteClick(object sender, EventArgs e)
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

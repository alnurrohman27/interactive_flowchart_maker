using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.ToolbarItems
{
    public class Redo : ToolStripButton, IToolbarItem
    {
        private ICommand command;

        public Redo()
        {
            //Author: Reza 140
            //Class: Redo
            //Date : 11/9/2016
            //Adding Icon Redo and show in toolbox
            this.Name = "Redo";
            this.ToolTipText = "Redo";
            this.Image = IconSet.next_1;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;

            this.Click += RedoClick;
        }

        private void RedoClick(object sender, EventArgs e)
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

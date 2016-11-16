using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.ToolbarItems
{
    public class Undo : ToolStripButton, IToolbarItem
    {
        private ICommand command;

        public Undo()
        {
            //Author: Reza 140
            //Class: Redo
            //Date : 11/9/2016
            //Adding Icon Redo and show in toolbox
            this.Name = "Undo";
            this.ToolTipText = "Undo";
            this.Image = IconSet.back;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;

            this.Click += UndoClick;
        }

        private void UndoClick(object sender, EventArgs e)
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

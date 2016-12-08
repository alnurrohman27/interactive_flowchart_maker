using System.Drawing;
using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart
{
    public class DefaultMenubar : MenuStrip, IMenuBar
    {
        public DefaultMenubar()
        {
            this.Location = new Point(0, 0);
            this.Name = "menu";
            this.Size = new Size(624, 24);
            this.TabIndex = 0;
            this.Text = "menu";
        }

        public void AddMenuItem(IMenuItem menuItem)
        {
            this.Items.Add((ToolStripMenuItem)menuItem);
        }
    }
}

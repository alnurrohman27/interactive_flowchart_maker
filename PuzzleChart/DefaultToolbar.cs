using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart
{
    public class DefaultToolbar : ToolStrip,IToolbar
    {
        public DefaultToolbar()
        {
            this.Dock = DockStyle.Top;
        }
        
        public void AddToolbarItem(IToolbarItem item)
        {
            this.Items.Add((ToolStripItem)item);
        }

        public void AddSeparator()
        {
            this.Items.Add(new ToolStripSeparator());
        }
    }
}

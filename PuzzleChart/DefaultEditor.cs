using System;
using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace PuzzleChart
{
    class DefaultEditor : TabControl, IEditor
    {
        private List<ICanvas> canvases;
        private ICanvas selectedCanvas;
        private IToolBox toolBox;
        public int newTabCount;

        public DefaultEditor()
        {
            Dock = DockStyle.Fill;
            canvases = new List<ICanvas>();
            newTabCount = 1;
            this.Selected += DefaultEditor_Selected;
        }

        public IToolBox ToolBox
        {
            get
            {
                return this.toolBox;
            }

            set
            {
                this.toolBox = value;
            }
        }

        public void AddCanvas(ICanvas canvas)
        {
            canvases.Add(canvas);
            TabPage selectedTab = new TabPage(canvas.Name);
            selectedTab.Controls.Add((Control)canvas);
            this.Controls.Add(selectedTab);
            this.SelectedTab = selectedTab;
            this.selectedCanvas = canvas;
            newTabCount++;
        }

        public ICanvas GetSelectedCanvas()
        {
            return this.selectedCanvas;
        }

        public void RemoveCanvas(ICanvas canvas)
        {
            if(this.canvases.Count > 1)
            {
                Debug.WriteLine("Close Canvas: " + canvas.Name);
                this.Controls.Remove(this.SelectedTab);
                this.canvases.Remove(canvas);
                this.SelectedTab = (TabPage)this.Controls[0];
                this.selectedCanvas = canvases[0];
            }
        }

        public void RemoveSelectedCanvas()
        {

        }

        public void SelectCanvas(ICanvas canvas)
        {
            this.selectedCanvas = canvas;
        }

        private void DefaultEditor_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                this.selectedCanvas = (ICanvas)e.TabPage.Controls[0];
                Debug.WriteLine("Canvas: " + selectedCanvas.Name + " is selected");
            }

            catch (Exception ex)
            {
                
            }

        }

    }
}

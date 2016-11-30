using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PuzzleChart.ToolbarItems;
using PuzzleChart.Tools;
using PuzzleChart.Commands;

namespace PuzzleChart
{
    public partial class MainWindow : Form
    {
        private IToolBox tool_box;
        private ICanvas canvas;
        private IToolbar toolbar;
        private IMenuBar menubar;

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Initializing UI objects.");

            #region Canvas

            Debug.WriteLine("Loading canvas...");
            this.canvas = new DefaultCanvas();
            this.toolStripContainer1.ContentPanel.Controls.Add((Control)this.canvas);

            #endregion


            #region Menubar

            Debug.WriteLine("Loading menubar...");
            this.menubar = new DefaultMenubar();
            this.Controls.Add((Control)this.menubar);

            DefaultMenuItem fileMenuItem = new DefaultMenuItem("File");
            this.menubar.AddMenuItem(fileMenuItem);

            DefaultMenuItem newMenuItem = new DefaultMenuItem("New");
            fileMenuItem.AddMenuItem(newMenuItem);
            DefaultMenuItem openMenuItem = new DefaultMenuItem("Open");
            fileMenuItem.AddMenuItem(openMenuItem);
            openMenuItem.Click += new System.EventHandler(this.OnOpenMenuItemClick);
            DefaultMenuItem saveMenuItem = new DefaultMenuItem("Save");
            fileMenuItem.AddMenuItem(saveMenuItem);
            saveMenuItem.Click += new System.EventHandler(this.OnSaveMenuItemClick);
            fileMenuItem.AddSeparator();
            DefaultMenuItem exitMenuItem = new DefaultMenuItem("Exit");
            fileMenuItem.AddMenuItem(exitMenuItem);
            exitMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClick);
             
            DefaultMenuItem editMenuItem = new DefaultMenuItem("Edit");
            this.menubar.AddMenuItem(editMenuItem);

            DefaultMenuItem undoMenuItem = new DefaultMenuItem("Undo");
            editMenuItem.AddMenuItem(undoMenuItem);
            undoMenuItem.Click += new System.EventHandler(this.OnUndoMenuItemClick);

            DefaultMenuItem redoMenuItem = new DefaultMenuItem("Redo");
            editMenuItem.AddMenuItem(redoMenuItem);
            redoMenuItem.Click += new System.EventHandler(this.OnRedoMenuItemClick);

            DefaultMenuItem viewMenuItem = new DefaultMenuItem("View");
            this.menubar.AddMenuItem(viewMenuItem);

            DefaultMenuItem helpMenuItem = new DefaultMenuItem("Help");
            this.menubar.AddMenuItem(helpMenuItem);

            DefaultMenuItem aboutMenuItem = new DefaultMenuItem("About");
            helpMenuItem.AddMenuItem(aboutMenuItem);
            helpMenuItem.Click += new System.EventHandler(this.OnAboutMenuItemClick);

            #endregion

            #region Toolbox

            // Initializing toolbox
            Debug.WriteLine("Loading toolbox...");
            this.tool_box = new DefaultToolbox();
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add((Control)this.tool_box);

            #endregion

            #region Tools

            // Initializing tools
            Debug.WriteLine("Loading tools...");
            //this.tool_box.AddTool(new SelectionTool());
            this.tool_box.AddSeparator();
            this.tool_box.AddTool(new LineTool());
            this.tool_box.AddTool(new DiamondTool());
            this.tool_box.AddTool(new RectangleTool());
            this.tool_box.AddTool(new ParallelogramTool());
            this.tool_box.AddTool(new OvalTool());
            this.tool_box.AddTool(new SelectionTool());
            //this.tool_box.AddTool(new StatefulLineTool());
            //this.tool_box.AddTool(new RectangleTool());
            this.tool_box.tool_selected += Toolbox_ToolSelected;

            #endregion

            #region Toolbar

            // Initializing toolbar
            Debug.WriteLine("Loading toolbar...");
            this.toolbar = new DefaultToolbar();
            this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control)this.toolbar);

            OpenCommand openCmd = new OpenCommand(this.canvas);
            SaveCommand saveCmd = new SaveCommand(this.canvas);
            UndoCommand undoCmd = new UndoCommand(this.canvas);
            RedoCommand redoCmd = new RedoCommand(this.canvas);

            Open toolItemOpen = new Open();
            toolItemOpen.SetCommand(undoCmd);
            Save toolItemSave = new Save();
            toolItemSave.SetCommand(saveCmd);
            Undo toolItemUndo = new Undo();
            toolItemUndo.SetCommand(undoCmd);
            Redo toolItemRedo = new Redo();
            toolItemRedo.SetCommand(redoCmd);

            this.toolbar.AddToolbarItem(toolItemOpen);
            this.toolbar.AddToolbarItem(toolItemSave);
            this.toolbar.AddSeparator();
            this.toolbar.AddToolbarItem(toolItemUndo);
            this.toolbar.AddToolbarItem(toolItemRedo);
            #endregion
        }

        #region Method
        private void Toolbox_ToolSelected(ITool tool)
        {
            if (this.canvas != null)
            {
                Debug.WriteLine("Tool " + tool.Name + " is selected");
                this.canvas.SetActiveTool(tool);
                tool.target_canvas = this.canvas;
            }
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click_1(object sender, EventArgs e)
        {

        }
        
        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }
        private void OnExitMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnAboutMenuItemClick(object sender, EventArgs e)
        {
            MessageBox.Show("Interactive Flow Chart Maker\n byKPL Kel 1");
        }

        private void OnUndoMenuItemClick(object sender, EventArgs e)
        {
            this.canvas.Undo();
        }

        private void OnRedoMenuItemClick(object sender, EventArgs e)
        {
            this.canvas.Redo();
        }

        #endregion

        private void Main_Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                this.canvas.Undo();
            }else if (e.Control && e.KeyCode == Keys.Y)
            {
                this.canvas.Redo();
            }
        }

        private void OnSaveMenuItemClick(object sender, EventArgs e)
        {
            this.canvas.Save();
        }

        private void OnOpenMenuItemClick(object sender, EventArgs e)
        {
            this.canvas.Open();
        }
    }
}

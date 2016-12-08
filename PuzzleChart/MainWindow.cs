using System;
using System.Diagnostics;
using System.Windows.Forms;
using PuzzleChart.ToolbarItems;
using PuzzleChart.Tools;
using PuzzleChart.Commands;
using System.IO;
using System.Reflection;
using PuzzleChart.Api;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private IToolBox tool_box;
        private ICanvas canvas;
        private IToolbar toolbar;
        private IMenuBar menubar;
        private IPlugin[] plugins;

        public MainWindow()
        {
            InitializeComponent();
            LoadPlugins();
            InitUI();
        }

        private static void InstantiateMyTypeFail(AppDomain domain)
        {
            // Calling InstantiateMyType will always fail since the assembly info
            // given to CreateInstance is invalid.
            try
            {
                // You must supply a valid fully qualified assembly name here.
                domain.CreateInstance("Assembly text name, Version, Culture, PublicKeyToken", "MyType");
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
        }

        private void LoadPlugins()
        {
            string path = Application.StartupPath + "\\Plugins";
            string[] pluginFiles = Directory.GetFiles(path, "*.dll");
            plugins = new IPlugin[pluginFiles.Length];

            for (int i = 0; i < pluginFiles.Length; i++)
            {
                Type type = null;
                try
                {
                    Assembly asm = Assembly.LoadFile(pluginFiles[i]);
                    if (asm != null)
                    {
                        var pluginInterface = typeof(IPlugin);
                        
                        foreach (Type t in asm.GetTypes())
                        {
                            if (pluginInterface.IsAssignableFrom(t))
                            {
                                type = t;
                            }
                        }
                    }

                    if (type != null)
                        plugins[i] = (IPlugin)Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Load Plugins: " + e.Message);
                }
            }
        }
        
        private void InitUI()
        {
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

            DefaultMenuItem copyMenuItem = new DefaultMenuItem("Copy");
            editMenuItem.AddMenuItem(copyMenuItem);
            copyMenuItem.Click += new System.EventHandler(this.OnCopyMenuItemClick);

            DefaultMenuItem pasteMenuItem = new DefaultMenuItem("Paste");
            editMenuItem.AddMenuItem(pasteMenuItem);
            copyMenuItem.Click += new System.EventHandler(this.OnPasteMenuItemClick);

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
            //this.tool_box.AddTool(new StatefulLineTool());
            //this.tool_box.AddTool(new RectangleTool());

            if (plugins != null)
            {
                for (int i = 0; i < this.plugins.Length; i++)
                {
                    this.tool_box.Register(plugins[i]);
                }
            }

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
            CopyCommand copyCmd = new CopyCommand(this.canvas);
            PasteCommand pasteCmd = new PasteCommand(this.canvas);

            Open toolItemOpen = new Open();
            toolItemOpen.SetCommand(openCmd);
            Save toolItemSave = new Save();
            toolItemSave.SetCommand(saveCmd);
            Undo toolItemUndo = new Undo();
            toolItemUndo.SetCommand(undoCmd);
            Redo toolItemRedo = new Redo();
            toolItemRedo.SetCommand(redoCmd);
            Copy toolItemCopy = new Copy();
            toolItemCopy.SetCommand(copyCmd);
            Paste toolItemPaste = new Paste();
            toolItemPaste.SetCommand(pasteCmd);

            this.toolbar.AddToolbarItem(toolItemOpen);
            this.toolbar.AddToolbarItem(toolItemSave);
            this.toolbar.AddToolbarItem(toolItemCopy);
            this.toolbar.AddToolbarItem(toolItemPaste);
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
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                this.canvas.Redo();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                this.canvas.Save();
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                this.canvas.Open();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                this.canvas.CopyObject();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                this.canvas.PasteObject();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                this.canvas.DeleteObject();
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

        private void OnPasteMenuItemClick(object sender, EventArgs e)
        {
            this.canvas.PasteObject();
        }

        private void OnCopyMenuItemClick(object sender, EventArgs e)
        {
            this.canvas.CopyObject();
        }
    }
}

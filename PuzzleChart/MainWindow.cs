using System;
using System.Diagnostics;
using System.Windows.Forms;
using PuzzleChart.ToolbarItems;
using PuzzleChart.Tools;
using PuzzleChart.Commands;
using System.IO;
using System.Reflection;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api;
using PuzzleChart.Api.State;
using System.Drawing;

namespace PuzzleChart
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private IToolBox tool_box;
        private IToolbar toolbar;
        private IMenuBar menubar;
        private IPlugin[] plugins;
        private IEditor editor;

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
            editor = new DefaultEditor();
            this.toolStripContainer1.ContentPanel.Controls.Add((Control)this.editor);

            ICanvas canvas = new DefaultCanvas();
            canvas.Name = "Untitled - 1";
            this.editor.AddCanvas(canvas);


            #endregion


            #region Menubar

            Debug.WriteLine("Loading menubar...");
            this.menubar = new DefaultMenubar();
            this.Controls.Add((Control)this.menubar);

            DefaultMenuItem fileMenuItem = new DefaultMenuItem("File");
            this.menubar.AddMenuItem(fileMenuItem);

            DefaultMenuItem newMenuItem = new DefaultMenuItem("New");
            fileMenuItem.AddMenuItem(newMenuItem);
            newMenuItem.Click += new EventHandler(this.OnNewMenuItemClick);
            DefaultMenuItem closeMenuItem = new DefaultMenuItem("Close");
            fileMenuItem.AddMenuItem(closeMenuItem);
            closeMenuItem.Click += new EventHandler(this.OnCloseMenuItemClick);
            DefaultMenuItem openMenuItem = new DefaultMenuItem("Open");
            fileMenuItem.AddMenuItem(openMenuItem);
            openMenuItem.Click += new EventHandler(this.OnOpenMenuItemClick);
            DefaultMenuItem saveMenuItem = new DefaultMenuItem("Save");
            fileMenuItem.AddMenuItem(saveMenuItem);
            saveMenuItem.Click += new EventHandler(this.OnSaveMenuItemClick);
            DefaultMenuItem exportMenuItem = new DefaultMenuItem("Export");
            fileMenuItem.AddMenuItem(exportMenuItem);
            exportMenuItem.Click += new EventHandler(this.OnExportMenuItemClick);
            fileMenuItem.AddSeparator();
            DefaultMenuItem exitMenuItem = new DefaultMenuItem("Exit");
            fileMenuItem.AddMenuItem(exitMenuItem);
            exitMenuItem.Click += new EventHandler(OnExitMenuItemClick);

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
            this.editor.ToolBox = tool_box;

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
            this.tool_box.AddTool(new TerminatorTool());
            //this.tool_box.AddTool(new StatefulLineTool());
            //this.tool_box.AddTool(new RectangleTool());

            if (plugins != null)
            {
                for (int i = 0; i < this.plugins.Length; i++)
                {
                    this.tool_box.Register(plugins[i]);
                }
            }
            this.tool_box.AddTool(new SelectionTool());

            this.tool_box.tool_selected += Toolbox_ToolSelected;

            #endregion

            #region Toolbar

            // Initializing toolbar
            Debug.WriteLine("Loading toolbar...");
            this.toolbar = new DefaultToolbar();
            this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control)this.toolbar);

            if(editor != null)
            {
                OpenCommand openCmd = new OpenCommand(editor.GetSelectedCanvas(), editor);
                SaveCommand saveCmd = new SaveCommand(editor.GetSelectedCanvas(),editor);
                CopyCommand copyCmd = new CopyCommand(editor.GetSelectedCanvas());
                PasteCommand pasteCmd = new PasteCommand(editor.GetSelectedCanvas());
                UndoRedoCommand undoRedoCmd = new UndoRedoCommand(editor.GetSelectedCanvas());

                Open toolItemOpen = new Open();
                toolItemOpen.SetCommand(openCmd);
                Save toolItemSave = new Save();
                toolItemSave.SetCommand(saveCmd);
                Copy toolItemCopy = new Copy();
                toolItemCopy.SetCommand(copyCmd);
                Paste toolItemPaste = new Paste();
                toolItemPaste.SetCommand(pasteCmd);
                Undo toolItemUndo = new Undo();
                toolItemUndo.SetCommand(undoRedoCmd);
                Redo toolItemRedo = new Redo();
                toolItemRedo.SetCommand(undoRedoCmd);

                this.toolbar.AddToolbarItem(toolItemOpen);
                this.toolbar.AddToolbarItem(toolItemSave);
                this.toolbar.AddToolbarItem(toolItemCopy);
                this.toolbar.AddToolbarItem(toolItemPaste);
                this.toolbar.AddSeparator();
                this.toolbar.AddToolbarItem(toolItemUndo);
                this.toolbar.AddToolbarItem(toolItemRedo);
            }
            #endregion
        }

        #region Method
        private void Toolbox_ToolSelected(ITool tool)
        {
            if (this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                Debug.WriteLine("Tool " + tool.Name + " is selected");
                canvas.SetActiveTool(tool);
                tool.target_canvas = canvas;

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

        private void OnNewMenuItemClick(object sender, EventArgs e)
        {
            NewFileCommand newFile = new NewFileCommand(this.editor);
            newFile.Execute();
        }

        private void OnCloseMenuItemClick(object sender, EventArgs e)
        {
            ICanvas canvas = this.editor.GetSelectedCanvas();
            CloseFileCommand closeCmd = new CloseFileCommand(canvas, this.editor);
            closeCmd.Execute();
        }

        private void OnExitMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnAboutMenuItemClick(object sender, EventArgs e)
        {
            MessageBox.Show("Interactive Flow Chart Maker\n byKPL Kel 1");
        }

        #endregion

        private void Main_Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                UndoRedoCommand undoRedoCmd = new UndoRedoCommand(canvas);
                undoRedoCmd.Unexecute();
            }
            else if (e.Control && e.KeyCode == Keys.Y && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                UndoRedoCommand undoRedoCmd = new UndoRedoCommand(canvas);
                undoRedoCmd.Execute();
            }
            else if (e.Control && e.KeyCode == Keys.N && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                NewFileCommand newCmd = new NewFileCommand(this.editor);
                newCmd.Execute();
            }
            else if (e.Control && e.KeyCode == Keys.S && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                SaveCommand saveCmd = new SaveCommand(canvas, editor);
                saveCmd.Execute();
            }
            else if (e.Control && e.KeyCode == Keys.O && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                OpenCommand openCmd = new OpenCommand(canvas, editor);
                openCmd.Execute();
            }
            else if (e.Control && e.KeyCode == Keys.C && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                CopyCommand copyCmd = new CopyCommand(canvas);
                copyCmd.Execute();
            }
            else if (e.Control && e.KeyCode == Keys.V && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                PasteCommand pasteCmd = new PasteCommand(canvas);
                pasteCmd.Execute();
            }
            else if (e.Control && e.KeyCode == Keys.A && this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                canvas.SelectAllObject();
            }
            else if(e.KeyCode == Keys.Delete)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                DeleteCommand deleteCmd = new DeleteCommand(canvas);
                deleteCmd.Execute();
            }
        }

        private void OnUndoMenuItemClick(object sender, EventArgs e)
        {
            if (this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                UndoRedoCommand undoRedoCmd = new UndoRedoCommand(canvas);
                undoRedoCmd.Unexecute();
            }

        }

        private void OnRedoMenuItemClick(object sender, EventArgs e)
        {
            if (this.editor != null)
            {
                ICanvas canvas = this.editor.GetSelectedCanvas();
                UndoRedoCommand undoRedoCmd = new UndoRedoCommand(canvas);
                undoRedoCmd.Unexecute();
            }
        }

        private void OnSaveMenuItemClick(object sender, EventArgs e)
        {
            ICanvas canvas = this.editor.GetSelectedCanvas();
            SaveCommand saveCmd = new SaveCommand(canvas, editor);
            saveCmd.Execute();
        }

        private void OnOpenMenuItemClick(object sender, EventArgs e)
        {
            ICanvas canvas = this.editor.GetSelectedCanvas();
            OpenCommand openCmd = new OpenCommand(canvas, editor);
            openCmd.Execute();
        }

        private void OnPasteMenuItemClick(object sender, EventArgs e)
        {
            ICanvas canvas = this.editor.GetSelectedCanvas();
            PasteCommand pasteCmd = new PasteCommand(canvas);
            pasteCmd.Execute();
        }

        private void OnCopyMenuItemClick(object sender, EventArgs e)
        {
            ICanvas canvas = this.editor.GetSelectedCanvas();
            CopyCommand copyCmd = new CopyCommand(canvas);
            copyCmd.Execute();
        }

        private void OnExportMenuItemClick(object sender, EventArgs e)
        {
            ICanvas canvas = this.editor.GetSelectedCanvas();
            int width = this.Width;
            int height = this.Height;
            int imageWidth = this.toolStripContainer1.ContentPanel.Size.Width - 26;
            int imageHeight = this.toolStripContainer1.ContentPanel.Size.Height - 27;
            Bitmap bmp = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            DrawToBitmap(bmp, rect);
            ExportCommand exportCmd = new ExportCommand(canvas, editor, width, height, imageWidth, imageHeight, bmp);
            exportCmd.Execute();
        }
    }
}

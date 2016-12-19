using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using PuzzleChart.Api;
using PuzzleChart.Api.Interfaces;
using PuzzleChart.Api.Shapes;
using PuzzleChart.Api.State;
using System.IO;

namespace PuzzleChart
{
    public class DefaultCanvas : Control, ICanvas
    {
        private ITool activeTool;
        private List<PuzzleObject> puzzle_objects;
        private Stack<ICommand> undoStack;
        private Stack<ICommand> redoStack;
        private List<PuzzleObject> listCopyItem;
        private bool saveFlag;

        public bool KeyPreview { get; private set; }

        public Boolean Saved
        {
            get
            {
                return this.saveFlag;
            }

            set
            {
                this.saveFlag = value;
            }
        }

        public DefaultCanvas()
        {
            this.puzzle_objects = new List<PuzzleObject>();

            this.redoStack = new Stack<ICommand>();
            this.undoStack = new Stack<ICommand>();

            this.listCopyItem = new List<PuzzleObject>();

            this.Saved = false;

            this.DoubleBuffered = true;

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            this.Paint += DefaultCanvas_Paint;
            this.MouseDown += DefaultCanvas_MouseDown;
            this.MouseUp += DefaultCanvas_MouseUp;
            this.MouseMove += DefaultCanvas_MouseMove;
            this.MouseDoubleClick += DefaultCanvas_MouseDoubleClick;
            this.KeyDown += DefaultCanvas_KeyDown;

        }

        public ICommand PopUndoStack()
        {
            if (undoStack.Count > 0)
            {
                ICommand command = this.undoStack.Pop();
                return command;
            }
            else
                return null;

        }

        public void PushUndoStack(ICommand command)
        {
            this.undoStack.Push(command);
        }

        public ICommand PopRedoStack()
        { 
            if (redoStack.Count > 0)
            {
                ICommand command = this.redoStack.Pop();
                return command;
            }
            else
                return null;
        }

        public void PushRedoStack(ICommand command)
        {
            this.redoStack.Push(command);
        }

        public void ClearStack()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }

        public void RemovePuzzleObject(PuzzleObject puzzle_object)
        {
            this.puzzle_objects.Remove(puzzle_object);
        }

        public ITool GetActiveTool()
        {
            return this.activeTool;
        }

        private void DefaultCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseMove(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseUp(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                if(Control.ModifierKeys == Keys.None)
                    this.activeTool.ToolMouseDown(sender, e);
                else if(Control.ModifierKeys == Keys.Control)
                    this.activeTool.ToolMouseDownAndKeys(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_Paint(object sender, PaintEventArgs e)
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                obj.SetGraphics(e.Graphics);
                obj.Draw();
            }
        }

        private void DefaultCanvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.activeTool.ToolMouseDoubleClick(sender, e);
        }

        private void DefaultCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.A)
            {
                SelectAllObject();
            }
        }

        public void SelectAllObject()
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                if(obj.State is StaticState)
                {
                    PuzzleState editState = new EditState();
                    obj.ChangeState(editState);
                    Repaint();
                }
            }
        }

        public void Repaint()
        {
            this.Invalidate();
            this.Update();
        }

        public void SetActiveTool(ITool tool)
        {
            this.activeTool = tool;
        }

        public void SetBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        public void AddPuzzleObject(PuzzleObject puzzle_object)
        {
            this.puzzle_objects.Add(puzzle_object);
        }

        public PuzzleObject GetObjectAt(int x, int y)
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                if (obj.Intersect(x, y))
                {
                    return obj;
                }
            }
            return null;
        }

        public PuzzleObject SelectObjectAt(int x, int y)
        {
            PuzzleObject obj = GetObjectAt(x, y);
            if (obj != null)
            {
                obj.Select();
            }

            return obj;
        }

        public List<PuzzleObject> GetAllObjects()
        {
            return this.puzzle_objects;
        }

        public void DeselectAllObjects()
        {
            foreach (PuzzleObject drawObj in puzzle_objects)
            {
                drawObj.Deselect();
            }
        }

        public List<PuzzleObject> GetSelectedObjects()
        {
            List<PuzzleObject> listObj = new List<PuzzleObject>();
            foreach(PuzzleObject obj in this.puzzle_objects)
            {
                if (obj.State is EditState)
                    listObj.Add(obj);
            }
            return listObj;
        }

        public void SetCopyItem(List<PuzzleObject> listObj)
        {
            this.listCopyItem = listObj;
        }

        public List<PuzzleObject> GetCopyItem()
        {
            return this.listCopyItem;
        }
    }
}


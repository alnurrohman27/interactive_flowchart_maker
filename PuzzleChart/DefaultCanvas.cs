using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PuzzleChart
{
    public class DefaultCanvas : Control,ICanvas
    {
        private ITool activeTool;
        private List<PuzzleObject> puzzle_objects;
        private List<PuzzleObject> memory_stack;
        private PuzzleObject temp;

        public DefaultCanvas()
        {
            this.puzzle_objects = new List<PuzzleObject>();
            this.memory_stack = new List<PuzzleObject>();
            this.DoubleBuffered = true;

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            this.Paint += DefaultCanvas_Paint;
            this.MouseDown += DefaultCanvas_MouseDown;
            this.MouseUp += DefaultCanvas_MouseUp;
            this.MouseMove += DefaultCanvas_MouseMove;
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
                this.activeTool.ToolMouseDown(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_Paint(object sender, PaintEventArgs e)
        {
            foreach (PuzzleObject obj in puzzle_objects)
            {
                obj.graphics = e.Graphics;
                obj.Draw();
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

        public void Undo()
        {
            var last = puzzle_objects.Count - 1;
            if(last >= 0)
            {
                this.temp = puzzle_objects[puzzle_objects.Count - 1];
                puzzle_objects.RemoveAt(puzzle_objects.Count - 1);
                memory_stack.Add(temp);
                Debug.WriteLine("Undo is selected");
                this.Repaint();
            }        
           
        }

        public void Redo()
        {
            var last = memory_stack.Count - 1;
            if(last >= 0)
            {
                this.temp = memory_stack[memory_stack.Count - 1];
                memory_stack.RemoveAt(memory_stack.Count - 1);
                puzzle_objects.Add(temp);
                Debug.WriteLine("Redo is selected");
                this.Repaint();
            }
        }
    }
}

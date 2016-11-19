using System;
using PuzzleChart.State;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PuzzleChart
{
    public abstract class PuzzleObject
    {
        public Guid ID { get; set; }
        public Graphics graphics { get; set; }

        public PuzzleState State
        {
            get
            {
                return this.state;
            }
        }

        private PuzzleState state;

        public PuzzleObject()
        {
            ID = Guid.NewGuid();
            this.ChangeState(PreviewState.GetInstance()); //default initial state
        }

        public abstract bool Intersect(int xTest, int yTest);
        public abstract void Translate(int x, int y, int xAmount, int yAmount);
        public abstract void RenderOnPreview();
        public abstract void RenderOnEditingView();
        public abstract void RenderOnStaticView();

        public void ChangeState(PuzzleState state)
        {
            this.state = state;
        }

        public virtual void Draw()
        {
            this.state.Draw(this);
        }

        public void Select()
        {
            Debug.WriteLine("Object id=" + ID.ToString() + " is selected.");
            this.state.Select(this);
        }

        public void Deselect()
        {
            Debug.WriteLine("Object id=" + ID.ToString() + " is deselected.");
            this.state.Deselect(this);
        }

    }
}

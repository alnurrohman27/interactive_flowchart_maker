using PuzzleChart.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public abstract class StatefulPuzzleObject : PuzzleObject
    {
        public PuzzleState State
        {
            get
            {
                return this.state;
            }
        }

        private PuzzleState state;

        public abstract void RenderOnStaticView();
        public abstract void RenderOnEditingView();
        public abstract void RenderOnPreview();

        public StatefulPuzzleObject()
        {
            this.ChangeState(PreviewState.GetInstance());
        }

        public void ChangeState(PuzzleState state)
        {
            this.state = state;
        }

        public override void Draw()
        {
            this.state.Draw(this);
        }

        public void Select()
        {
            Debug.WriteLine("Object id =" + id.ToString() + "is selected");
            this.state.Select(this);
        }
        public void Deselect()
        {
            Debug.WriteLine("Object id =" + id.ToString() + "is selected");
            this.state.Deselect(this);
        }
    }
}

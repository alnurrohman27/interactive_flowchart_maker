using System;
using PuzzleChart.Api.State;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace PuzzleChart.Api
{
    public abstract class PuzzleObject
    {
        public Guid ID { get; set; }
        public TranslateMemory transMem { get; set; }
        public List<TranslateMemory> translate = new List<TranslateMemory>();
        public int translate_count = 0;

        public PuzzleState State
        {
            get
            {
                return this.state;
            }
        }

        private PuzzleState state;
        private Graphics graphics;

        public PuzzleObject()
        {
            ID = Guid.NewGuid();
            transMem = new TranslateMemory();
            this.ChangeState(PreviewState.GetInstance()); //default initial state
        }

        public abstract bool Add(PuzzleObject obj);
        public abstract bool Remove(PuzzleObject obj);
        public abstract Point LineIntersect(Point start_point, Point end_point);
        public abstract bool Intersect(int xTest, int yTest);
        public abstract void Translate(int x, int y, int xAmount, int yAmount);
        public abstract void RenderOnPreview();
        public abstract void RenderOnEditingView();
        public abstract void RenderOnStaticView();
        public abstract void TranslateUndoRedo(bool undoRedo);

        public void ChangeState(PuzzleState state)
        {
            this.state = state;
        }

        public virtual void Draw()
        {
            this.state.Draw(this);
        }

        public virtual void SetGraphics(Graphics graphics)
        {
            this.graphics = graphics;
        }

        public virtual Graphics GetGraphics()
        {
            return this.graphics;
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

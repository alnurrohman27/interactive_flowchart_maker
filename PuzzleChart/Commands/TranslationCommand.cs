using PuzzleChart.Api;
using PuzzleChart.Api.Interfaces;
using System.Diagnostics;

namespace PuzzleChart.Commands
{
    public class TranslationCommand : ICommand
    {
        private ICanvas canvas;
        private PuzzleObject obj;
        private int x, y, xAmount, yAmount;
        private bool translateRedo;

        public TranslationCommand(ICanvas canvas, PuzzleObject obj, int x, int y, int xAmount, int yAmount)
        {
            this.canvas = canvas;
            this.obj = obj;
            this.x = x;
            this.y = y;
            this.xAmount = xAmount;
            this.yAmount = yAmount;
            this.translateRedo = false;
        }

        public void Execute()
        {
            Debug.WriteLine("Translate Object: " + this.obj.ID);
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            if (translateRedo)
            {
                this.obj.Translate(this.x, this.y, this.xAmount, this.yAmount);
                defCanvas.Repaint();
            }
            this.translateRedo = true;
            defCanvas.PushUndoStack(this);
        }

        public void Unexecute()
        {
            Debug.WriteLine("Untranslate Object: " + this.obj.ID);
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            this.obj.Untranslate(this.x, this.y, xAmount, yAmount);
            defCanvas.Repaint();
            defCanvas.PushRedoStack(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.State
{
    public class EditState : PuzzleState
    {
        private static PuzzleState instance;

        public static PuzzleState GetInstance()
        {
            if(instance == null)
            {
                instance = new EditState();
            }
            return instance;
        }
        
        public override void Draw(StatefulPuzzleObject obj)
        {
            obj.RenderOnEditingView();
        }
        public override void Deselect(StatefulPuzzleObject obj)
        {
            obj.ChangeState(StaticState.GetInstance());
        }
    }
}

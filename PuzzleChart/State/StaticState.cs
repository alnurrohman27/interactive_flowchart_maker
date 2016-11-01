using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.State
{
    public class StaticState : PuzzleState
    {
        private static PuzzleState instance;

        public static PuzzleState GetInstance()
        {
            if (instance == null)
            {
                instance = new StaticState();
            }
            return instance;
        }

        public override void Draw(StatefulPuzzleObject obj)
        {
            obj.RenderOnStaticView();
        }

        public override void Select(StatefulPuzzleObject obj)
        {
            obj.ChangeState(EditState.GetInstance());
        }
    }
}

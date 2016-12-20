using System;
using PuzzleChart.Api.Interfaces;
using System.Collections.Generic;
using PuzzleChart.Api;
using PuzzleChart.Api.State;

namespace PuzzleChart.Commands
{
    public class CopyCommand : ICommand
    {
        private ICanvas canvas;
        private List<PuzzleObject> listObj;

        public CopyCommand(ICanvas canvas)
        {
            this.canvas = canvas;
            this.listObj = new List<PuzzleObject>();
        }
        public void Execute()
        {
            DefaultCanvas defCanvas = (DefaultCanvas)canvas;
            foreach (PuzzleObject obj in defCanvas.GetAllObjects())
            {
                if (obj.State is EditState)
                {
                    Console.WriteLine("Copy object to clipboard: " + obj.ID);
                    listObj.Add(obj);
                }
            }
            defCanvas.SetCopyItem(listObj);
        }

        public void Unexecute()
        {

        }
    }
}

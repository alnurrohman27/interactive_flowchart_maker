﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public interface IObserver
    {
        void Update(int deltaX, int deltaY);
    }
}

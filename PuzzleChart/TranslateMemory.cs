﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart
{
    public class TranslateMemory
    {
        public int xBefore;
        public int yBefore;
        public int xAmount;
        public int yAmount;
        public int xAmountRedo;
        public int yAmountRedo;
        public bool flag;

        public TranslateMemory()
        {
            xBefore = 0;
            yBefore = 0;
            xAmount = 0;
            yAmount = 0;
            xAmountRedo = 0;
            yAmountRedo = 0;
            flag = false;
        }
    }
}

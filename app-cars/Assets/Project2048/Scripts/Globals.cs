/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project2048
{
    class Globals
    {
        public readonly static int Height = 4;
        public readonly static int Width = 4;
        public static readonly float AnimationDuration = 0.05f;

        public const string ID_TILE = "{0}x{1}";
        public const string ID_MOVE = "Move {0} {1}";
        public const string ID_UNDO = "Undo Button";
        public const string ID_RESET = "Reset Button";
        public const string ID_START = "Start Button";
        public const string ID_QUIT = "Quit Button";
    }
}

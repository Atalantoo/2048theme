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
        // VARS *********************************

        public static int Height = 4;
        public static int Width = 4;
        public static int LEVEL_CURRENT = 0;
        public static string[] LEVELS = { "model_t", "f40" };

        // CONST *********************************

        public const string MAIN_SCENE = "Main";
        public const string GAME_SCENE = "Game";

        public const string ID_TILE = "{0}x{1}";
        public const string ID_MOVE = "Move {0} {1}";
        public const string ID_UNDO = "Undo Button";
        public const string ID_RESET = "Reset Button";
        public const string ID_START = "Start Button";
        public const string ID_QUIT = "Quit Button";
        public const string ID_LEVEL = "Level Toggle ({0})";
    }
}

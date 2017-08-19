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

        // TODO public static string PACKAGE_CURRENT = "Cars";
        public static int LEVEL_CURRENT = 0;
        public static int LEVELS_LENGTH = 2;

        // CONST *********************************

        public const string MAIN_SCENE = "Main";
        public const string GAME_SCENE = "Game";

        public const string ID_TILE = "{0}x{1}";
        public const string ID_MOVE = "Move {0} {1}";
        public const string ID_UNDO = "Undo Button";
        public const string ID_START = "Start Button";
        public const string ID_QUIT = "Quit Button";
        public const string ID_LEVEL = "Level Toggle ({0})";
        public const string ID_SCORE = "Score Text";
        public const string ID_BOARD = "Board";

        public static string[] SPRITES = new string[] { //
            "0000",
            "0002",
            "0004",
            "0008",
            "0016",
            "0032",
            "0064",
            "0128",
            "0256",
            "0512",
            "1024",
            "2048" };
    }
}

/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Globals
{
    // VARS *********************************

    public static int Height = 4;
    public static int Width = 4;
    public static int LEVEL_CURRENT = 0;
    public static int LEVELS_LENGTH = 2;

    // CONST *********************************

    public const string SCENE_MAP = "Main";
    public const string SCENE_GAME = "Game";

    public const string UICanvas = "UICanvas";
    public const string BackgroundSprite = "BackgroundSprite";

    public const string ConfirmButton = "ConfirmButton";
    public const string CancelButton = "CancelButton";
    public const string UndoButton = "UndoButton";
    public const string QuitButton = "QuitButton";
    public const string QuitDialog = "QuitDialog";
    public const string ScoreText = "ScoreText";

    public const string GAMEOBJECT_START = "Start Button";
    public const string GAMEOBJECT_LEVEL = "Level Toggle ({0})";
    public const string GAMEOBJECT_BOARD = "Board";
    public const string GAMEOBJECT_TILE = "{0}x{1}";
    public const string GAMEOBJECT_MOVE = "Move {0} {1}";

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


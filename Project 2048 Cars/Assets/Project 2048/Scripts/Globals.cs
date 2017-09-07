using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons.UI;

using UnityEngine;

class Globals
{
    // VARS *********************************

    public static int Height = 4;
    public static int Width = 4;
    public static int LEVEL_CURRENT = 0;
    public static int LEVEL_MAX = 9;

    // CONST *********************************

    public const string SCENE_MAP = "Map";
    public const string SCENE_GAME = "Game";

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

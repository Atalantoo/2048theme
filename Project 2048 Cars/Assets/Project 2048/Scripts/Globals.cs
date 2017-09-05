/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

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

    public static StyleProvider Theme = InitThemeProvider();
    public static StyleProvider InitThemeProvider()
    {
        StyleProvider t = new StyleProvider();
        t.Config("main", new Theme()
        {
            Text = ColorHelper.HEXToRGB("FFFFFF"),
            Background = ColorHelper.HEXToRGB("08C7C7")
        });
        t.Config("game", new Theme()
        {
            Text = ColorHelper.HEXToRGB("FFFFFF"),
            Background = ColorHelper.HEXToRGB("08C7C7")
        });
        return t;
    }

    public static TranslateUIProvider Lang = InitTranslateProvider();
    public static TranslateUIProvider InitTranslateProvider()
    {
        TranslateUIProvider t = new TranslateUIProvider();
        if (Application.systemLanguage.ISO() != null)
            t.PreferredLanguage = Application.systemLanguage.ISO();
        t.Initialize();
        return t;
    }

    public static Dictionary<string, string> save = InitSave();
    public static Dictionary<string, string> InitSave()
    {
        Dictionary<string, string> save = new Dictionary<string, string>();
        
        // STATIC
        save.Add("level_count", "4");
        save.Add("level_0_require", "0");
        save.Add("level_1_require", "1");
        save.Add("level_2_require", "3");

        // STORED
        save.Add("level_0_unlocked", "true");
        save.Add("level_0_tile_max", "0512");
        save.Add("level_0_achiv_0512", "true");
        save.Add("level_0_achiv_1024", "false");
        save.Add("level_0_achiv_2048", "false");

        save.Add("level_1_unlocked", "true");
        save.Add("level_1_tile_max", "0256");
        save.Add("level_1_achiv_0512", "false");

        // CALC
        save.Add("achivement_count", "1");
        return save;
    }
}

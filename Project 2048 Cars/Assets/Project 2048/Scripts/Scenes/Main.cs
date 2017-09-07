using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;


class Main
{
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


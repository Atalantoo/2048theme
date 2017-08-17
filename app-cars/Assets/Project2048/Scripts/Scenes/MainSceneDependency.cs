/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project2048.Scenes
{
    class MainSceneDependency
    {
        public static void InjectUI(MainScene main)
        {
            BindButtons(main);
        }

        private static void BindButtons(MainScene main)
        {
            GameObject go;
            Button btn;

            go = GameObject.Find(Globals.ID_START);
            btn = go.GetComponent<Button>();
            btn.onClick.AddListener(main.StartAction);
        }
    }
}

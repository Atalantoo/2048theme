/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Project2048.Scenes
{
    class MainSceneDelegate
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
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(main.StartAction);

            Toggle tog;
            for (int i = 0; i < Globals.LEVELS_LENGTH; i++)
            {
                go = GameObject.Find(String.Format(Globals.ID_LEVEL, i+1));
                tog = go.GetComponent<Toggle>();
                tog.isOn = false;
                tog.onValueChanged.AddListener(main.SelectLevelAction);
            }
        }
    }
}

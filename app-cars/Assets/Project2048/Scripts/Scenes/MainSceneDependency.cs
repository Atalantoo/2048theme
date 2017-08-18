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
    class MainSceneDependency
    {
        public static void InjectUI(MainScene main)
        {
            BindButtons(main);
        }

        private static void BindButtons(MainScene main)
        {
            UnityAction[] actions = { main.SelectLevel0Action, main.SelectLevel1Action };

            GameObject go;
            Button btn;

            go = GameObject.Find(Globals.ID_START);
            btn = go.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(main.StartAction);

            for(int i = 0;  i< Globals.LEVELS.Length; i++)
            {
                go = GameObject.Find(String.Format(Globals.ID_LEVEL, i));
                btn = go.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                // TODO save
                if (i< actions.Length)
                {
                    btn.interactable = true;
                    btn.onClick.AddListener(actions[i]);
                }
                else
                {
                    btn.interactable = false;
                }
            }
        }
    }
}

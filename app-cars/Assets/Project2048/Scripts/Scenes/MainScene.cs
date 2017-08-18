/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Project2048.Scenes
{
    class MainScene : MonoBehaviour
    {
        void Start()
        {
            Globals.LEVEL_CURRENT = 0;
            MainSceneDependency.InjectUI(this);
            UpdateScreen();
        }

        // ********************************************************************

        internal void SelectLevel0Action()
        {
            Globals.LEVEL_CURRENT = 0;
            UpdateScreen();
        }

        internal void SelectLevel1Action()
        {
            Globals.LEVEL_CURRENT = 1;
            UpdateScreen();
        }

        internal void StartAction()
        {
            SceneManager.LoadScene(Globals.GAME_SCENE, LoadSceneMode.Single);
        }

        // ********************************************************************

        private void UpdateScreen()
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            GameObject go;
            Button btn;
            for (int i = 0; i < Globals.LEVELS.Length; i++)
            {
                go = GameObject.Find(String.Format(Globals.ID_LEVEL, i));
                btn = go.GetComponent<Button>();
                if (i > 1) // TODO not reachable
                    btn.interactable = false;
                else if (Globals.LEVEL_CURRENT == i)
                    btn.interactable = false;
                else
                    btn.interactable = true;
            }
        }
    }
}

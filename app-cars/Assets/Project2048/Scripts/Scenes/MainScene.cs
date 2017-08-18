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
            SetToggle();
            UpdateScreen();
        }

        // ********************************************************************

        internal void SelectLevelAction(bool arg)
        {
            GetToggle();
            UpdateScreen();
        }

        internal void StartAction()
        {
            GetToggle();
            SceneManager.LoadScene(Globals.GAME_SCENE, LoadSceneMode.Single);
        }

        // ********************************************************************

        private void UpdateScreen()
        {
            // UpdateButtons();
        }

        private void SetToggle()
        {
            GameObject go;
            Toggle tog;
            for (int i = 0; i < Globals.LEVELS.Length; i++)
            {
                go = GameObject.Find(String.Format(Globals.ID_LEVEL, i+1));
                tog = go.GetComponent<Toggle>();
                tog.isOn = (i == Globals.LEVEL_CURRENT);
            }
        }

        private void GetToggle()
        {
            GameObject go;
            Toggle tog;
            for (int i = 0; i < Globals.LEVELS.Length; i++)
            {
                go = GameObject.Find(String.Format(Globals.ID_LEVEL, i+1));
                tog = go.GetComponent<Toggle>();
                if (tog.isOn)
                    Globals.LEVEL_CURRENT = i;
            }
        }
    }
}

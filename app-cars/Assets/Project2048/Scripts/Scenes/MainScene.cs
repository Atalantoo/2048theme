/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project2048.Scenes
{
    class MainScene : MonoBehaviour
    {
        void Start()
        {
            MainSceneDependency.InjectUI(this);
        }

        // ***************************

        internal void StartAction()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}

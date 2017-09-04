/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class MainScene : MonoBehaviour
{
    public MainSceneView View;

    void Start()
    {
        Globals.LEVEL_CURRENT = 0;
        MainSceneDelegate.InjectUI(this);
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
        FindLevels();
        GetToggle();
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }


    // ********************************************************************

    private void FindLevels()
    {

        Globals.LEVELS_LENGTH = 2;
    }

    private void UpdateScreen()
    {
        // UpdateButtons();
    }

    private void SetToggle()
    {
        GameObject go;
        Toggle tog;
        for (int i = 0; i < Globals.LEVELS_LENGTH; i++)
        {
            go = GameObject.Find(String.Format(Globals.GAMEOBJECT_LEVEL, i + 1));
            tog = go.GetComponent<Toggle>();
            tog.isOn = (i == Globals.LEVEL_CURRENT);
        }
    }

    private void GetToggle()
    {
        GameObject go;
        Toggle tog;
        for (int i = 0; i < Globals.LEVELS_LENGTH; i++)
        {
            go = GameObject.Find(String.Format(Globals.GAMEOBJECT_LEVEL, i + 1));
            tog = go.GetComponent<Toggle>();
            if (tog.isOn)
                Globals.LEVEL_CURRENT = i;
        }
    }
}


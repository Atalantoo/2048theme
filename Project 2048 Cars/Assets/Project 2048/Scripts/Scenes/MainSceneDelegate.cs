/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Commons.UI;

class MainSceneDelegate
{
    public static void InjectUI(MainScene scene)
    {
        scene.View = new MainSceneView();
        scene.View.UICanvas = GameObject.Find("UICanvas");
        BindButtons(scene);
        ApplyTranslation(scene);
        InitAnimations(scene);
    }

    private static void BindButtons(MainScene scene)
    {
        GameObject go;
        Button btn;
        go = GameObject.Find(Globals.GAMEOBJECT_START);
        btn = go.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(scene.StartAction);

        /*
        Toggle tog;
        for (int i = 0; i < Globals.LEVELS_LENGTH; i++)
        {
            go = GameObject.Find(String.Format(Globals.GAMEOBJECT_LEVEL, i + 1));
            tog = go.GetComponent<Toggle>();
            tog.isOn = false;
            tog.onValueChanged.AddListener(scene.SelectLevelAction);
        }
        */
    }

    private static void ApplyTranslation(MainScene scene)
    {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Globals.Lang.Apply(scene.View.UICanvas);
        long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log("ApplyTranslation in " + (end - start) + " ms");
    }

    private static void InitAnimations(MainScene scene)
    {
        GameObject.Find("StartButton").AddComponent<FocusAnimator>();
    }
}


/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;

using Commons;
using Commons.UI;

class MainSceneDelegate
{
    public static void InjectDependencies(MainScene scene)
    {
        GameObject UICanvas = GameObject.Find("UICanvas");
        GameObject LevelTogglePrefab = UICanvas.FindChild("LevelTogglePrefab", true);
        scene.View = new MainSceneView()
        {
            UICanvas = UICanvas,
            LevelTogglePrefab = LevelTogglePrefab,
            StartButton = GameObject.Find("StartButton")
        };
    }

    public static void InitializeUI(MainScene scene)
    {
        BindButtons(scene);
        ApplyTranslation(scene);
        InitAnimations(scene);
    }

    // **************************************************************

    private static void BindButtons(MainScene scene)
    {
        scene.View.StartButton.OnClick(scene.StartAction);

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


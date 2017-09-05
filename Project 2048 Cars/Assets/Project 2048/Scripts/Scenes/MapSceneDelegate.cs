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

class MapSceneDelegate
{
    public static void InjectDependencies(MapScene scene)
    {
        GameObject UICanvas = GameObject.Find("UICanvas");
        GameObject LevelTogglePrefab = UICanvas.FindChild("LevelTogglePrefab", true);
        GameObject SettingsDialog = UICanvas.FindChild("SettingsDialog", true);
        scene.View = new MaSceneView()
        {
            UICanvas = UICanvas,
            BackgroundImage = GameObject.Find("BackgroundImage"),
            LevelTogglePrefab = LevelTogglePrefab,
            AchiementCountText = GameObject.Find("AchiementCountText"),
            StartButton = GameObject.Find("StartButton"),
            LevelDescription = GameObject.Find("LevelDescription"),
            LevelDescriptionIndexText = GameObject.Find("LevelDescriptionIndexText"),
            LevelDescriptionImage = GameObject.Find("LevelDescriptionImage"),
            SettingsButton = GameObject.Find("SettingsButton"),
            SettingsDialog = SettingsDialog,
            SettingsCloseButton = SettingsDialog.FindChild("CloseButton")
        };
    }

    public static void InitializeUI(MapScene scene)
    {
        InitDialogs(scene);
        BindButtons(scene);
        ApplyTheme(scene);
        ApplyTranslation(scene);
        InitAnimations(scene);
    }

    // **************************************************************

    private static void InitDialogs(MapScene scene)
    {
        scene.View.SettingsDialog.AddComponent<BringToFront>();
        scene.View.SettingsDialog.SetActive(false);
    }

    private static void BindButtons(MapScene scene)
    {
        scene.View.StartButton.OnClick(scene.StartAction);
        scene.View.SettingsButton.OnClick(scene.SettingsOpenAction);
        scene.View.SettingsCloseButton.OnClick(scene.SettingsCloseAction);
    }

    private static void ApplyTheme(MapScene scene)
    {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Globals.Theme.Apply(scene.View.UICanvas);

        // TODO auto
        scene.View.BackgroundImage.GetComponent<Image>().color = Globals.Theme.themes["main"].Background;

        long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log("ApplyTheme in " + (end - start) + " ms");
    }

    private static void ApplyTranslation(MapScene scene)
    {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Globals.Lang.Apply(scene.View.UICanvas);
        long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log("ApplyTranslation in " + (end - start) + " ms");
    }

    private static void InitAnimations(MapScene scene)
    {
        GameObject.Find("StartButton").AddComponent<FocusAnimator>();
    }
}


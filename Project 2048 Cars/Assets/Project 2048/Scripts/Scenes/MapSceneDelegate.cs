using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using Commons;
using Commons.UI;
using SimpleJSON;

class MapSceneDelegate
{
    public static void InjectDependencies(MapScene scene)
    {
        GameObject UICanvas = GameObject.Find("UICanvas");
        scene.View = new MaSceneView()
        {
            UICanvas = UICanvas,
            BackgroundImage = GameObject.Find("BackgroundImage"),

            AchiementCountText = GameObject.Find("AchiementCountText"),

            LevelTogglePrefab = UICanvas.FindChild("LevelTogglePrefab", true),
            StartButton = GameObject.Find("StartButton"),
            LevelDescription = GameObject.Find("LevelDescription"),
            LevelDescriptionIndexText = GameObject.Find("LevelDescriptionIndexText"),
            LevelDescriptionImage = GameObject.Find("LevelDescriptionImage"),

            SettingsButton = GameObject.Find("SettingsButton"),
            Settings = GameObject.Find("Settings"),
        };
    }

    public static void InitializeUI(MapScene scene)
    {
        scene.View.SettingsButton.OnClick(scene.SettingsOpenAction);
        scene.View.Settings.AddComponent<Settings>();
        scene.View.Settings.GetComponent<Settings>().TranslateTarget = scene.View.UICanvas;

        scene.View.StartButton.OnClick(scene.StartAction);

        Main.Lang.Apply(scene.View.UICanvas);
        Main.Theme.Apply(scene.View.UICanvas);
        scene.View.BackgroundImage.GetComponent<Image>().color = Main.Theme.themes["main"].Background;

        scene.View.StartButton.AddComponent<FocusAnimator>();
    }

}


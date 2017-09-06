/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

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
        GameObject LevelTogglePrefab = UICanvas.FindChild("LevelTogglePrefab", true);
        GameObject SettingsDialog = UICanvas.FindChild("SettingsDialog", true);
        GameObject LanguageDialog = UICanvas.FindChild("LanguageDialog", true);
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
            SettingsCloseButton = SettingsDialog.FindChild("CloseButton"),
            LanguageButton = SettingsDialog.FindChild("LanguageButton"),
            LanguageDialog = LanguageDialog,
            LanguageCloseButton = LanguageDialog.FindChild("CloseButton"),
            LanguageTogglePrefab = LanguageDialog.FindChild("LanguageTogglePrefab", true),
            LanguageValueText = LanguageDialog.FindChild("LanguageValueText"),
        };
    }

    public static void InitializeUI(MapScene scene)
    {
        InitDialogs(scene);
        BuildLanguageList(scene);
        BindButtons(scene);
        ApplyTheme(scene);
        ApplyTranslation(scene);
        InitAnimations(scene);
    }

    // **************************************************************

    private static void BuildLanguageList(MapScene scene)
    {
        string current = Globals.Lang.PreferredLanguage;
        Language[] langs = Globals.Lang.Languages;

        // current
        string current_label = Globals.Lang.Language(current).label;
        scene.View.LanguageValueText.GetComponent<Text>().text = current_label;

        // load prefab
        GameObject original = scene.View.LanguageTogglePrefab;
        Transform parent = original.transform.parent;
        original.SetActive(false);

        // populate
        int count = langs.Length;
        scene.View.LanguageToggles = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            Language lang = langs[i];
            string code = lang.code;
            string label = lang.label;
            bool status = current.Equals(code);

            GameObject go = GameObject.Instantiate(original, parent);
            go.name = code;
            go.GetComponentInChildren<Text>().text = label;
            go.GetComponent<Toggle>().isOn = status;
            go.GetComponent<Toggle>().onValueChanged.AddListener(scene.LanguageSelectedAction);
            go.SetActive(true);

            scene.View.LanguageToggles[i] = go;
        }
    }

    private static void InitDialogs(MapScene scene)
    {
        scene.View.SettingsDialog.AddComponent<BringToFront>();
        scene.View.SettingsDialog.SetActive(false);
        scene.View.LanguageDialog.AddComponent<BringToFront>();
        scene.View.LanguageDialog.SetActive(false);
    }

    private static void BindButtons(MapScene scene)
    {
        scene.View.StartButton.OnClick(scene.StartAction);
        scene.View.SettingsButton.OnClick(scene.SettingsOpenAction);
        scene.View.SettingsCloseButton.OnClick(scene.SettingsCloseAction);
        scene.View.LanguageButton.OnClick(scene.LanguageOpenAction);
        scene.View.LanguageCloseButton.OnClick(scene.LanguageCloseAction);
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

    public static void ApplyTranslation(MapScene scene)
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


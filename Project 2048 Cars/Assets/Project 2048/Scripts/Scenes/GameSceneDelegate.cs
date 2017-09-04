/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Commons;
using Commons.UI;
using Commons.Inputs;
using Commons.Lang;
using Commons.Animations;
using Project2048;
using System.IO;

class GameSceneDelegate
{
    public static void InjectCore(GameScene scene)
    {
        scene.Core = new GameManager();
        scene.View = new GameSceneView();
        scene.TileSprites = new Dictionary<int, Sprite>();
    }

    public static void InjectViewGame(GameScene scene)
    {
        ReferenceTiles(scene);
        ReferenceMoves(scene);
        InitAnimations(scene);
    }

    public static void InjectViewUI(GameScene scene)
    {
        scene.View = new GameSceneView();
        scene.View.Camera = GameObject.Find("Main Camera");
        scene.View.UICanvas = GameObject.Find("UICanvas");
        scene.View.BackgroundSprite = GameObject.Find("BackgroundSprite");
        scene.View.WallpaperSprite = GameObject.Find("WallpaperSprite");
        scene.View.ScoreValue = GameObject.Find("ScoreText");
        scene.View.Completion = GameObject.Find("Completion");
        scene.View.CompletionValue = scene.View.Completion.FindChild("Text");
        scene.View.UndoButton = GameObject.Find("UndoButton");
        scene.View.QuitButton = GameObject.Find("QuitButton");
        scene.View.QuitDialog = scene.View.UICanvas.FindChild("QuitDialog", true);
        scene.View.QuitConfirmButton = scene.View.QuitDialog.FindChild("ConfirmButton");
        scene.View.QuitCancelButton = scene.View.QuitDialog.FindChild("CancelButton");
        scene.View.MergeAnimation = scene.View.UICanvas.FindChild("MergeAnimation", true);
        InitDialogs(scene);
        BindActions(scene);
        InitInputs(scene);
        LoadColor();
        LoadBackground(scene);
        ApplyTheme(scene);
        ApplyTranslation(scene);
    }

    // *************************************

    private static void LoadColor()
    {
        string path = Globals.LEVEL_CURRENT.ToString() + "/" + "data";
        TextAsset txt = Resources.Load<TextAsset>(path);
        string dataAsJson = txt.text;
        LevelData loadedData = JsonUtility.FromJson<LevelData>(dataAsJson);
        Color color = ColorHelper.HEXToRGB(loadedData.color);
        Globals.Theme.themes["game"].Background = color;
    }

    private static void LoadBackground(GameScene scene)
    {
        string path = Globals.LEVEL_CURRENT.ToString() + "/" + "wallpaper";
        Sprite sprite = Resources.Load<Sprite>(path);
        scene.View.WallpaperSprite.GetComponent<SpriteRenderer>().sprite = sprite;
        scene.View.BackgroundSprite.GetComponent<SpriteRenderer>().color = Globals.Theme.themes["game"].Background;
    }

    private static void ApplyTheme(GameScene scene)
    {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Globals.Theme.Apply(scene.View.UICanvas);

        scene.View.Completion.GetComponent<Image>().color = Globals.Theme.themes["game"].Text;
        scene.View.CompletionValue.GetComponent<Text>().color = Globals.Theme.themes["game"].Background;

        long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log("ApplyTheme in " + (end - start) + " ms");
    }

    private static void ApplyTranslation(GameScene scene)
    {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Globals.Lang.Apply(scene.View.UICanvas);
        long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log("ApplyTranslation in " + (end - start) + " ms");
    }

    private static void InitAnimations(GameScene scene)
    {
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            GameObject.Find(String.Format(Globals.GAMEOBJECT_MOVE, move, true))
                .AddComponent<BlinkAnimator>();
    }

    private static void ReferenceTiles(GameScene scene)
    {
        scene.View.TileObjects = new Dictionary<string, GameObject>();
        for (int y = 0; y < Globals.Height; y++)
            for (int x = 0; x < Globals.Width; x++)
            {
                string name = String.Format(Globals.GAMEOBJECT_TILE, y, x);
                scene.View.TileObjects.Add(name, GameObject.Find(name));
            }
    }

    private static void ReferenceMoves(GameScene scene)
    {
        scene.View.GameMoves = new Dictionary<string, SpriteRenderer>();
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            foreach (bool b in new bool[] { true, false })
            {
                string name = String.Format(Globals.GAMEOBJECT_MOVE, move, b);
                var sr = GameObject.Find(name).GetComponent<SpriteRenderer>();
                scene.View.GameMoves.Add(name, sr);
            }
    }

    private static void InitDialogs(GameScene scene)
    {
        scene.View.QuitDialog.AddComponent<BringToFront>();
        scene.View.QuitDialog.SetActive(false);
    }

    private static void InitInputs(GameScene scene)
    {
        GameObject go;
        InputDetector input;

        go = GameObject.Find(Globals.GAMEOBJECT_BOARD);

        input = go.AddComponent<KeysArrowDetector>();
        input.Left = scene.MoveLeftAction;
        input.Right = scene.MoveRightAction;
        input.Up = scene.MoveUpAction;
        input.Down = scene.MoveDownAction;
#if UNITY_EDITOR
        input = go.AddComponent<MouseSwipeDetector>();
        input.Left = scene.MoveLeftAction;
        input.Right = scene.MoveRightAction;
        input.Up = scene.MoveUpAction;
        input.Down = scene.MoveDownAction;
#endif
#if UNITY_ANDROID
        input = go.AddComponent<TouchSwipeDetector>();
        input.Left = scene.MoveLeftAction;
        input.Right = scene.MoveRightAction;
        input.Up = scene.MoveUpAction;
        input.Down = scene.MoveDownAction;
#endif
    }

    private static void BindActions(GameScene scene)
    {
        ButtonOnClick(scene.View.UndoButton, scene.UndoAction);
        ButtonOnClick(scene.View.QuitButton, scene.QuitAction);
        ButtonOnClick(scene.View.QuitConfirmButton, scene.QuitConfirmAction);
        ButtonOnClick(scene.View.QuitCancelButton, scene.QuitCancelAction);
    }

    private static void ButtonOnClick(GameObject go, UnityAction action)
    {
        Button btn;
        btn = go.GetComponent<Button>();
        btn.onClick.AddListener(action);
    }
}


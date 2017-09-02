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
    public static void InjectCore(GameScene main)
    {
        main.Core = new GameManager();
        main.View = new GameSceneView();
        main.TileSprites = new Dictionary<int, Sprite>();
    }

    public static void InjectViewGame(GameScene main)
    {
        ReferenceTiles(main);
        ReferenceMoves(main);
        InitAnimations(main);
    }

    public static void InjectViewUI(GameScene main)
    {
        main.View.ScoreText = GameObject.Find(Globals.ScoreText);
        main.View.BackgroundSprite = GameObject.Find(Globals.BackgroundSprite);
        main.View.UndoButton = GameObject.Find(Globals.UndoButton);
        InitDialogs(main);
        BindActions(main);
        InitInputs(main);
        LoadColor();
        ApplyTheme(main);
        Contract.Requires<ArgumentNullException>(main.View.ScoreText != null);
        Contract.Requires<ArgumentNullException>(main.View.BackgroundSprite != null);
        Contract.Requires<ArgumentNullException>(main.View.UndoButton != null);
        Contract.Requires<ArgumentNullException>(main.View.QuitDialog != null);
    }

    // *************************************

    private static void LoadColor()
    {
        string filePath = Globals.LEVEL_CURRENT.ToString() + "/" + "data";
        TextAsset txt = Resources.Load<TextAsset>(filePath);
        string dataAsJson = txt.text;
        LevelData loadedData = JsonUtility.FromJson<LevelData>(dataAsJson);
        Color color = ColorHelper.HEXToRGB(loadedData.color);
        Globals.Theme.themes["game"].Background = color;
    }

    private static void ApplyTheme(GameScene main)
    {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        main.View.BackgroundSprite.GetComponent<SpriteRenderer>().color = Globals.Theme.themes["game"].Background;
        Globals.Theme.Apply(GameObject.Find("UICanvas"));
        long end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log("Theme applied in " + (end - start) + " ms");
    }

    private static void InitAnimations(GameScene main)
    {
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            GameObject.Find(String.Format(Globals.GAMEOBJECT_MOVE, move, true))
                .AddComponent<BlinkAnimator>();
    }

    private static void ReferenceTiles(GameScene main)
    {
        main.View.TileObjects = new Dictionary<string, GameObject>();
        for (int y = 0; y < Globals.Height; y++)
            for (int x = 0; x < Globals.Width; x++)
            {
                string name = String.Format(Globals.GAMEOBJECT_TILE, y, x);
                main.View.TileObjects.Add(name, GameObject.Find(name));
            }
    }

    private static void ReferenceMoves(GameScene main)
    {
        main.View.GameMoves = new Dictionary<string, SpriteRenderer>();
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            foreach (bool b in new bool[] { true, false })
            {
                string name = String.Format(Globals.GAMEOBJECT_MOVE, move, b);
                var sr = GameObject.Find(name).GetComponent<SpriteRenderer>();
                main.View.GameMoves.Add(name, sr);
            }
    }

    private static void InitDialogs(GameScene main)
    {
        main.View.QuitDialog = GameObject
            .Find(Globals.UICanvas)
            .FindChild(Globals.QuitDialog, true);
        main.View.QuitDialog.AddComponent<BringToFront>();
        main.View.QuitDialog.SetActive(false);
    }

    private static void InitInputs(GameScene main)
    {
        GameObject go;
        InputDetector input;

        go = GameObject.Find(Globals.GAMEOBJECT_BOARD);

        input = go.AddComponent<KeysArrowDetector>();
        input.Left = main.MoveLeftAction;
        input.Right = main.MoveRightAction;
        input.Up = main.MoveUpAction;
        input.Down = main.MoveDownAction;
#if UNITY_EDITOR
        input = go.AddComponent<MouseSwipeDetector>();
        input.Left = main.MoveLeftAction;
        input.Right = main.MoveRightAction;
        input.Up = main.MoveUpAction;
        input.Down = main.MoveDownAction;
#endif
#if UNITY_ANDROID
        input = go.AddComponent<TouchSwipeDetector>();
        input.Left = main.MoveLeftAction;
        input.Right = main.MoveRightAction;
        input.Up = main.MoveUpAction;
        input.Down = main.MoveDownAction;
#endif
    }

    private static void BindActions(GameScene main)
    {
        ButtonOnClick(main.View.UndoButton, main.UndoAction);
        ButtonOnClick(GameObject.Find(Globals.QuitButton), main.QuitAction);
        ButtonOnClick(main.View.QuitDialog.FindChild(Globals.ConfirmButton), main.QuitConfirmAction);
        ButtonOnClick(main.View.QuitDialog.FindChild(Globals.CancelButton), main.QuitCancelAction);
    }

    private static void ButtonOnClick(GameObject go, UnityAction action)
    {
        Button btn;
        btn = go.GetComponent<Button>();
        btn.onClick.AddListener(action);
    }
}


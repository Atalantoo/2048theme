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
    public static void InjectDependencies(GameScene scene)
    {
        scene.Core = new GameManager();
        scene.TileSprites = new Dictionary<int, Sprite>();
        GameObject UICanvas = GameObject.Find("UICanvas");
        GameObject QuitDialog = UICanvas.FindChild("QuitDialog", true);
        scene.View = new GameSceneView()
        {
            UICanvas = UICanvas,
            Camera = GameObject.Find("Main Camera"),
            BackgroundSprite = GameObject.Find("BackgroundSprite"),
            WallpaperSprite = GameObject.Find("WallpaperSprite"),
            MergeAnimation = UICanvas.FindChild("MergeAnimation", true),

            ScoreValue = GameObject.Find("ScoreText"),
            Completion = GameObject.Find("Completion"),
            CompletionValue = GameObject.Find("CompletionText"),
            LevelCurrentText = GameObject.Find("LevelCurrentText"),

            UndoButton = GameObject.Find("UndoButton"),

            QuitButton = GameObject.Find("QuitButton"),
            QuitDialog = QuitDialog,
            QuitConfirmButton = QuitDialog.FindChild("ConfirmButton"),
            QuitCancelButton = QuitDialog.FindChild("CancelButton"),

            SettingsButton = GameObject.Find("SettingsButton"),
            Settings = GameObject.Find("Settings"),
        };
    }

    public static void InitializeCore(GameScene scene)
    {
        // TODO
    }

    public static void InitializeUI(GameScene scene)
    {
        scene.View.SettingsButton.OnClick(scene.SettingsOpenAction);
        scene.View.Settings.AddComponent<Settings>();
        scene.View.Settings.GetComponent<Settings>().TranslateTarget = scene.View.UICanvas;
        if (SaveProdiver.GetSaveInt("achivement_count") < 2)
            scene.View.Settings.GetComponent<Settings>().DisplayHelpAtStartup = true;

        InitDialogs(scene);
        BindActions(scene);
        InitInputs(scene);
        LoadColor();
        LoadBackground(scene);

        Main.Lang.Apply(scene.View.UICanvas);
        Main.Theme.Apply(scene.View.UICanvas);
        scene.View.Completion.GetComponent<Image>().color = Main.Theme.themes["game"].Text;
        scene.View.CompletionValue.GetComponent<Text>().color = Main.Theme.themes["game"].Background;

        scene.View.LevelCurrentText.GetComponent<Text>().text = (Globals.LEVEL_CURRENT + 1).ToString();
    }

    public static void InitializeGame(GameScene scene)
    {
        ReferenceTiles(scene);
        ReferenceMoves(scene);
        InitAnimations(scene);
    }

    // **************************************************************

    private static void LoadColor()
    {
        string path = Globals.LEVEL_CURRENT.ToString() + "/" + "data";
        TextAsset txt = Resources.Load<TextAsset>(path);
        string dataAsJson = txt.text;
        LevelData loadedData = JsonUtility.FromJson<LevelData>(dataAsJson);
        Color color = ColorHelper.HEXToRGB(loadedData.color);
        Main.Theme.themes["game"].Background = color;
    }

    private static void LoadBackground(GameScene scene)
    {
        string path = Globals.LEVEL_CURRENT.ToString() + "/" + "wallpaper";
        Sprite sprite = Resources.Load<Sprite>(path);
        scene.View.WallpaperSprite.GetComponent<SpriteRenderer>().sprite = sprite;
        scene.View.BackgroundSprite.GetComponent<SpriteRenderer>().color = Main.Theme.themes["game"].Background;
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
        scene.View.UndoButton.OnClick(scene.UndoAction);
        scene.View.QuitButton.OnClick(scene.QuitAction);
        scene.View.QuitConfirmButton.OnClick(scene.QuitConfirmAction);
        scene.View.QuitCancelButton.OnClick(scene.QuitCancelAction);
    }

}


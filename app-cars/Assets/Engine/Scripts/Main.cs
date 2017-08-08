using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class InputDetectorAggregator: IInputDetector
{
    private IInputDetector[] inputDetectors;

    public InputDetectorAggregator(IInputDetector[] inputDetectors)
    {
        this.inputDetectors = inputDetectors;
    }

    public InputDirection? DetectInputDirection()
    {
        foreach (IInputDetector i in inputDetectors)
        {
            InputDirection? value = i.DetectInputDirection();
            if (value.HasValue)
                return value;
        }
        return null;
    }
}

public class Main : MonoBehaviour
{
    int Width = 4;
    int Height = 4;
    string levelName = "model_t";

    GameManager gameManager;
    Game game;
    Dictionary<int, Sprite> sprites;

    public IInputDetector inputDetector;

    void Start()
    {
        sprites = new Dictionary<int, Sprite>();
        gameManager = new GameManager();
        inputDetector = new InputDetectorAggregator(GetComponents<IInputDetector>());;

        BindButtons();
        LoadResources();
        StartGame();
        UpdateScreen();
    }

    void Update()
    {
//        if (game.State == GameState.Playing)
//        {
            InputDirection? value = inputDetector.DetectInputDirection();
            if (value.HasValue)
            {
                game = gameManager.Turn(new GameTurnInput()
                {
                    Move = (Movement)Enum.Parse(typeof(Movement), value.ToString())
                });

                UpdateScreen();
            }
//        }
    }
    // ***************************



    public void ResetAction()
    {
        LoadResources();
        StartGame();
        UpdateScreen();
    }

    public void BackAction()
    {

    }

    // ***************************

    private void BindButtons()
    {
        GameObject go;
        Button btn;
        go = GameObject.Find("Button RESET");
        btn = go.GetComponent<Button>();
        btn.onClick.AddListener(ResetAction);
        go = GameObject.Find("botmove1");
        go.AddComponent<BlinkAnimator>();
    }

    private void LoadResources()
    {
        LoadSprites();
    }

    private void StartGame()
    {
        GameStartInput startInput = new GameStartInput()
        {
            Height = Height,
            Width = Width
        };
        game = gameManager.Start(startInput);
    }


    private void UpdateScreen()
    {
        int item;
        GameObject itemGO;
        SpriteRenderer spriteRend;
        Sprite sprite;

        for (int y = 0; y < Height; y++)
        {
            string row = "";
            for (int x = 0; x < Width; x++)
            {
                row += game.Board[y, x];
            }
        }
        // TODO sprites
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                item = game.Board[y, x].Value;
                sprite = sprites[item];
                itemGO = GameObject.Find(y + "x" + x);
                spriteRend = itemGO.GetComponent<SpriteRenderer>();
                spriteRend.sprite = sprite;
            }
        // TODO moves
        // TODO score
    }

    // **************************

    static string[] suite = new string[] { //
            "0000",
            "0002",
            "0004",
            "0008",
            "0016",
            "0032",
            "0064",
            "0128",
            "0256",
            "0512",
            "1024",
            "2048" };

    private void LoadSprites()
    {
        sprites.Clear();
        foreach (string i in suite)
            sprites.Add(Int16.Parse(i), Resources.Load<Sprite>(levelName + "/" + i));
    }

    // DEBUG **************************

    void OnGUI()
    {
        if (GUILayout.Button("Start at Level 1"))
            LevelAction("model_t");
        if (GUILayout.Button("Start at Level 2"))
            LevelAction("f40");
        if (GUILayout.Button("Win 512"))
            throw new NotImplementedException();
        if (GUILayout.Button("Win 1024"))
            throw new NotImplementedException();
        if (GUILayout.Button("Win 2048"))
            throw new NotImplementedException();
    }

    private void LevelAction(string newLevelName)
    {
        levelName = newLevelName;
        LoadResources();
        game.Board[0, 3].Value = 2048;
        game.Board[0, 2].Value = 1024;
        game.Board[0, 1].Value = 512;
        game.Board[0, 0].Value = 256;
        game.Board[1, 0].Value = 128;
        game.Board[1, 1].Value = 64;
        game.Board[1, 2].Value = 32;
        game.Board[1, 3].Value = 16;
        game.Board[2, 3].Value = 8;
        game.Board[2, 2].Value = 4;
        game.Board[2, 1].Value = 2;
        UpdateScreen();
    }

}

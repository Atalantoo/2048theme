using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Main : MonoBehaviour
{
    int Width = 4;
    int Height = 4;
    float SpriteWidth = 300f;
    float SpriteHeight = 775f;
    string levelName = "model_t";

    GameManager gameManager;
    Game game;
    Dictionary<int, Sprite> sprites;

    public Main()
    {
        sprites = new Dictionary<int, Sprite>();
        gameManager = new GameManager();
    }

    void Start()
    {
        BindButtons();
        LoadResources();
        StartGame();
        UpdateScreen();
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

    public void Level1Action()
    {
        LevelAction("model_t");
    }

    public void Level2Action()
    {
        LevelAction("f40");
    }

    // ***************************

    private void BindButtons()
    {
        GameObject go;
        Button btn;

        go = GameObject.Find("Button RESET");
        btn = go.GetComponent<Button>();
        btn.onClick.AddListener(ResetAction);

        go = GameObject.Find("Button LVL 1");
        btn = go.GetComponent<Button>();
        btn.onClick.AddListener(Level1Action);

        go = GameObject.Find("Button LVL 2");
        btn = go.GetComponent<Button>();
        btn.onClick.AddListener(Level2Action);
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

        // TODO sprites
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                item = game.Board[y, x];
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

    private void LevelAction(string newLevelName)
    {
        levelName = newLevelName;
        LoadResources();
        game.Board[0, 3] = 2048;
        game.Board[0, 2] = 1024;
        game.Board[0, 1] = 512;
        game.Board[0, 0] = 256;
        game.Board[1, 0] = 128;
        game.Board[1, 1] = 64;
        game.Board[1, 2] = 32;
        game.Board[1, 3] = 16;
        game.Board[2, 3] = 8;
        game.Board[2, 2] = 4;
        game.Board[2, 1] = 2;
        UpdateScreen();
    }


}

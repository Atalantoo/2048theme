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
        LoadResources();
        StartGame();
        UpdateScreen();
    }

    void Update()
    {

    }

    // ***************************

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
            sprites.Add(Int16.Parse(i), Resources.Load<Sprite>(i));
    }

}

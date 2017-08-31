﻿/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Commons;
using Project2048;

class GameScene : MonoBehaviour
{
    public GameManager Core;
    public Game Model;
    public GameSceneView View;

    public Dictionary<int, Sprite> TileSprites;

    void Start()
    {
        GameSceneDelegate.InjectCore(this);
        GameSceneDelegate.InjectViewUI(this);
        GameSceneDelegate.InjectViewGame(this);
        ResetAction();
    }

    void Update()
    {
        // TODO OnScreenOrientationChange
        GameObjectUtils.ResizeViewToScreen(300, 600);
        GameObjectUtils.ResizeSpriteToScreen(View.BackgroundSprite);
    }

    // ********************************************************************

    internal void ResetAction()
    {
        LoadResources();
        StartGame();
        UpdateScreen();
    }

    internal void UndoAction()
    {
        Model = Core.Undo();
        UpdateScreen();
    }

    internal void MoveLeftAction()
    {
        Model = Core.Turn(new GameTurnInput() { Move = Movement.Left });
        UpdateScreen();
    }
    internal void MoveRightAction()
    {
        Model = Core.Turn(new GameTurnInput() { Move = Movement.Right });
        UpdateScreen();
    }
    internal void MoveUpAction()
    {
        Model = Core.Turn(new GameTurnInput() { Move = Movement.Up });
        UpdateScreen();
    }
    internal void MoveDownAction()
    {
        Model = Core.Turn(new GameTurnInput() { Move = Movement.Down });
        UpdateScreen();
    }

    internal void QuitAction()
    {
        View.QuitDialog.SetActive(true);
    }

    internal void QuitConfirmAction()
    {
        SceneManager.LoadScene(Globals.SCENE_MAP, LoadSceneMode.Single);
    }

    internal void QuitCancelAction()
    {
        View.QuitDialog.SetActive(false);
    }

    // ********************************************************************

    private void LoadResources()
    {
        LoadSprites();
    }

    private void StartGame()
    {
        GameStartInput startInput = new GameStartInput()
        {
            Height = Globals.Height,
            Width = Globals.Width
        };
        Model = Core.Start(startInput);
    }

    private void UpdateScreen()
    {
        UpdateSprites();
        UpdateButtons();
        UpdateMoves();
        UpdateScore();
    }

    private void UpdateScore()
    {
        Text txt;
        txt = View.ScoreText.GetComponent<Text>();
        txt.text = Model.Score.ToString();
    }

    private void UpdateMoves()
    {
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
        {
            bool state = this.Model.AvailableMoves.Contains(move);
            View.GameMoves[String.Format(Globals.GAMEOBJECT_MOVE, move, true)].enabled = state;
            View.GameMoves[String.Format(Globals.GAMEOBJECT_MOVE, move, false)].enabled = !state;
        }
    }

    private void UpdateSprites()
    {
        int item;
        GameObject itemGO;
        SpriteRenderer spriteRend;
        Sprite sprite;
        string name;
        for (int y = 0; y < Globals.Height; y++)
            for (int x = 0; x < Globals.Width; x++)
            {
                item = Model.Board[y, x].Value;
                sprite = TileSprites[item];
                name = String.Format(Globals.GAMEOBJECT_TILE, y, x);
                itemGO = View.TileObjects[name];
                spriteRend = itemGO.GetComponent<SpriteRenderer>();
                spriteRend.sprite = sprite;
            }
    }

    private void UpdateButtons()
    {
        View.UndoButton.GetComponent<Button>()
            .interactable = Model.CanUndo;
    }

    private void LoadSprites()
    {
        string path = Globals.LEVEL_CURRENT.ToString();
        TileSprites.Clear();
        foreach (string i in Globals.SPRITES)
            TileSprites.Add(Int16.Parse(i), Resources.Load<Sprite>(path + "/" + i));
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUILayout.Button("Start at Level 1"))
            LevelAction(0);
        if (GUILayout.Button("Start at Level 2"))
            LevelAction(1);
    }

    private void LevelAction(int newLevelName)
    {
        Globals.LEVEL_CURRENT = newLevelName;
        LoadResources();
        Model.Board[0, 3].Value = 2048;
        Model.Board[0, 2].Value = 1024;
        Model.Board[0, 1].Value = 512;
        Model.Board[0, 0].Value = 256;
        Model.Board[1, 0].Value = 128;
        Model.Board[1, 1].Value = 64;
        Model.Board[1, 2].Value = 32;
        Model.Board[1, 3].Value = 16;
        Model.Board[2, 3].Value = 8;
        Model.Board[2, 2].Value = 4;
        Model.Board[2, 1].Value = 2;
        UpdateScreen();
    }
#endif
}


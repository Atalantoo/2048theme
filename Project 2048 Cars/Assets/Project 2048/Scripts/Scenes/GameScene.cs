/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
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
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using Commons;
using Commons.Lang;
using Commons.UI;
using Project2048;

class GameScene : MonoBehaviour
{
    public GameManager Core;
    public Game Model;
    public GameSceneView View;

    public Dictionary<int, Sprite> TileSprites;

    void Start()
    {
        GameSceneDelegate.InjectDependencies(this);
        GameSceneDelegate.InitializeCore(this);
        GameSceneDelegate.InitializeUI(this);
        GameSceneDelegate.InitializeGame(this);
        ResetAction();
    }

    void Update()
    {
        // TODO OnScreenOrientationChange event
        GameObjectUtils.ResizeViewToScreen(350, 600);
        GameObjectUtils.ResizeSpriteToScreen(View.BackgroundSprite);
        GameObjectUtils.ResizeSpriteToScreenRight(View.WallpaperSprite);
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

        Transform parent = View.UICanvas.transform;
        Item item;
        GameObject go;
        GameObject it;
        string name;
        Camera cam = View.Camera.GetComponent<Camera>();
        for (int y = 0; y < Globals.Height; y++)
            for (int x = 0; x < Globals.Width; x++)
                if (Model.Board[y, x].HasBeenMerged)
                {
                    item = Model.Board[y, x];
                    name = String.Format(Globals.GAMEOBJECT_TILE, y, x);
                    it = View.TileObjects[name];
                    Vector3 screenPos = cam.WorldToScreenPoint(it.transform.position);

                    go = Instantiate(View.MergeAnimation, parent);
                    go.GetComponent<Text>().text = "+" + item.Value.ToString();
                    go.transform.position = screenPos;
                    go.SetActive(true);

                    Model.Board[y, x].HasBeenMerged = false;
                }
    }

    private void UpdateScore()
    {
        Text txt;
        txt = View.ScoreValue.GetComponent<Text>();
        txt.text = Model.Score.ToString();

        int tileMax = 2;
        for (int y = 0; y < Globals.Height; y++)
            for (int x = 0; x < Globals.Width; x++)
                if (tileMax < Model.Board[y, x].Value)
                    tileMax = Model.Board[y, x].Value;
        txt = View.CompletionValue.GetComponent<Text>();
        txt.text = tileMax.ToString();
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
        string path;
        TileSprites.Clear();
        foreach (string i in Globals.SPRITES)
        {
            path = Globals.LEVEL_CURRENT.ToString() + "/" + i;
            TileSprites.Add(Int16.Parse(i), Resources.Load<Sprite>(path));
        }
    }


#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUILayout.Button("Level 1 Completed"))
            LevelAction(0);
        if (GUILayout.Button("Level 2 Completed"))
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

public class LevelData
{
    public string color;
}


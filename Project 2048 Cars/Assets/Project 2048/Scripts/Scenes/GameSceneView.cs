using System;
using System.Collections.Generic;
using UnityEngine;

class GameSceneView
{
    public GameObject Camera;
    public GameObject UICanvas;
    public GameObject BackgroundSprite;
    public GameObject WallpaperSprite;

    public GameObject ScoreValue;
    public GameObject Completion;
    public GameObject CompletionValue;
    public GameObject LevelCurrentText;

    public GameObject UndoButton;
    public GameObject QuitDialog;
    public GameObject QuitButton;
    public GameObject QuitConfirmButton;
    public GameObject QuitCancelButton;

    public GameObject MergeAnimation;

    public Dictionary<string, GameObject> TileObjects;
    public Dictionary<string, SpriteRenderer> GameMoves;

    public GameObject SettingsButton;
    public GameObject Settings;
}

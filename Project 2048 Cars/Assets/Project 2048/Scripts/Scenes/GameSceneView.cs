using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class GameSceneView
{
    public GameObject QuitDialog;
    public GameObject UndoButton;
    public GameObject ScoreText;
    public GameObject BackgroundSprite;
    public Dictionary<string, GameObject> TileObjects;
    public Dictionary<string, SpriteRenderer> GameMoves;
}

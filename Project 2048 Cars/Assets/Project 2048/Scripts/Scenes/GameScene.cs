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
using Project2048;
using Project2048.Core;

namespace Project2048.Scenes
{
    class GameScene : MonoBehaviour
    {
        public GameManager gameManager;
        private Game state;

        // GameSceneReferences
        public GameObject dialog;
        public Dictionary<int, Sprite> sprites;

        void Start()
        {
            GameSceneDelegate.InjectCore(this);
            GameSceneDelegate.InjectUI(this);
            sprites = new Dictionary<int, Sprite>();
            ResetAction();
        }

        void Update()
        {
            // TODO OnScreenOrientationChange
            GameObjectUtils.ResizeViewToScreen(300, 600);
            GameObjectUtils.ResizeSpriteToScreen(GameObject.Find("Background Sprite"));
        }

        // ********************************************************************

        private void ResetAction()
        {
            LoadResources();
            StartGame();
            UpdateScreen();
        }

        internal void UndoAction()
        {
            state = gameManager.Undo();
            UpdateScreen();
        }

        internal void MoveLeftAction()
        {
            state = gameManager.Turn(new GameTurnInput() { Move = Movement.Left });
            UpdateScreen();
        }
        internal void MoveRightAction()
        {
            state = gameManager.Turn(new GameTurnInput() { Move = Movement.Right });
            UpdateScreen();
        }
        internal void MoveUpAction()
        {
            state = gameManager.Turn(new GameTurnInput() { Move = Movement.Up });
            UpdateScreen();
        }
        internal void MoveDownAction()
        {
            state = gameManager.Turn(new GameTurnInput() { Move = Movement.Down });
            UpdateScreen();
        }

        internal void QuitAction()
        {
            dialog.SetActive(true);
        }

        internal void QuitConfirmAction()
        {
            SceneManager.LoadScene(Globals.MAIN_SCENE, LoadSceneMode.Single);
        }

        internal void QuitCancelAction()
        {
            dialog.SetActive(false);
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
            state = gameManager.Start(startInput);
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
            GameObject go;
            Text txt;
            go = GameObject.Find(Globals.ID_SCORE);
            txt = go.GetComponent<Text>();
            txt.text = state.Score.ToString();
        }

        private void UpdateMoves()
        {
            foreach (Movement move in Enum.GetValues(typeof(Movement)))
            {
                bool state = this.state.AvailableMoves.Contains(move);
                Sprite(String.Format(Globals.ID_MOVE, move, true)).enabled = state;
                Sprite(String.Format(Globals.ID_MOVE, move, false)).enabled = !state;
            }
        }

        private SpriteRenderer Sprite(string str)
        {
            return GameObject.Find(str).GetComponent<SpriteRenderer>();
        }

        private void UpdateSprites()
        {
            int item;
            GameObject itemGO;
            SpriteRenderer spriteRend;
            Sprite sprite;
            for (int y = 0; y < Globals.Height; y++)
                for (int x = 0; x < Globals.Width; x++)
                {
                    item = state.Board[y, x].Value;
                    sprite = sprites[item];
                    itemGO = GameObject.Find(String.Format(Globals.ID_TILE, y, x));
                    spriteRend = itemGO.GetComponent<SpriteRenderer>();
                    spriteRend.sprite = sprite;
                }
        }

        private void UpdateButtons()
        {
            GameObject go;
            Button btn;
            go = GameObject.Find(Globals.ID_UNDO);
            btn = go.GetComponent<Button>();

            btn.interactable = state.CanUndo;
        }

        private void LoadSprites()
        {
            string path = Globals.LEVEL_CURRENT.ToString();
            sprites.Clear();
            foreach (string i in Globals.SPRITES)
                sprites.Add(Int16.Parse(i), Resources.Load<Sprite>(path + "/" + i));
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
            state.Board[0, 3].Value = 2048;
            state.Board[0, 2].Value = 1024;
            state.Board[0, 1].Value = 512;
            state.Board[0, 0].Value = 256;
            state.Board[1, 0].Value = 128;
            state.Board[1, 1].Value = 64;
            state.Board[1, 2].Value = 32;
            state.Board[1, 3].Value = 16;
            state.Board[2, 3].Value = 8;
            state.Board[2, 2].Value = 4;
            state.Board[2, 1].Value = 2;
            UpdateScreen();
        }
#endif
    }
}

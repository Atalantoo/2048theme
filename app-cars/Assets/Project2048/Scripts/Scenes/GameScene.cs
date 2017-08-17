/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Project2048.Core;
using UnityEngine.UI;
using System.Linq;

namespace Project2048.Scenes
{
    class GameScene : MonoBehaviour
    {
        public GameManager gameManager;

        private string levelName = "model_t";
        private Game game;
        private Dictionary<int, Sprite> sprites;

        void Start()
        {
            GameSceneDependency.InjectCore(this);
            GameSceneDependency.InjectUI(this);
            sprites = new Dictionary<int, Sprite>();
            ResetAction();
        }

        // ***************************

        public void ResetAction()
        {
            LoadResources();
            StartGame();
            UpdateScreen();
        }

        public void UndoAction()
        {
            game = gameManager.Undo();
            UpdateScreen();
        }

        public void MoveLeftAction()
        {
            game = gameManager.Turn(new GameTurnInput() { Move = Movement.Left });
            UpdateScreen();
        }
        public void MoveRightAction()
        {
            game = gameManager.Turn(new GameTurnInput() { Move = Movement.Right });
            UpdateScreen();
        }
        public void MoveUpAction()
        {
            game = gameManager.Turn(new GameTurnInput() { Move = Movement.Up });
            UpdateScreen();
        }
        public void MoveDownAction()
        {
            game = gameManager.Turn(new GameTurnInput() { Move = Movement.Down });
            UpdateScreen();
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
                Height = Globals.Height,
                Width = Globals.Width
            };
            game = gameManager.Start(startInput);
        }

        private void UpdateScreen()
        {
            UpdateSprites();
            UpdateButtons();
            UpdateMoves();
            // TODO score
        }

        private void UpdateMoves()
        {
            foreach (Movement move in Enum.GetValues(typeof(Movement)))
            {
                bool state = game.AvailableMoves.Contains(move);
                Sprite("move_" + move.ToString() + "_true").enabled = state;
                Sprite("move_" + move.ToString() + "_false").enabled = !state;
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
                    item = game.Board[y, x].Value;
                    sprite = sprites[item];
                    itemGO = GameObject.Find(y + "x" + x);
                    spriteRend = itemGO.GetComponent<SpriteRenderer>();
                    spriteRend.sprite = sprite;
                }
        }

        private void UpdateButtons()
        {
            GameObject go;
            Button btn;
            go = GameObject.Find("undo_button");
            btn = go.GetComponent<Button>();

            btn.interactable = game.CanUndo;
        }

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

#if UNITY_EDITOR
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
#endif
    }
}

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

        private Game game;
        private Dictionary<int, Sprite> sprites;

        void Start()
        {
            GameSceneDependency.InjectCore(this);
            GameSceneDependency.InjectUI(this);
            sprites = new Dictionary<int, Sprite>();
            ResetAction();
        }

        // ********************************************************************

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

        internal void QuitAction()
        {
            SceneManager.LoadScene(Globals.MAIN_SCENE, LoadSceneMode.Single);
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
            game = gameManager.Start(startInput);
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
            txt.text = "Score: " + game.Score.ToString();
        }

        private void UpdateMoves()
        {
            foreach (Movement move in Enum.GetValues(typeof(Movement)))
            {
                bool state = game.AvailableMoves.Contains(move);
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
                    item = game.Board[y, x].Value;
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
            string path = Globals.LEVELS[Globals.LEVEL_CURRENT];
            sprites.Clear();
            foreach (string i in suite)
                sprites.Add(Int16.Parse(i), Resources.Load<Sprite>(path + "/" + i));
        }

#if UNITY_EDITOR
        void OnGUI()
        {
            if (GUILayout.Button("Start at Level 1"))
                LevelAction(1);
            if (GUILayout.Button("Start at Level 2"))
                LevelAction(2);
            if (GUILayout.Button("Win 512"))
                throw new NotImplementedException();
            if (GUILayout.Button("Win 1024"))
                throw new NotImplementedException();
            if (GUILayout.Button("Win 2048"))
                throw new NotImplementedException();
        }

        private void LevelAction(int newLevelName)
        {
            Globals.LEVEL_CURRENT = newLevelName;
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

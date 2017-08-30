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
            GameSceneDelegate.InjectCore(this);
            GameSceneDelegate.InjectUI(this);
            sprites = new Dictionary<int, Sprite>();
            ResetAction();
        }

        void Update()
        {
            // TODO OnScreenOrientationChange
            ResizeViewToScreen();
            ResizeSpriteToScreen(GameObject.Find("Background Sprite"));
        }
        void ResizeViewToScreen()
        {
            if (Screen.width > Screen.height) // Landscape
            {
                Camera.main.orthographicSize = 300;
            }
            else // Portrait
            {
                Camera.main.orthographicSize = 600;
            }
        }

        /*
         * http://answers.unity3d.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
         */
        void ResizeSpriteToScreen(GameObject go)
        {
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) return;

            go.transform.localScale = new Vector3(1, 1, 1);

            var width = sr.sprite.bounds.size.x;
            var height = sr.sprite.bounds.size.y;

            var worldScreenHeight = Camera.main.orthographicSize * 2.0;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            float x = (float)worldScreenWidth / width;
            float y = (float)worldScreenHeight / height;

            go.transform.localScale = new Vector3(x, y, 0);
        }

        // ********************************************************************

        private void ResetAction()
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
            GameObject go = GameObject.Find("Dialog");
            // go.GetComponent<Renderer>().enabled =true;
            //            go  = Instantiate(Resources.Load("Prefab/Dialog View"), go.transform) as GameObject;
            //            SpriteRenderer spriteRend = go.AddComponent<SpriteRenderer>();
            //            spriteRend.sortingOrder = 500;
            // SceneManager.LoadScene(Globals.DIALOG_SCENE, LoadSceneMode.Additive);
            // SceneManager.LoadScene(Globals.MAIN_SCENE, LoadSceneMode.Single);
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
            txt.text = game.Score.ToString();
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

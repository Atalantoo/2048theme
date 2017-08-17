/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Project2048.Core;
using Commons.Inputs;
using Commons.Animations;

namespace Project2048.Scenes
{
    class GameSceneDependency
    {
        public static void InjectCore(GameScene main)
        {
            main.gameManager = new GameManager();
        }

        public static void InjectUI(GameScene main)
        {
            BindButtons(main);
            BindInputs(main);
        }

        private static void BindInputs(GameScene main)
        {
            GameObject gameObject;
            InputDetector input;

            gameObject = main.gameObject;

            input = gameObject.AddComponent<KeysArrowDetector>();
            input.Left = main.MoveLeftAction;
            input.Right = main.MoveRightAction;
            input.Up = main.MoveUpAction;
            input.Down = main.MoveDownAction;
#if UNITY_EDITOR
            input = gameObject.AddComponent<MouseSwipeDetector>();
            input.Left = main.MoveLeftAction;
            input.Right = main.MoveRightAction;
            input.Up = main.MoveUpAction;
            input.Down = main.MoveDownAction;
#endif
#if UNITY_ANDROID
            input = gameObject.AddComponent<TouchSwipeDetector>();
            input.Left = main.MoveLeftAction;
            input.Right = main.MoveRightAction;
            input.Up = main.MoveUpAction;
            input.Down = main.MoveDownAction;
#endif
        }
        private static void BindButtons(GameScene main)
        {
            GameObject go;
            Button btn;

            go = GameObject.Find("reset_button");
            btn = go.GetComponent<Button>();
            btn.onClick.AddListener(main.ResetAction);

            go = GameObject.Find("undo_button");
            btn = go.GetComponent<Button>();
            btn.onClick.AddListener(main.UndoAction);

            foreach (Movement move in Enum.GetValues(typeof(Movement)))
            {
                go = GameObject.Find("move_" + move.ToString() + "_true");
                go.AddComponent<BlinkAnimator>();
            }
        }

    }
}

/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2048.Core;
using Commons.Inputs;
using Commons.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Project2048
{
    class MainDependecy
    {
        public static void InjectCore(Main main)
        {
            main.gameManager = new GameManager();
        }

        public static void InjectUI(Main main)
        {
            BindButtons(main);
            BindInputs(main);
        }

        private static void BindInputs(Main main)
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
        private static void BindButtons(Main main)
        {
            GameObject go;
            Button btn;

            go = GameObject.Find("reset_button");
            btn = go.GetComponent<Button>();
            btn.onClick.AddListener(main.ResetAction);

            go = GameObject.Find("undo_button");
            btn = go.GetComponent<Button>();
            btn.onClick.AddListener(main.UndoAction);

            go = GameObject.Find("botmove1");
            go.AddComponent<BlinkAnimator>();
        }

    }
}

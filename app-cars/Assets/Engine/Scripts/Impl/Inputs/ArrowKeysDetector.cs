using UnityEngine;
using System;

namespace Commons.Inputs
{
    public class ArrowKeysDetector : InputDetector
    {

        void Update()
        {
            if (Right != null && Left != null && Up != null && Down != null)
            {
                if (Input.GetKeyUp(KeyCode.UpArrow))
                    Up();
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                    Down();
                else if (Input.GetKeyUp(KeyCode.RightArrow))
                    Right();
                else if (Input.GetKeyUp(KeyCode.LeftArrow))
                    Left();
            }
        }
    }
}
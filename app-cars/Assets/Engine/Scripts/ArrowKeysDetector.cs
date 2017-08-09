using UnityEngine;
using System;

public class ArrowKeysDetector : InputDetector
{

    void Update()
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
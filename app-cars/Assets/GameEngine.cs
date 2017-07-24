using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum InputDirection
{
    Left, Right, Top, Bottom
}

public interface IGameEngine
{
   // string[][] next(string[][]);
}

public class GameEngine
{
    /**
     * Initialization input
     * 
     * Line 1 : 2 integers W H. The (W, H) couple represents the width and height of the building as a number of windows.
     * Line 2:linkCount, the number of links between factories.
     * Next linkCount lines: 3 space-separated integers factory1, factory2 and distance, where distance is the number of turns needed for a troop to travel between factory1 and factory2.
     */
    public string[][] initInput;
    /**
     * Input for one game turn
     * 
     */
    public string[][] turnInput;

    public void init()
    {
        initInput = new string[1][];
        initInput[0] = new string[] { "4", "4" };

        int w = Int32.Parse(initInput[0][0]);
        int h = Int32.Parse(initInput[0][1]);

        turnInput = new string[h][];
        for (int i = 0; i < h; i++)
        {
            turnInput[i] = new string[w];
            turnInput[i][w] = "0";
        }
    }

    public void next()
    {

    }
}

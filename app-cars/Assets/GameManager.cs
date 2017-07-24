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
public enum GameState
{
    Playing,
    Won
}
public interface IGameEngine
{
   // string[][] turn(string[][] inputs);
}

// https://dgkanatsios.com/2016/01/23/building-the-2048-game-in-unity-via-c-and-visual-studio/
public class GameManager : IGameEngine
{
    /**
     * Initialization input
     * 
     * Line 1 : 2 integers W H. The (W, H) couple represents the width and height of the building as a number of windows.
     * Line 2:linkCount, the number of links between factories.
     * Next linkCount lines: 3 space-separated integers factory1, factory2 and distance, where distance is the number of turns needed for a troop to travel between factory1 and factory2.
     */
    /**
     * Input for one game turn
     * 
     */

    public string[][] init(string[][] input)
    {
        string[][] res;

        int w = Int32.Parse(input[0][0]);
        int h = Int32.Parse(input[0][1]);

        res = new string[h][];
        //res[0] = new string[] { input[0][0], input[0][1] };
        for (int y = 0; y < h; y++)
        {
            res[y] = new string[w];
            for (int x = 0; x < w; x++)
            {
                res[y][x] = "0";
            }
        }
        // TODO random rule
        CreateNewItem(res);
        CreateNewItem(res);
        /*   score = 0;
           UpdateScore(0);
           gameState = GameState.Playing; */
        return res;
    }

    private void CreateNewItem(string[][] matrix)
    {
        int freeOnes = count(matrix, "0");
        Random rnd = new Random();
        int nextOne = rnd.Next(0, freeOnes);
        int i = 0;
        for (int y = 0; y < matrix.Length; y++)
        {
            for (int x = 0; x < matrix[0].Length; x++)
            {
                if ("0".Equals(matrix[y][x]))
                    i++;
                if (nextOne == i)
                    matrix[y][x] = "2";
            }
        }
    }

    public static int count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
        {
            for (int x = 0; x < matrix[0].Length; x++)
            {
                if (match.Equals(matrix[y][x]))
                    res++;
            }
        }
        return res;
    }

    public static string[][] turn(string[][] input)
    {
        string[][] res;

        int w = Int32.Parse(input[0][0]);
        int h = Int32.Parse(input[0][1]);
        string[,] matrix = new string[h, w];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                matrix[y, x] = input[1 + y][x];
            }
        }
        string[] action = input[1 + h];
        string move = action[0];

        // TODO is can move
        res = new string[h][];
        if ('R'.Equals(move))
        {
            res[0] = new string[h];
        }
        return res;
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Commons.Lang;
using Commons.Game;
using System.Diagnostics;

public enum GameState
{
    Playing, Won, Loss
}
public enum InputDirection
{
    L, R, T, B
}
public enum HorizontalMovement
{
    L, R
}
public enum VerticalMovement
{
    T, B
}

// https://dgkanatsios.com/2016/01/23/building-the-2048-game-in-unity-via-c-and-visual-studio/
public class GameManager2048 : GameManager
{
    Game rules;
    int width;
    int height;
    string[][] matrix;
    // TODO GameState state;
    // TODO int score;
    InputDirection inputDir;
    // TODO string[][][] histoStates;
    // TODO string[] histoEvents;

    // PUBLIC *******************************************************

    public string[][] Start(string[][] startInput)
    {
        PreConditions.CheckArgument(null != startInput);

        input = startInput;
        rules.Round("Starting the Game").Execute();
        output = matrix;

        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);
        return output;
    }

    public string[][] Turn(string[][] turnInput)
    {
        PreConditions.CheckArgument(null != turnInput);

        input = turnInput;
        rules.Round("Turn").Execute();
        output = matrix;

        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);
        return output;
    }

    public override void Initialize()
    {
        rules = Games.Get("Game")
            //.Add(Modes.Get("singleplayer")
                .Start(Turns.Get("Starting the Game")
                    .Start(Phases.Get("Setup")
                        .Start(Update_size)
                        .Next(Create_zeros)
                        .Next(Create_random_item)
                        .Next(Create_random_item))
                    .Next(Phases.Get("end")
                        .Start(Update_score)))
                // TODO      .next( available_moves)))
                .Next(Turns.Get("Turn")
                    .Start(Phases.Get("Beginning phase")
                        .Start(Update_size)
                        .Next(Update_board)
                        .Next(Input_direction))
                    .Next(Phases.Get("play phase")
                        .Start(Merge_items)
                        .Next(Move_items))
                    //.next(create_random_item)
                    .Next(Phases.Get("Ending phase")
                        .Start(Update_score)))
            //)
            //  TODO     .next( available_moves)))
            // TODO next(Turns.get("Ending the Game")
            //  TODO .start(end)
            //  TODO .next( available_moves)))                  
            .Build();
    }

    //  PRIVATE *******************************************************

    void Update_score()
    {
        // TODO state = GameState.Playing;
        // TODO score = 0;
    }

    void Update_size()
    {
        Debug.Assert(input != null && input.Length > 0 && input[0].Length > 0);

        width = Int32.Parse(input[0][0]);
        height = Int32.Parse(input[0][1]);

        Debug.Assert(width > 0 && height > 0);
    }

    void Create_zeros()
    {
        Debug.Assert(width > 0 && height > 0);

        matrix = new string[height][];
        for (int y = 0; y < height; y++)
        {
            matrix[y] = new string[width];
            for (int x = 0; x < this.width; x++)
                matrix[y][x] = "0";
        }

        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);
        Debug.Assert(Count(matrix, "0") == width * height);
    }

    void Create_random_item()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(matrix != null && matrix.Length > 0);
        int old2Ones = Count(matrix, "2");

        int freeOnes = Count(matrix, "0");
        Random rnd = new Random();
        int nextOne = rnd.Next(0, freeOnes);
        int i = 0;
        for (int y = 0; y < width; y++)
            for (int x = 0; x < height; x++)
            {
                if (EmptyItem(matrix, y, x))
                    i++;
                if (nextOne == i)
                    matrix[y][x] = "2";
            }

        int new2Ones = Count(matrix, "2");
        Debug.Assert(new2Ones == old2Ones + 1);
    }

    void Update_board()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(input != null && input.Length > 0 && input[0].Length > 0);

        matrix = new string[height][];
        for (int y = 0; y < height; y++)
            matrix[y] = input[1 + y];

        Debug.Assert(matrix != null && matrix.Length > 0);
    }

    void Merge_items()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);

        for (int y = 0; y < height; y++)
            foreach (int x in ColumnNumbers(inputDir, width))
            {
                int next = FoundTwinItem(matrix[y], x, Dir(inputDir));
                if (next == x)
                    continue;
                Action_mergeItems(matrix, y, x, next);
            }

        // TODO  Debug.Assert()
    }

    void Move_items()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);

        for (int y = 0; y < height; y++)
            foreach (int x in ColumnNumbers(inputDir, width))
            {
                if (EmptyItem(matrix, y, x))
                    continue;
                int next = FoundEmptyItem(matrix[y], x, Dir(inputDir));
                if (next == x)
                    continue;
                Action_moveItem(matrix, y, x, next);
            }

        // TODO  Debug.Assert()
    }

    void Input_direction()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(input != null && input.Length > 0 && input[0].Length > 0);

        string str = input[1 + height][0];
        PreConditions.CheckArgument(Enum.IsDefined(typeof(InputDirection), str));
        inputDir = (InputDirection)Enum.Parse(typeof(InputDirection), str);
    }

    // STATIC *******************************************************

    static IEnumerable<int> ColumnNumbers(InputDirection move, int width)
    {
        IEnumerable<int> nbr;
        HorizontalMovement mov = (move == InputDirection.R) ? HorizontalMovement.R : HorizontalMovement.L;
        nbr = Enumerable.Range(0, width);
        nbr = (mov == HorizontalMovement.L) ? nbr : nbr.Reverse();
        return nbr;
    }

    static int Dir(InputDirection move)
    {
        int dir;
        HorizontalMovement mov = (move == InputDirection.R) ? HorizontalMovement.R : HorizontalMovement.L;
        dir = (mov == HorizontalMovement.L) ? -1 : 1;
        return dir;
    }

    static int FoundEmptyItem(string[] row, int x, int direction)
    {
        int item = x;
        bool emptyItemFound = true;
        while (emptyItemFound)
        {
            int next = item + direction;
            emptyItemFound = Arrays.InBound(row, next) && EmptyItem(row, next);
            if (emptyItemFound)
                item = next;
            else
                return item;
        }
        return item;
    }

    static int Count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
            for (int x = 0; x < matrix[0].Length; x++)
                if (match.Equals(matrix[y][x]))
                    res++;
        return res;
    }

    static bool EmptyItem(string[] row, int x)
    {
        return "0".Equals(row[x]);
    }

    static bool EmptyItem(string[][] matrix, int y, int x)
    {
        return "0".Equals(matrix[y][x]);
    }

    static void Action_mergeItems(string[][] matrix, int y, int x, int next)
    {
        int val = Int32.Parse(matrix[y][x]) * 2;
        matrix[y][x] = val.ToString();
        matrix[y][next] = "0";
    }

    static void Action_moveItem(string[][] matrix, int y, int x, int next)
    {
        matrix[y][next] = matrix[y][x];
        matrix[y][x] = "0";
    }

    static int FoundTwinItem(string[] row, int x, int direction)
    {
        int item = x;
        int next = item + direction;
        if (Arrays.InBound(row, next) && row[x].Equals(row[next]))
            item = next;
        return item;
    }
}
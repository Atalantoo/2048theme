using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Commons.Lang;
using Commons.Game;
using System.Diagnostics;

// https://dgkanatsios.com/2016/01/23/building-the-2048-game-in-unity-via-c-and-visual-studio/
public class GameManager2048 : GameManager
{
    Game rules;

    // API *******************************************************

    public override void Initialize()
    {
        InitializeRules();
    }

    public string[][] Start(string[][] startInput)
    {
        PreConditions.CheckArgument(null != startInput);

        input = startInput;
        rules.Turn("Starting the Game").Execute();
        output = matrix;


        return output;
    }

    public string[][] Turn(string[][] turnInput)
    {
        PreConditions.CheckArgument(null != turnInput);

        input = turnInput;
        rules.Turn("Player Turn").Execute();
        output = matrix;


        return output;
    }

    // RULES *******************************************************

    void InitializeRules()
    {
        rules = Games.Get("SinglePlayer Game")
            .Start(Turns.Get("Starting the Game")
                .Start(Phases.Get("Setup phase")
                    .Start(Steps.Get(Update_size))
                    .Next(Steps.Get(Create_zeros))
                    .Next(Steps.Get(Create_random_item))
                    .Next(Steps.Get(Create_random_item)))
                .Next(Phases.Get("Ending phase")
                    .Start(Steps.Get(Update_score))))
            .Next(Turns.Get("Player Turn")
                .Start(Phases.Get("Beginning phase")
                    .Start(Steps.Get(Update_size))
                    .Next(Steps.Get(Update_board))
                    .Next(Steps.Get(Input_direction)))
                .Next(Phases.Get("Move phase")
                    .Start(Steps.Get(Merge_identical_items))
                    .Next(Steps.Get(Move_items))
                    .Next(Steps.Get(Create_random_item)))
                .Next(Phases.Get("Ending phase")
                    .Start(Steps.Get(Update_score))))
        .Build();

        //TODO .Add(Modes.Get( "singleplayer" )
        //TODO .Add(Variant
        // TODO      .Next(  available_moves )))
        // )
        //  TODO     .Next(  available_moves )))
        // TODO next(Turns.Get( "Ending the Game" )
        //  TODO .Start( end )
        //  TODO .Next(  available_moves )))    
    }

    //  OBJECT *******************************************************
    // TODO external Game object ?

    enum GameState { Playing, Won, Loss }
    enum InputDirection { L, R, T, B }
    enum HorizontalMovement { L, R }
    enum VerticalMovement { T, B }

    int width;
    int height;
    string[][] matrix;
    GameState state;
    int score;
    InputDirection inputDir;
    // TODO string[][][] histoStates;
    // TODO string[] histoEvents;

    void Update_score()
    {
        state = GameState.Playing;
        score = 0;
    }

    void Update_size()
    {
        width = Int32.Parse(input[0][0]);
        height = Int32.Parse(input[0][1]);
    }

    void Create_zeros()
    {
        matrix = new string[height][];
        for (int y = 0; y < height; y++)
        {
            matrix[y] = new string[width];
            for (int x = 0; x < this.width; x++)
                matrix[y][x] = "0";
        }
    }

    void Create_random_item()
    {
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
    }

    void Update_board()
    {
        matrix = new string[height][];
        for (int y = 0; y < height; y++)
            matrix[y] = input[1 + y];
    }

    void Merge_identical_items()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in ColumnNumbers(inputDir, width))
            {
                int next = FiundTwinItem(matrix[y], x, Dir(inputDir));
                if (next == -1)
                    continue;
                Action_mergeItems(matrix, y, x, next);
            }
    }

    void Move_items()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in ColumnNumbers(inputDir, width))
            {
                if (EmptyItem(matrix, y, x))
                    continue;
                int next = FiundEmptyItem(matrix[y], x, Dir(inputDir));
                if (next == x)
                    continue;
                Action_moveItem(matrix, y, x, next);
            }
    }

    void Input_direction()
    {
        string str = input[1 + height][0];
        PreConditions.CheckArgument(Enum.IsDefined(typeof(InputDirection), str));
        inputDir = (InputDirection)Enum.Parse(typeof(InputDirection), str);
    }

    // FUNCTION(S ) *******************************************************

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

    static int FiundEmptyItem(string[] row, int x, int direction)
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

    static int FiundTwinItem(string[] row, int current, int direction)
    {
        int next = current + direction;
        while (Arrays.InBound(row, next))
        {
            if (row[next].Equals(row[current]))
                return next;
            if (!row[next].Equals("0"))
                break;
            next += direction;
        }
        return -1;
    }
}
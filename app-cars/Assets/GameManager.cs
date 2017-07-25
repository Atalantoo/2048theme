using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Commons.Lang;
using Commons.Game;
using System.Diagnostics;

public enum InputDirection
{
    L, R, T, B
}
public enum GameState
{
    Playing,
    Won,
    Loss
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
public class GameManager : Game
{
    Rule rules;
    int width;
    int height;
    string[][] matrix;
    GameState state;
    int score;
    InputDirection inputDir;

    // TODO histo

    public GameManager()
    {
        Initialize();
    }

    // PUBLIC *******************************************************

    public string[][] init(string[][] input)
    {
        PreConditions.checkArgument(null != input);
        this.input = input;
        rules.getRound("init").execute();
        output = matrix;
        return output;
    }

    public string[][] turn(string[][] input)
    {
        PreConditions.checkArgument(null != input);
        this.input = input;
        rules.getRound("turn").execute();
        output = matrix;
        return output;
    }

    //  PRIVATE *******************************************************

    void Initialize()
    {
        rules = Rules.get("game")
            .start(Rounds.get("init")
                .start(Phases.get("board")
                    .start(update_size)
                    .next(create_zeros)
                    .next(create_random_item)
                    .next(create_random_item))
                .next(Phases.get("end")
                    .start(update_score)))
            //       .next( available_moves)))
            .next(Rounds.get("turn")
                .start(Phases.get("start")
                    .start(update_size)
                    .next(update_board)
                    .next(player_action))
                .next(Phases.get("play")
                    .start(merge_items)
                    .next(move_items))
                //.next(create_random_item)
                .next(Phases.get("end")
                    .start(update_score)))
            //       .next( available_moves)))
            //next(Rounds.get("end")
            //   .start(end)
            //   .next( available_moves)))
            .build();
    }

    void update_score()
    {
        state = GameState.Playing;
        score = 0;
    }

    void update_size()
    {
        Debug.Assert(input != null && input.Length > 0 && input[0].Length > 0);

        width = Int32.Parse(input[0][0]);
        height = Int32.Parse(input[0][1]);

        Debug.Assert(width > 0 && height > 0);
    }

    void create_zeros()
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
        Debug.Assert(count(matrix, "0") == width * height);
    }

    void create_random_item()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(matrix != null && matrix.Length > 0);
        int old2Ones = count(matrix, "2");

        int freeOnes = count(matrix, "0");
        Random rnd = new Random();
        int nextOne = rnd.Next(0, freeOnes);
        int i = 0;
        for (int y = 0; y < width; y++)
            for (int x = 0; x < height; x++)
            {
                if (emptyItem(matrix, y, x))
                    i++;
                if (nextOne == i)
                    matrix[y][x] = "2";
            }

        int new2Ones = count(matrix, "2");
        Debug.Assert(new2Ones == old2Ones + 1);
    }

    void update_board()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(input != null && input.Length > 0 && input[0].Length > 0);

        matrix = new string[height][];
        for (int y = 0; y < height; y++)
            matrix[y] = input[1 + y];

        Debug.Assert(matrix != null && matrix.Length > 0);
    }

    void merge_items()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);

        for (int y = 0; y < height; y++)
            foreach (int x in columnNumbers(inputDir, width))
            {
                int next = foundTwinItem(matrix[y], x, dir(inputDir));
                if (next == x)
                    continue;
                action_mergeItems(matrix, y, x, next);
            }

        // TODO  Debug.Assert()
    }

    void move_items()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(matrix != null && matrix.Length > 0 && matrix[0].Length > 0);

        for (int y = 0; y < height; y++)
            foreach (int x in columnNumbers(inputDir, width))
            {
                if (emptyItem(matrix, y, x))
                    continue;
                int next = foundEmptyItem(matrix[y], x, dir(inputDir));
                if (next == x)
                    continue;
                action_moveItem(matrix, y, x, next);
            }

        // TODO  Debug.Assert()
    }

    void player_action()
    {
        Debug.Assert(width > 0 && height > 0);
        Debug.Assert(input != null && input.Length > 0 && input[0].Length > 0);

        string str = input[1 + height][0];
        PreConditions.checkArgument(Enum.IsDefined(typeof(InputDirection), str));
        inputDir = (InputDirection)Enum.Parse(typeof(InputDirection), str);
    }

    // STATIC *******************************************************

    static IEnumerable<int> columnNumbers(InputDirection move, int width)
    {
        IEnumerable<int> nbr;
        HorizontalMovement mov = (move == InputDirection.R) ? HorizontalMovement.R : HorizontalMovement.L;
        nbr = Enumerable.Range(0, width);
        nbr = (mov == HorizontalMovement.L) ? nbr : nbr.Reverse();
        return nbr;
    }

    static int dir(InputDirection move)
    {
        int dir;
        HorizontalMovement mov = (move == InputDirection.R) ? HorizontalMovement.R : HorizontalMovement.L;
        dir = (mov == HorizontalMovement.L) ? -1 : 1;
        return dir;
    }

    static int foundEmptyItem(string[] row, int x, int direction)
    {
        int item = x;
        bool emptyItemFound = true;
        while (emptyItemFound)
        {
            int next = item + direction;
            emptyItemFound = Arrays.inBound(row, next) && emptyItem(row, next);
            if (emptyItemFound)
                item = next;
            else
                return item;
        }
        return item;
    }

    static int count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
            for (int x = 0; x < matrix[0].Length; x++)
                if (match.Equals(matrix[y][x]))
                    res++;
        return res;
    }

    static bool emptyItem(string[] row, int x)
    {
        return "0".Equals(row[x]);
    }

    static bool emptyItem(string[][] matrix, int y, int x)
    {
        return "0".Equals(matrix[y][x]);
    }

    static void action_mergeItems(string[][] matrix, int y, int x, int next)
    {
        int val = Int32.Parse(matrix[y][x]) * 2;
        matrix[y][x] = val.ToString();
        matrix[y][next] = "0";
    }

    static void action_moveItem(string[][] matrix, int y, int x, int next)
    {
        matrix[y][next] = matrix[y][x];
        matrix[y][x] = "0";
    }

    static int foundTwinItem(string[] row, int x, int direction)
    {
        int item = x;
        int next = item + direction;
        if (Arrays.inBound(row, next) && row[x].Equals(row[next]))
            item = next;
        return item;
    }
}
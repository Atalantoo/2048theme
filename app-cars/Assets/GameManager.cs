using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Commons.Lang;

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

public interface IGameEngine
{
    // string[][] turn(string[][] inputs);
}

// https://dgkanatsios.com/2016/01/23/building-the-2048-game-in-unity-via-c-and-visual-studio/
public class GameManager : IGameEngine
{
    int width;
    int height;
    string[][] matrix;
    GameState state;
    int score;
    string[][] action;
    // TODO histo

    public string[][] init(string[][] input)
    {
        rule_init_game(input);
        rule_update_game(input);
        rule_init_matrix_with_0();
        rule_init_matrix_with_2_random_items();
        return matrix;
    }

    public string[][] turn(string[][] input)
    {
        rule_update_game(input);
        rule_update_matrix(input);
        rule_update_input(input);

        rule_turn_prepare();
        // TODO possible actions
        rule_turn_merge_same_items();
        rule_turn_move_items_to_horizontal_direction_if_free();
        // TODO with_1_random_items createRandomItem
        // TODO UpdateScore(0);
        // TODO end game loss
        // TODO end game win
        // TODO res possible moves
        return matrix;
    }

    HorizontalMovement horizontalMovement;
    IEnumerable<int> columnNumbers;
    int dir;

    // RULES ********************************************************

    private void rule_init_game(string[][] input)
    {
        score = 0;
        state = GameState.Playing;
    }

    private void rule_init_matrix_with_0()
    {
        matrix = new string[height][];
        for (int y = 0; y < height; y++)
        {
            matrix[y] = new string[width];
            for (int x = 0; x < width; x++)
                matrix[y][x] = "0";
        }
    }

    private void rule_init_matrix_with_2_random_items()
    {
        createRandomItem(matrix);
        createRandomItem(matrix);
    }

    private void rule_update_game(string[][] input)
    {
        width = Int32.Parse(input[0][0]);
        height = Int32.Parse(input[0][1]);
    }

    private void rule_update_matrix(string[][] input)
    {
        matrix = new string[height][];
        for (int y = 0; y < height; y++)
            matrix[y] = input[1 + y];
    }

    private void rule_update_input(string[][] input)
    {
        action = new string[1][];
        action[0] = input[1 + height];
    }

    private void rule_turn_prepare()
    {
        string move = action[0][0];
        PreConditions.checkArgument(Enum.IsDefined(typeof(InputDirection), move));
        horizontalMovement = ("R".Equals(move)) ? HorizontalMovement.R : HorizontalMovement.L;
        columnNumbers = Enumerable.Range(0, width);
        dir = (horizontalMovement == HorizontalMovement.L) ? -1 : 1;
        columnNumbers = (horizontalMovement == HorizontalMovement.L) ? columnNumbers : columnNumbers.Reverse();
    }

    private void rule_turn_move_items_to_horizontal_direction_if_free()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in columnNumbers)
            {
                if (emptyItem(y, x))
                    continue;
                int next = foundEmptyItem(matrix[y], x, dir);
                if (next == x)
                    continue;
                matrix[y][next] = matrix[y][x];
                matrix[y][x] = "0";
            }
    }

    private void rule_turn_merge_same_items()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in columnNumbers)
            {
                int next = foundTwinItem(matrix[y], x, dir);
                if (next == x)
                    continue;
                int val = Int32.Parse(matrix[y][x]) * 2;
                matrix[y][x] = val.ToString();
                matrix[y][next] = "0";
            }
    }

    // UTILS ********************************************************

    public int count(string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
            for (int x = 0; x < matrix[0].Length; x++)
                if (match.Equals(matrix[y][x]))
                    res++;
        return res;
    }

    private void createRandomItem(string[][] matrix)
    {
        int freeOnes = count("0");
        Random rnd = new Random();
        int nextOne = rnd.Next(0, freeOnes);
        int i = 0;
        for (int y = 0; y < matrix.Length; y++)
            for (int x = 0; x < matrix[0].Length; x++)
            {
                if (emptyItem(y, x))
                    i++;
                if (nextOne == i)
                    matrix[y][x] = "2";
            }
    }

    private int foundEmptyItem(string[] row, int x, int direction)
    {
        int item = x;
        bool emptyItemFound = true;
        while (emptyItemFound)
        {
            int next = item + direction;
            emptyItemFound = ArrayUtils.inBound(row, next) && emptyItem(row, next);
            if (emptyItemFound)
                item = next;
            else
                return item;
        }
        return item;
    }

    private int foundTwinItem(string[] row, int x, int direction)
    {
        int item = x;
        int next = item + direction;
        if (ArrayUtils.inBound(row, next) && row[x].Equals(row[next]))
            item = next;
        return item;
    }

    public bool emptyItem(string[] array, int i)
    {
        return "0".Equals(array[i]);
    }

    public bool emptyItem(int y, int x)
    {
        return "0".Equals(matrix[y][x]);
    }
}


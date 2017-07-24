using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Commons.Lang;
using Commons.Game;

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
public class GameManager
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
        rule_init_phase_start(input);
        rule_init_phase_board(input);
        // rule_update_game(input);
        return matrix;
    }

    public string[][] turn(string[][] input)
    {
        rule_turn_phase_start(input);
        // TODO rule_turn_phase_moves()
        rule_turn_phase_play();

        // TODO UpdateScore(0);
        // TODO end game loss
        // TODO rule_turn_phase_end
        // TODO res possible moves
        return matrix;
    }

    HorizontalMovement horizontalMovement;
    IEnumerable<int> columnNumbers;
    int dir;

    // RULES ********************************************************

    private void rule_init_phase_start(string[][] input)
    {
        score = 0;
        state = GameState.Playing;
        action_update_game(input);
    }

    private void rule_init_phase_board(string[][] input)
    {
        rule_init_phase_board_step_zeros();
        rule_init_phase_board_step_random2();
    }

    private void rule_init_phase_board_step_zeros()
    {
        matrix = new string[height][];
        for (int y = 0; y < height; y++)
        {
            matrix[y] = new string[width];
            for (int x = 0; x < width; x++)
                matrix[y][x] = action_createEmpty();
        }
    }

    private void rule_init_phase_board_step_random2()
    {
        action_createRandomItem(matrix);
        action_createRandomItem(matrix);
    }

    // TURN ********************

    // TURN: PHASE(S)

    private void rule_turn_phase_start(string[][] input)
    {
        action_update_game(input);
        step_update_matrix(input);
        step_update_input(input);
        step_direction();
    }

    // TURN: STEP(S)

    private void step_update_matrix(string[][] input)
    {
        matrix = new string[height][];
        for (int y = 0; y < height; y++)
            matrix[y] = input[1 + y];
    }

    private void step_update_input(string[][] input)
    {
        action = new string[1][];
        action[0] = input[1 + height];
    }

    private void step_direction()
    {
        string move = action[0][0];
        PreConditions.checkArgument(Enum.IsDefined(typeof(InputDirection), move));
        horizontalMovement = ("R".Equals(move)) ? HorizontalMovement.R : HorizontalMovement.L;
        columnNumbers = Enumerable.Range(0, width);
        dir = (horizontalMovement == HorizontalMovement.L) ? -1 : 1;
        columnNumbers = (horizontalMovement == HorizontalMovement.L) ? columnNumbers : columnNumbers.Reverse();
    }

    private void rule_turn_phase_play()
    {
        step_turn_merge_same_items();
        step_turn_move_items_to_horizontal_direction_if_free();
        // TODO with_1_random_items createRandomItem

    }

    private void step_turn_merge_same_items()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in columnNumbers)
            {
                int next = foundTwinItem(matrix[y], x, dir);
                if (next == x)
                    continue;
                action_mergeItems(y, x, next);
            }
    }

    private void step_turn_move_items_to_horizontal_direction_if_free()
    {
        for (int row = 0; row < height; row++)
            foreach (int actual in columnNumbers)
            {
                if (emptyItem(row, actual))
                    continue;
                int next = foundEmptyItem(matrix[row], actual, dir);
                if (next == actual)
                    continue;
                action_movIteme(row, actual, next);
            }
    }

    // TURN: ACTION(S)

    private void action_update_game(string[][] input)
    {
        width = Int32.Parse(input[0][0]);
        height = Int32.Parse(input[0][1]);
    }

    private static string action_createEmpty()
    {
        return "0";
    }

    private void action_createRandomItem(string[][] matrix)
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

    private void action_mergeItems(int y, int x, int next)
    {
        int val = Int32.Parse(matrix[y][x]) * 2;
        matrix[y][x] = val.ToString();
        matrix[y][next] = "0";
    }

    private void action_movIteme(int y, int x, int next)
    {
        matrix[y][next] = matrix[y][x];
        matrix[y][x] = "0";
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

    private int foundEmptyItem(string[] row, int x, int direction)
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

    private int foundTwinItem(string[] row, int x, int direction)
    {
        int item = x;
        int next = item + direction;
        if (Arrays.inBound(row, next) && row[x].Equals(row[next]))
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


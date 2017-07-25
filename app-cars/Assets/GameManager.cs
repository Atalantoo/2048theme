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

public class Game2048 : Game
{
    public int width;
    public int height;
    public string[][] matrix;
    public GameState state;
    public int score;
    // TODO histo
    public HorizontalMovement horizontalMovement;
    public IEnumerable<int> columnNumbers;
    public int dir;
}

// https://dgkanatsios.com/2016/01/23/building-the-2048-game-in-unity-via-c-and-visual-studio/
public class GameManager
{
    Game2048 game;
    Rule<Game2048> rules;

    public GameManager()
    {
        rules = Rules<Game2048>.get("game")
            .start(Rounds<Game2048>.get("init")
                .start(Phases<Game2048>.get("board")
                    .start(update_size)
                    .next(create_zeros)
                    .next(create_random_item)
                    .next(create_random_item))
                .next(Phases<Game2048>.get("end")
                    .start(update_score)))
            //       .next( available_moves)))
            .next(Rounds<Game2048>.get("turn")
                .start(Phases<Game2048>.get("start")
                    .start(update_size)
                    .next(update_board)
                    .next(update_direction))
                .next(Phases<Game2048>.get("play")
                    .start(merge_items)
                    .next(move_items))
                    //.next(create_random_item)
                .next(Phases<Game2048>.get("end")
                    .start(update_score)))
            //       .next( available_moves)))
            //next(Rounds<Game2048>.get("end")
            //   .start(end)
            //   .next( available_moves)))
            .build();
    }

    public string[][] init(string[][] input)
    {
        game = new Game2048();
        game.input = input;
        game = rules.childs[0].execute(game);
        game.output = game.matrix;
        return game.output;
    }

    public string[][] turn(string[][] input)
    {
        game = new Game2048();
        game.input = input;
        game = rules.childs[1].execute(game);
        game.output = game.matrix;
        return game.output;
    }

    private Func<Game2048, Game2048>update_score = delegate (Game2048 game)
    {
        game.score = 0;
        game.state = GameState.Playing;
        return game;
    };

    private Func<Game2048, Game2048>update_size = delegate (Game2048 game)
    {
        game.width = Int32.Parse(game.input[0][0]);
        game.height = Int32.Parse(game.input[0][1]);
        return game;
    };

    private Func<Game2048, Game2048>create_zeros = delegate (Game2048 game)
    {
        game.matrix = new string[game.height][];
        for (int y = 0; y < game.height; y++)
        {
            game.matrix[y] = new string[game.width];
            for (int x = 0; x < game.width; x++)
                game.matrix[y][x] = "0";
        }
        return game;
    };

    private Func<Game2048, Game2048>create_random_item = delegate (Game2048 game)
    {
        int freeOnes = count(game.matrix, "0");
        Random rnd = new Random();
        int nextOne = rnd.Next(0, freeOnes);
        int i = 0;
        for (int y = 0; y < game.matrix.Length; y++)
            for (int x = 0; x < game.matrix[0].Length; x++)
            {
                if (emptyItem(game.matrix, y, x))
                    i++;
                if (nextOne == i)
                    game.matrix[y][x] = "2";
            }
        return game;
    };

    private Func<Game2048, Game2048>update_direction = delegate (Game2048 game)
    {
        string[][] action;
        action = new string[1][];
        action[0] = game.input[1 + game.height];
        string move = action[0][0];
        PreConditions.checkArgument(Enum.IsDefined(typeof(InputDirection), move));
        game.horizontalMovement = ("R".Equals(move)) ? HorizontalMovement.R : HorizontalMovement.L;
        game.columnNumbers = Enumerable.Range(0, game.width);
        game.dir = (game.horizontalMovement == HorizontalMovement.L) ? -1 : 1;
        game.columnNumbers = (game.horizontalMovement == HorizontalMovement.L) ? game.columnNumbers : game.columnNumbers.Reverse();
        return game;
    };

    private Func<Game2048, Game2048>update_board = delegate (Game2048 game)
    {
        game.matrix = new string[game.height][];
        for (int y = 0; y < game.height; y++)
            game.matrix[y] = game.input[1 + y];
        return game;
    };

    private Func<Game2048, Game2048>merge_items = delegate (Game2048 game)
    {
        for (int row = 0; row < game.height; row++)
            foreach (int col in game.columnNumbers)
            {
                int next = foundTwinItem(game.matrix[row], col, game.dir);
                if (next == col)
                    continue;
                action_mergeItems(game.matrix, row, col, next);
            }
        return game;
    };

    private Func<Game2048, Game2048>move_items = delegate (Game2048 game)
    {
        for (int row = 0; row < game.height; row++) 
            foreach (int col in game.columnNumbers)
            {
                if (emptyItem(game.matrix, row, col))
                    continue;
                int next = foundEmptyItem(game.matrix[row], col, game.dir);
                if (next == col)
                    continue;
                action_moveItem(game.matrix, row, col, next);
            }
        return game;
    };

    private static int foundEmptyItem(string[] row, int x, int direction)
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

    private static int count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
            for (int x = 0; x < matrix[0].Length; x++)
                if (match.Equals(matrix[y][x]))
                    res++;
        return res;
    }

    private static bool emptyItem(string[] row, int x)
    {
        return "0".Equals(row[x]);
    }

    private static bool emptyItem(string[][] matrix, int y, int x)
    {
        return "0".Equals(matrix[y][x]);
    }

    private static void action_mergeItems(string[][] matrix, int y, int x, int next)
    {
        int val = Int32.Parse(matrix[y][x]) * 2;
        matrix[y][x] = val.ToString();
        matrix[y][next] = "0";
    }

    private static void action_moveItem(string[][] matrix, int y, int x, int next)
    {
        matrix[y][next] = matrix[y][x];
        matrix[y][x] = "0";
    }

    private static int foundTwinItem(string[] row, int x, int direction)
    {
        int item = x;
        int next = item + direction;
        if (Arrays.inBound(row, next) && row[x].Equals(row[next]))
            item = next;
        return item;
    }

}
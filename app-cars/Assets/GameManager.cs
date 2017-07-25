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
    Rule rules;
    Game game;

    public GameManager()
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
                    .next(update_direction))
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

    public string[][] init(string[][] input)
    {
        game = new Game2048();
        game.input = input;
        game = rules.childs[0].execute(game);
        game.output = ((Game2048)game).matrix;
        return game.output;
    }

    public string[][] turn(string[][] input)
    {
        game = new Game2048();
        game.input = input;
        game = rules.childs[1].execute(game);
        game.output = ((Game2048)game).matrix;
        return game.output;
    }

    private Func<Game, Game> update_score = delegate (Game game)
    {
        ((Game2048)game).score = 0;
        ((Game2048)game).state = GameState.Playing;
        return game;
    };

    private Func<Game, Game> update_size = delegate (Game game)
    {
        ((Game2048)game).width = Int32.Parse(game.input[0][0]);
        ((Game2048)game).height = Int32.Parse(game.input[0][1]);
        return game;
    };

    private Func<Game, Game> create_zeros = delegate (Game game)
    {
        ((Game2048)game).matrix = new string[((Game2048)game).height][];
        for (int y = 0; y < ((Game2048)game).height; y++)
        {
            ((Game2048)game).matrix[y] = new string[((Game2048)game).width];
            for (int x = 0; x < ((Game2048)game).width; x++)
                ((Game2048)game).matrix[y][x] = "0";
        }
        return game;
    };

    private Func<Game, Game> create_random_item = delegate (Game game)
    {
        int freeOnes = count(((Game2048)game).matrix, "0");
        Random rnd = new Random();
        int nextOne = rnd.Next(0, freeOnes);
        int i = 0;
        for (int y = 0; y < ((Game2048)game).matrix.Length; y++)
            for (int x = 0; x < ((Game2048)game).matrix[0].Length; x++)
            {
                if (emptyItem(((Game2048)game).matrix, y, x))
                    i++;
                if (nextOne == i)
                    ((Game2048)game).matrix[y][x] = "2";
            }
        return game;
    };

    private Func<Game, Game> update_direction = delegate (Game game)
    {
        string[][] action;
        action = new string[1][];
        action[0] = ((Game2048)game).input[1 + ((Game2048)game).height];
        string move = action[0][0];
        PreConditions.checkArgument(Enum.IsDefined(typeof(InputDirection), move));
        ((Game2048)game).horizontalMovement = ("R".Equals(move)) ? HorizontalMovement.R : HorizontalMovement.L;
        ((Game2048)game).columnNumbers = Enumerable.Range(0, ((Game2048)game).width);
        ((Game2048)game).dir = (((Game2048)game).horizontalMovement == HorizontalMovement.L) ? -1 : 1;
        ((Game2048)game).columnNumbers = (((Game2048)game).horizontalMovement == HorizontalMovement.L) ? ((Game2048)game).columnNumbers : ((Game2048)game).columnNumbers.Reverse();
        return game;
    };

    private Func<Game, Game> update_board = delegate (Game game)
    {
        ((Game2048)game).matrix = new string[((Game2048)game).height][];
        for (int y = 0; y < ((Game2048)game).height; y++)
            ((Game2048)game).matrix[y] = ((Game2048)game).input[1 + y];
        return game;
    };

    private Func<Game, Game> merge_items = delegate (Game game)
    {
        for (int row = 0; row < ((Game2048)game).height; row++)
            foreach (int col in ((Game2048)game).columnNumbers)
            {
                int next = foundTwinItem(((Game2048)game).matrix[row], col, ((Game2048)game).dir);
                if (next == col)
                    continue;
                action_mergeItems(((Game2048)game).matrix, row, col, next);
            }
        return game;
    };

    private Func<Game, Game> move_items = delegate (Game game)
    {
        for (int row = 0; row < ((Game2048)game).height; row++) 
            foreach (int col in ((Game2048)game).columnNumbers)
            {
                if (emptyItem(((Game2048)game).matrix, row, col))
                    continue;
                int next = foundEmptyItem(((Game2048)game).matrix[row], col, ((Game2048)game).dir);
                if (next == col)
                    continue;
                action_moveItem(((Game2048)game).matrix, row, col, next);
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
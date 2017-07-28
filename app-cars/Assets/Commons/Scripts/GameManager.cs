using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

// API *******************************************************
public enum GameState { Playing, Won, Loss }
public enum InputDirection { L, R, T, B }
public class GameStartInput
{
    public int Width;
    public int Height;
}
public class GameTurnInput
{
    public string Move;
}
public class Game
{
    public int Width;
    public int Height;
    public int[,] board;
    public GameState state;
    public int score;
    public InputDirection[] AvailableMoves;
}
public class GameManager
{

    public void Initialize() { InitializeRules(); }
    public Game Start(GameStartInput input) { actionStart.Invoke(input); return Game2(); }
    public Game Turn(GameTurnInput input) { actionTurn.Invoke(input); return Game2(); }
    public Game Reload(Game input) { actionReload.Invoke(input); return Game2(); }

    Game Game2()
    {
        return new Game()
        {
        };
    }

    // RULES *******************************************************

    Action<GameStartInput> actionStart;
    Action<Game> actionReload;
    Action<GameTurnInput> actionTurn;

    public void InitializeRules()
    {
        //TODO .Add(Modes.Get( "singleplayer" )
        //TODO .Add(Variant
        actionReload = (i) =>
        { // Starting_the_Game_Turn
            { // Setup_Phase
                Clean_game_states();
                Reload_Board(i);
            }
            { // End
                Update_score();
                //  TODO     .Next(  available_moves )))
                Return();
            }
        };
        actionStart = (i) =>
        { // Starting_the_Game_Turn
            { // Setup_Phase
                Update_input(i);
                Clean_game_states();
                Fill_with_zeros_items();
                Create_random_item();
                Create_random_item();
            }
            { // End
                Update_score();
                //  TODO     .Next(  available_moves )))
                Return();
            }
        };
        actionTurn = (i) =>
        { // Player Turn
            { // Beginning phase
                Input_direction(i);
            }
            {  // Move phase
                Merge_identical_items();
                Move_items();
                Create_random_item();
            }
            { // End
                Update_score();
                //  TODO     .Next(  available_moves )))
                Return();
            }
        };
        // TODO next(Turns.Get( "Ending the Game" )
        //  TODO .Start( end )
        //  TODO .Next(  available_moves )))   
    }

    //  OBJECT *******************************************************
    // TODO external Game object ?

    enum HorizontalMovement { L, R }
    enum VerticalMovement { T, B }
    class Point { public int y; public int x; public Point(int y, int x) { this.y = y; this.x = x; } }
    private const int NEW = 2;
    private const int FREE = 0;

    int width;
    int height;
    int[,] matrix;
    GameState state;
    int score;
    InputDirection inputDir;
    // TODO string[][][] histoStates;
    // TODO string[] histoEvents;

    void Update_input(GameStartInput input)
    {
        width = input.Width;
        height = input.Height;
    }

    void Clean_game_states()
    {
        width = -1;
        height = -1;
        matrix = null;
        score = -1;
    }

    void Update_score()
    {
        state = GameState.Playing;
        score = 0;
    }

    void Fill_with_zeros_items()
    {
        matrix = new int[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                matrix[y, x] = FREE;
    }

    void Create_random_item()
    {
        Point[] freeItems = GetEmptyItems();
        int i = new Random().Next(0, freeItems.Length);
        matrix[freeItems[i].y, freeItems[i].x] = NEW;
    }

    void Merge_identical_items()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in ColumnNumbers(inputDir, width))
            {
                int next = FundTwinItem(y, x, Dir(inputDir));
                if (next == -1)
                    continue;
                Action_mergeItems(y, x, next);
            }
    }

    void Move_items()
    {
        for (int y = 0; y < height; y++)
            foreach (int x in ColumnNumbers(inputDir, width))
            {
                if (EmptyItem(y, x))
                    continue;
                int next = FindEmptyItem(y, x, Dir(inputDir));
                if (next == x)
                    continue;
                Action_moveItem(y, x, next);
            }
    }

    private void Input_direction(GameTurnInput i)
    {
        inputDir = (InputDirection)Enum.Parse(typeof(InputDirection), i.Move);
    }

    private void Reload_Board(Game i)
    {
        width = i.Width;
        height = i.Height;
        matrix = i.board;
    }

    private void Return()
    {
        throw new NotImplementedException();
    }

    // FUNCTION(S ) *******************************************************

    IEnumerable<int> ColumnNumbers(InputDirection move, int width)
    {
        HorizontalMovement mov = (move == InputDirection.R) ? HorizontalMovement.R : HorizontalMovement.L;
        IEnumerable<int>  nbr = Enumerable.Range(0, width);
        return (mov == HorizontalMovement.L) ? nbr : nbr.Reverse();
    }

    int Dir(InputDirection move)
    {
        HorizontalMovement mov = (move == InputDirection.R) ? HorizontalMovement.R : HorizontalMovement.L;
        return (mov == HorizontalMovement.L) ? -1 : 1;
    }

    int FindEmptyItem(int y, int xStart, int direction)
    {
        int x = xStart;
        bool emptyItemFound = true;
        while (emptyItemFound)
        {
            int next = x + direction;
            emptyItemFound = (next >= 0 && next < width) && EmptyItem(y, next);
            if (emptyItemFound)
                return next;
            else
                return x;
        }
        throw new Exception();
    }

    Point[] GetEmptyItems()
    {
        List<Point> res = new List<Point>();
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (FREE.Equals(matrix[y, x]))
                    res.Add(new Point(y, x));
        return res.ToArray();
    }

    bool EmptyItem(int y, int x)
    {
        return FREE.Equals(matrix[y, x]);
    }

    void Action_mergeItems(int y, int x, int next)
    {
        int val = matrix[y, x] * 2;
        matrix[y, x] = val;
        matrix[y, next] = FREE;
    }

    void Action_moveItem(int y, int x, int next)
    {
        matrix[y, next] = matrix[y, x];
        matrix[y, x] = FREE;
    }

    int FundTwinItem(int y, int xCurrent, int direction)
    {
        int x = xCurrent + direction;
        while (x >= 0 && x < width)
            if (matrix[y, xCurrent].Equals(matrix[y, x]))
                return x;
            else if (!FREE.Equals(matrix[y, x]))
                break;
            else
                x += direction;
        return -1;
    }
}
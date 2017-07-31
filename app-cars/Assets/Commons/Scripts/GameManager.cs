using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

// API *******************************************************

public interface IGameManager
{
    Game Start(GameStartInput input);
    Game Turn(GameTurnInput input);
    Game Reload(Game input);
}
public class Game
{
    public int Width;
    public int Height;
    public int[,] Board;
    public GameState State;
    public int Score;
    public InputDirection[] AvailableMoves;
}
public class GameStartInput
{
    public int Width;
    public int Height;
}
public class GameTurnInput
{
    public string Move;
}
public enum GameState { Playing, Won, Loss }
public enum InputDirection { Left, Right, Top, Bottom }

// IMPL *******************************************************

public class GameManager : Game, IGameManager
{
    public Game Start(GameStartInput input)
    { // Starting_the_Game_Turn
        { // Setup_Phase
            Clean_game_states();
            Update_input(input);
            Fill_with_zeros_items();
            Create_random_item();
            Create_random_item();
        }
        { // End
            Update_score();
            //  TODO     .Next(  available_moves )))
        }
        return Return();
    }

    public Game Turn(GameTurnInput input)
    { // Player Turn
        { // Beginning phase
            Input_direction(input);
        }
        {  // Move phase
            Merge_identical_items();
            Move_items();
            Create_random_item();
        }
        { // End
            Update_score();
            //  TODO     .Next(  available_moves )))
        }
        return Return();
    }

    public Game Reload(Game input)
    { // Starting_the_Game_Turn
        { // Setup_Phase
            Clean_game_states();
            Reload_Board(input);
        }
        { // End
            Update_score();
            //  TODO     .Next(  available_moves )))
        }
        return Return();
    }

    // TODO next(Turns.Get( "Ending the Game" )
    //  TODO .Start( end )
    //  TODO .Next(  available_moves )))   

    //  OBJECT *******************************************************
    // TODO external Game object ?

    private enum HorizontalMovement { Left, Right }
    private enum VerticalMovement { Top, Bottom }
    private const int NEW = 2;
    private const int FREE = 0;
    private class Point { public int y; public int x; public Point(int y, int x) { this.y = y; this.x = x; } }

    InputDirection inputDir;
    // TODO string[][][] histoStates;
    // TODO string[] histoEvents;

    void Update_input(GameStartInput input)
    {
        Width = input.Width;
        Height = input.Height;
    }

    void Clean_game_states()
    {
        Width = -1;
        Height = -1;
        Board = null;
        Score = -1;
    }

    void Update_score()
    {
        State = GameState.Playing;
        Score = 0;
    }

    void Fill_with_zeros_items()
    {
        Board = new int[Height, Width];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                Board[y, x] = FREE;
    }

    void Create_random_item()
    {
        Point[] freeItems = GetEmptyItems();
        int i = new Random().Next(0, freeItems.Length);
        Board[freeItems[i].y, freeItems[i].x] = NEW;
    }

    void Input_direction(GameTurnInput i)
    {
        inputDir = (InputDirection)Enum.Parse(typeof(InputDirection), i.Move);
    }

    void Reload_Board(Game i)
    {
        Width = i.Width;
        Height = i.Height;
        Board = i.Board;
    }

    Game Return()
    {
        return new Game()
        {
            Width = Width,
            Height = Height,
            Board = Board
        };
    }

    void Merge_identical_items()
    {
        for (int y = 0; y < Height; y++)
            foreach (int x in ColumnNumbers(inputDir, Width))
                if (!FREE.Equals(Board[y, x]))
                {
                    int next = FindTwinItem(y, x, Dir(inputDir));
                    if (next != -1)
                    {
                        Board[y, x] = Board[y, x] * 2;
                        Board[y, next] = FREE;
                    }
                }
    }

    void Move_items()
    {
        for (int y = 0; y < Height; y++)
            foreach (int x in ColumnNumbers(inputDir, Width))
                if (!FREE.Equals(Board[y, x]))
                {
                    int next = FindEmptyItem(y, x, Dir(inputDir));
                    if (next != x)
                    {
                        Board[y, next] = Board[y, x];
                        Board[y, x] = FREE;
                    }
                }
    }

    // FUNCTION(S ) *******************************************************


    int FindTwinItem(int y, int xCurrent, int direction)
    {
        int x = xCurrent + direction;
        while (x >= 0 && x < Width)
            if (Board[y, xCurrent].Equals(Board[y, x]))
                return x;
            else if (!FREE.Equals(Board[y, x]))
                break;
            else
                x += direction;
        return -1;
    }

    IEnumerable<int> ColumnNumbers(InputDirection move, int width)
    {
        HorizontalMovement mov = (move == InputDirection.Right) ? HorizontalMovement.Right : HorizontalMovement.Left;
        IEnumerable<int> nbr = Enumerable.Range(0, width);
        return (mov == HorizontalMovement.Left) ? nbr : nbr.Reverse();
    }

    int Dir(InputDirection move)
    {
        HorizontalMovement mov = (move == InputDirection.Right) ? HorizontalMovement.Right : HorizontalMovement.Left;
        return (mov == HorizontalMovement.Left) ? -1 : 1;
    }

    int FindEmptyItem(int y, int xStart, int direction)
    {
        int x = xStart;
        bool emptyItemFound = true;
        while (emptyItemFound)
        {
            int next = x + direction;
            emptyItemFound = (next >= 0 && next < Width) && FREE.Equals(Board[y, next]);
            if (emptyItemFound)
                x = next;
        }
        return x;
    }


    Point[] GetEmptyItems()
    {
        List<Point> res = new List<Point>();
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                if (FREE.Equals(Board[y, x]))
                    res.Add(new Point(y, x));
        return res.ToArray();
    }


}
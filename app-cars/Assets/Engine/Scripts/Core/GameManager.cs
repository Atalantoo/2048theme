using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace Project2048.Core
{

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
        public Item[,] Board;
        public int Score;
        public GameState State;
        public Movement[] AvailableMoves;
        public Movement LastMove;
    }
    public class GameStartInput
    {
        public int Width;
        public int Height;
    }
    public class GameTurnInput
    {
        public Movement Move;
    }
    public enum GameState { Playing, Won, Loss }
    public enum Movement { Left, Right, Up, Down }
    public class Item { public int Value; public Item(int value) { Value = value; } public string ToString() { return "" + Value; } }

    // IMPL *******************************************************

    public class GameManager : Game, IGameManager
    {
        public Game Start(GameStartInput input)
        { // Starting_the_Game_Turn
            { // Setup_Phase
                Clean();
                Setup(input);
                Fill_with_zeros_items();
                Create_random_item();
                Create_random_item();
            }
            { // End
                Get_available_moves();
                Update_State();
                Update_score();
            }
            return Return();
        }

        public Game Turn(GameTurnInput input)
        { // Player Turn
            { // Beginning phase
                Get_Input(input);
            }
            {  // Move phase
                Move_Items();
                Create_random_item();
            }
            { // End
                Get_available_moves();
                Update_State();
                Update_score();
            }
            return Return();
        }

        public Game Reload(Game input)
        { // Starting_the_Game_Turn
            { // Setup_Phase
                Clean();
                Setup(input);
            }
            { // End
                Get_available_moves();
                Update_State();
                Update_score();
            }
            return Return();
        }

        // TODO next(Turns.Get( "Ending the Game" )

        // OBJECT *******************************************************

        private const int NEW = 2;
        private const int FREE = 0;
        private const int MAX = 2048;
        private int[] score_values;
        private class Point { public int y; public int x; public Point(int y, int x) { this.y = y; this.x = x; } }

        // TODO string[][][] histoStates;

        public GameManager()
        {
            score_values = new int[2049];
            score_values[0] = 0;
            score_values[2] = 0;
            score_values[4] = 4;
            score_values[8] = 8 + 2 * score_values[4];
            score_values[16] = 16 + 2 * score_values[8];
            score_values[32] = 32 + 2 * score_values[16];
            score_values[64] = 64 + 2 * score_values[32];
            score_values[128] = 128 + 2 * score_values[64];
            score_values[256] = 256 + 2 * score_values[128];
            score_values[512] = 512 + 2 * score_values[256];
            score_values[1024] = 1024 + 2 * score_values[512];
            score_values[2048] = 2048 + 2 * score_values[1024];
        }

        private void Clean()
        {
            Width = -1;
            Height = -1;
            Board = null;
            Score = -1;
        }

        private void Setup(GameStartInput input)
        {
            Width = input.Width;
            Height = input.Height;
            Board = new Item[Height, Width];
        }

        private void Setup(Game i)
        {
            Width = i.Width;
            Height = i.Height;
            Board = i.Board;
        }

        private void Get_Input(GameTurnInput input)
        {
            LastMove = input.Move;
        }

        private Game Return()
        {
            return new Game()
            {
                Width = Width,
                Height = Height,
                Board = Board,
                Score = Score,
                State = State,
                AvailableMoves = AvailableMoves
            };
        }

        private void Update_score()
        {
            Score = Calc_score(Board);
        }

        public int Calc_score(Item[,] matrix)
        {
            int res = 0;
            for (int y = 0; y < matrix.GetLength(0); y++)
                for (int x = 0; x < matrix.GetLength(1); x++)
                    res += score_values[matrix[y, x].Value];
            return res;
        }

        private void Update_State()
        {
            if (GetItemsByValue(Board, 2048).Length > 0)
                State = GameState.Won;
            else if (AvailableMoves.Length > 0)
                State = GameState.Playing;
            else
                State = GameState.Loss;
        }

        void Fill_with_zeros_items()
        {
            for (int y = 0; y < Board.GetLength(0); y++)
                for (int x = 0; x < Board.GetLength(1); x++)
                    Board[y, x] = new Item(FREE);
        }

        private void Create_random_item()
        {
            Point[] freeItems = GetItemsByValue(Board, FREE);
            int i = new Random().Next(0, freeItems.Length);
            Console.WriteLine(i);
            Board[freeItems[i].y, freeItems[i].x].Value = NEW;
        }

        private void Move_Items()
        {
            Item[,] actu = Board;
            Item[,] prev = null;
            bool canMove = true;
            while (canMove)
            {
                // BUGFIX too much simultaneous merge
                prev = Clone2(actu);
                switch (LastMove)
                {
                    case Movement.Right:
                        for (int y = 0; y < Height; y++)
                            for (int x = Width - 2; x >= 0; x--)
                                Move(actu[y, x], actu[y, x + 1]);
                        break;
                    case Movement.Left:
                        for (int y = 0; y < Height; y++)
                            for (int x = 1; x < Width; x++)
                                Move(actu[y, x], actu[y, x - 1]);
                        break;
                    case Movement.Up:
                        for (int x = 0; x < Width; x++)
                            for (int y = 1; y < Height; y++)
                                Move(actu[y, x], actu[y - 1, x]);
                        break;
                    case Movement.Down:
                        for (int x = 0; x < Width; x++)
                            for (int y = Height - 2; y >= 0; y--)
                                Move(actu[y, x], actu[y + 1, x]);
                        break;
                }
                Console.WriteLine("loop");
                canMove = !Equals2(prev, actu);
            }
        }

        private bool Equals2(Item[,] a, Item[,] b)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (a[y, x].Value != b[y, x].Value)
                        return false;
            return true;
        }

        private Item[,] Clone2(Item[,] src)
        {
            Item[,] dest = new Item[Height, Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    dest[y, x] = new Item(src[y, x].Value);
            return dest;
        }

        private void Move(Item from, Item to)
        {
            if (FREE != from.Value)
                if (FREE == to.Value)
                {
                    to.Value = from.Value;
                    from.Value = FREE;
                }
                else if (from.Value == to.Value && to.Value < MAX)
                {
                    to.Value += from.Value;
                    from.Value = FREE;
                }
        }

        private void Get_available_moves()
        {
            if (State == GameState.Playing)
                AvailableMoves = Calc_available_moves(Board);
            else
                AvailableMoves = new Movement[0];
        }


        // FUNCTION(S ) *******************************************************

        public static Movement[] Calc_available_moves(Item[,] matrix)
        {
            List<Movement> moves = new List<Movement>();
            bool hasFree = GetItemsByValue(matrix, FREE).Length > 0;
            if (hasFree)
            {
                moves.Add(Movement.Up);
                moves.Add(Movement.Down);
                moves.Add(Movement.Left);
                moves.Add(Movement.Right);
            }
            else
            {
                if (HasVerticalMovement(matrix))
                {
                    moves.Add(Movement.Up);
                    moves.Add(Movement.Down);

                }
                if (HasHorizontalMovement(matrix))
                {
                    moves.Add(Movement.Left);
                    moves.Add(Movement.Right);
                }
            }
            return moves.ToArray<Movement>();
        }

        // TODO simulation (robust)
        [System.Obsolete]
        private static bool HasVerticalMovement(Item[,] matrix)
        {
            for (int y = 0; y < matrix.GetLength(0) - 1; y++)
                for (int x = 0; x < matrix.GetLength(1); x++)
                    if (matrix[y, x] == matrix[y + 1, x])
                        return true;
            return false;
        }

        // TODO simulation (robust)
        [System.Obsolete]
        private static bool HasHorizontalMovement(Item[,] matrix)
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
                for (int x = 0; x < matrix.GetLength(1) - 1; x++)
                    if (matrix[y, x] == matrix[y, x + 1])
                        return true;
            return false;
        }

        private static Point[] GetItemsByValue(Item[,] matrix, int value)
        {
            List<Point> res = new List<Point>();
            for (int y = 0; y < matrix.GetLength(0); y++)
                for (int x = 0; x < matrix.GetLength(1); x++)
                    if (value.Equals(matrix[y, x].Value))
                        res.Add(new Point(y, x));
            return res.ToArray();
        }
    }
}
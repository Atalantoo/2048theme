/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace Project2048.Core
{
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
        private static int[] score_values = Build_score_values();
        private class Point { public int y; public int x; public Point(int y, int x) { this.y = y; this.x = x; } }

        // TODO string[][][] histoStates;

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

        public static int Calc_score(Item[,] Board)
        {
            int res = 0;
            for (int y = 0; y < Board.GetLength(0); y++)
                for (int x = 0; x < Board.GetLength(1); x++)
                    res += score_values[Board[y, x].Value];
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
            if (freeItems.Length > 0)
            {
                int i = new Random().Next(0, freeItems.Length);
                Point newItem = freeItems[i];
                Board[newItem.y, newItem.x].Value = NEW;
            }
        }

        private void Move_Items()
        {
            Item[,] actu = Board;
            Item[,] prev = null;
            bool canMove = true;
            while (canMove)
            {
                prev = Clone_Board(actu);
                actu = Move_Items(actu, LastMove);
                canMove = !Equals_Board(prev, actu);
            }
            Board = actu;
        }

        private static Item[,] Move_Items(Item[,] board, Movement move)
        {
            Item[,] array = Clone_Board(board);
            int Height = board.GetLength(0);
            int Width = board.GetLength(1);
            switch (move)
            {
                case Movement.Right:
                    for (int y = 0; y < Height; y++)
                        for (int x = Width - 2; x >= 0; x--)
                            Move(array[y, x], array[y, x + 1]);
                    break;
                case Movement.Left:
                    for (int y = 0; y < Height; y++)
                        for (int x = 1; x < Width; x++)
                            Move(array[y, x], array[y, x - 1]);
                    break;
                case Movement.Up:
                    for (int x = 0; x < Width; x++)
                        for (int y = 1; y < Height; y++)
                            Move(array[y, x], array[y - 1, x]);
                    break;
                case Movement.Down:
                    for (int x = 0; x < Width; x++)
                        for (int y = Height - 2; y >= 0; y--)
                            Move(array[y, x], array[y + 1, x]);
                    break;
            }
            return array;
        }

        private static bool Equals_Board(Item[,] a, Item[,] b)
        {
            int Height = a.GetLength(0);
            int Width = a.GetLength(1);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (a[y, x].Value != b[y, x].Value)
                        return false;
            return true;
        }

        private static Item[,] Clone_Board(Item[,] src)
        {
            int Height = src.GetLength(0);
            int Width = src.GetLength(1);
            Item[,] dest = new Item[Height, Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    dest[y, x] = new Item(src[y, x].Value);
            return dest;
        }

        private static void Move(Item from, Item to)
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
            AvailableMoves = Calc_available_moves(Board);
        }

        // FUNCTION(S ) *******************************************************

        public static int[] Build_score_values()
        {
            int[] score_values;
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
            return score_values;
        }

        public static Movement[] Calc_available_moves(Item[,] matrix)
        {
            Item[,] prev = Clone_Board(matrix);
            List<Movement> moves = new List<Movement>();
            if (!Equals_Board(prev, Move_Items(matrix, Movement.Up)))
                moves.Add(Movement.Up);
            if (!Equals_Board(prev, Move_Items(matrix, Movement.Down)))
                moves.Add(Movement.Down);
            if (!Equals_Board(prev, Move_Items(matrix, Movement.Left)))
                moves.Add(Movement.Left);
            if (!Equals_Board(prev, Move_Items(matrix, Movement.Right)))
                moves.Add(Movement.Right);
            return moves.ToArray<Movement>();
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
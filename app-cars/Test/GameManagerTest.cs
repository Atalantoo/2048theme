using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Test;

[TestClass]
public class GameManagerUnitTest
{

    [TestMethod]
    public void Usecase_01_init()
    {
        string[][] inp = GameTest.readFile("../../usecase_01_init-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_01_init-o.txt");
        string[][] res = FromOutputToArray(game.Start(
            FromArrayToInput(inp)));
        Assert.IsNotNull(exp);
        Assert.IsNotNull(res);
        DisplayResult(res);
        Assert.AreEqual(exp.Length, res.Length);
        Assert.IsTrue(14 <= Count(res, "0"));
        Assert.AreEqual(2, Count(res, "2"));
    }

    [TestMethod] public void Usecase_02_move_mid_to_left() => TestReloadThenTurn("usecase_02_move_mid_to_left");
    [TestMethod] public void Usecase_02_move_mid_to_right() => TestReloadThenTurn("usecase_02_move_mid_to_right");
    [TestMethod] public void Usecase_03_move_twins_left() => TestReloadThenTurn("usecase_03_move_twins_left_to_left");
    [TestMethod] public void Usecase_03_move_twins_mid_to_right() => TestReloadThenTurn("usecase_03_move_twins_mid_to_right");
    [TestMethod] public void Usecase_04_move_blocked_twins_mid_to_right() => TestReloadThenTurn("usecase_04_move_blocked_twins_mid_to_right");
    [TestMethod] public void Usecase_04_move_spaced_twins_mid_to_right() => TestReloadThenTurn("usecase_04_move_spaced_twins_mid_to_right");
    [TestMethod] public void Usecase_05_move_bot() => TestReloadThenTurn("usecase_05_move_bot");
    [TestMethod] public void Usecase_05_move_top() => TestReloadThenTurn("usecase_05_move_top");
    [TestMethod] public void Usecase_06_move_spaced_twins_mid_to_top() => TestReloadThenTurn("usecase_06_move_spaced_twins_mid_to_top");
    [TestMethod] public void Usecase_07_won() => TestReload("usecase_07_won");
    [TestMethod] public void Usecase_07_loss() => TestReload("usecase_07_loss");
    [TestMethod] public void Usecase_fix_01_missing_item_on_border() => TestReloadThenTurn("usecase_fix_01_missing_item_on_border");

    [TestMethod]
    public void Test_Calc_score()
    {
        Assert.AreEqual(0,
            game.Calc_score(Arrays(new int[,]{
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 }
                })));
        Assert.AreEqual(0,
            game.Calc_score(Arrays(new int[,]{
                { 0,0,0,2 },
                { 0,0,0,2 },
                { 0,0,0,0 },
                { 0,0,0,0 }
                })));
        Assert.AreEqual(4,
            game.Calc_score(Arrays(new int[,]{
                { 0,0,0,4 },
                { 0,0,0,2 },
                { 0,0,0,0 },
                { 0,0,0,0 }
                })));
        Assert.AreEqual(16,
            game.Calc_score(Arrays(new int[,]{
                { 0,0,0,8 },
                { 0,0,0,2 },
                { 0,0,0,2 },
                { 2,0,0,2 }
                })));
        Assert.AreEqual(24,
            game.Calc_score(Arrays(new int[,]{
                { 0,2,2,8 },
                { 0,0,0,4 },
                { 0,0,0,4 },
                { 0,0,0,0 }
                })));
        Assert.AreEqual(32,
            game.Calc_score(Arrays(new int[,]{
                { 0,2,2,8 },
                { 0,0,2,8 },
                { 0,0,0,0 },
                { 0,0,0,0 }
                })));
        Assert.AreEqual(52,
            game.Calc_score(Arrays(new int[,]{
                { 0,2,4,16 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 2,0,0,0 }
                })));
    }

    [TestMethod]
    public void Test_Calc_moves()
    {
        Assert.AreEqual(4,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 0,2,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 2,0,0,0 }
                })).Length);
        Assert.AreEqual(0,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,2 },
                { 2,4,2,4 }
                })).Length);
        Assert.AreEqual(4,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,2 },
                { 2,4,2,0 }
                })).Length);
        Assert.AreEqual(2,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,32 },
                { 2,4,2,32 }
                })).Length);
        Assert.AreEqual(Movement.Top,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,32 },
                { 2,4,8,32 }
                }))[0]);
        Assert.AreEqual(Movement.Bottom,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,32 },
                { 2,4,8,32 }
                }))[1]);
        Assert.AreEqual(Movement.Left,
            GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,2 },
                { 2,4,32,32 }
                }))[0]);
    }

    public static Item[,] Arrays(int[,] i)
    {
        Item[,] o;
        int h = i.GetLength(0);
        int w = i.GetLength(1);
        o = new Item[h, w];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                o[y, x] = new Item(i[y, x]);
        return o;
    }

    public static int[,] Transform(Item[,] i)
    {
        int[,] o;
        int h = i.GetLength(0);
        int w = i.GetLength(1);
        o = new int[h, w];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                o[y, x] = i[y, x].Value;
        return o;
    }

    GameTurnInput FromArrayToInput2(string[][] input)
    {
        return new GameTurnInput()
        {
            Move = (Movement)Enum.Parse(typeof(Movement), input[5][0])
        };
    }

    private string[][] FromOutputToArray(Game output)
    {
        string[][] res = new string[1 + 4 + 1 + 1 + 1][];
        res[0] = new string[] { output.Width.ToString(), output.Height.ToString() };
        for (int i = 0; i < 4; i++)
        {
            res[i + 1] = new string[4];
            for (int j = 0; j < 4; j++)
            {
                res[i + 1][j] = "" + output.Board[i, j].Value;
            }
        }
        res[5] = new string[] { output.Score.ToString() };
        res[6] = new string[] { output.State.ToString() };
        res[7] = new string[output.AvailableMoves.Length + 1];
        res[7][0] = output.AvailableMoves.Length.ToString();
        for (int i = 0; i < output.AvailableMoves.Length; i++)
        {
            res[7][i + 1] = output.AvailableMoves[i].ToString();
        }
        return res;
    }

    
    GameStartInput FromArrayToInput(string[][] input)
    {
        return new GameStartInput()
        {
            Width = Int32.Parse(input[0][0]),
            Height = Int32.Parse(input[0][1])
        };
    }
    Game FromArrayToInput3(string[][] input)
    {
        Game g = new Game()
        {
            Width = Int32.Parse(input[0][0]),
            Height = Int32.Parse(input[0][1]),
        };
        g.Board = new Item[,]{
            { new Item(Int32.Parse(input[1][0])), new Item(Int32.Parse(input[1][1])), new Item(Int32.Parse(input[1][2])), new Item(Int32.Parse(input[1][3])) },
            { new Item(Int32.Parse(input[2][0])), new Item(Int32.Parse(input[2][1])), new Item(Int32.Parse(input[2][2])), new Item(Int32.Parse(input[2][3])) },
            { new Item(Int32.Parse(input[3][0])), new Item(Int32.Parse(input[3][1])), new Item(Int32.Parse(input[3][2])), new Item(Int32.Parse(input[3][3])) },
            { new Item(Int32.Parse(input[4][0])), new Item(Int32.Parse(input[4][1])), new Item(Int32.Parse(input[4][2])), new Item(Int32.Parse(input[4][3])) }
        };
        return g;
    }
    private int Count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 1; y < 5; y++)
            for (int x = 0; x < 4; x++)
                if (match.Equals(matrix[y][x]))
                    res++;
        return res;
    }

    private void DisplayResult(string[][] res)
    {
        Console.WriteLine(res[0][0] + " " + res[0][1]);
        for (int i = 1; i < 5; i++)
            Console.WriteLine(res[i][0] + " " + res[i][1] + " " + res[i][2] + " " + res[i][3]);
        Console.WriteLine(res[5][0]);
        Console.WriteLine(res[6][0]);
        foreach (string s in res[7])
            Console.Write(s + " ");
    }

    static GameManager game = Init();
    static GameManager Init()
    {
        GameManager game;
        game = new GameManager();
        return game;
    }

    public void TestReload(string usecase)
    {
        string[][] inp = GameTest.readFile("../../" + usecase + "-i.txt");
        string[][] exp = GameTest.readFile("../../" + usecase + "-o.txt");
        string[][] res = FromOutputToArray(
            game.Reload(
                FromArrayToInput3(inp)));
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }

    public void TestReloadThenTurn(string usecase)
    {
        string[][] inp = GameTest.readFile("../../" + usecase + "-i.txt");
        string[][] exp = GameTest.readFile("../../" + usecase + "-o.txt");
        game.Reload(
            FromArrayToInput3(inp));
        string[][] res = FromOutputToArray(
            game.Turn(
                FromArrayToInput2(inp)));
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }
}


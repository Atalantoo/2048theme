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

    [TestMethod] public void Usecase_02_move_mid_to_left() => Common("usecase_02_move_mid_to_left");
    [TestMethod] public void Usecase_02_move_mid_to_right() => Common("usecase_02_move_mid_to_right");
    [TestMethod] public void Usecase_03_move_twins_left() => Common("usecase_03_move_twins_left_to_left");
    [TestMethod] public void Usecase_03_move_twins_mid_to_right() => Common("usecase_03_move_twins_mid_to_right");
    [TestMethod] public void Usecase_04_move_blocked_twins_mid_to_right() => Common("usecase_04_move_blocked_twins_mid_to_right");
    [TestMethod] public void Usecase_04_move_spaced_twins_mid_to_right() => Common("usecase_04_move_spaced_twins_mid_to_right");
    [TestMethod] public void Usecase_05_move_bot() => Common("usecase_05_move_bot");
    [TestMethod] public void Usecase_05_move_top() => Common("usecase_05_move_top");

    static GameManager game = Init();
    static GameManager Init()
    {
        GameManager game;
        game = new GameManager();
        return game;
    }

    public void Common(string usecase)
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
        g.Board = new int[,]{
            { Int32.Parse(input[1][0]), Int32.Parse(input[1][1]), Int32.Parse(input[1][2]), Int32.Parse(input[1][3]) },
            { Int32.Parse(input[2][0]), Int32.Parse(input[2][1]), Int32.Parse(input[2][2]), Int32.Parse(input[2][3]) },
            { Int32.Parse(input[3][0]), Int32.Parse(input[3][1]), Int32.Parse(input[3][2]), Int32.Parse(input[3][3]) },
            { Int32.Parse(input[4][0]), Int32.Parse(input[4][1]), Int32.Parse(input[4][2]), Int32.Parse(input[4][3]) }
        };
        return g;
    }
    GameTurnInput FromArrayToInput2(string[][] input)
    {
        return new GameTurnInput()
        {
            Move = input[5][0]
        };
    }

    private string[][] FromOutputToArray(Game output)
    {
        string[][] res = new string[4][];
        for (int i = 0; i < 4; i++)
        {
            res[i] = new string[4];
            for (int j = 0; j < 4; j++)
            {
                res[i][j] = "" + output.Board[i, j];
            }
        }
        return res;
    }

    // IMPL

    private int Count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
            for (int x = 0; x < matrix[0].Length; x++)
                if (match.Equals(matrix[y][x]))
                    res++;
        return res;
    }

    private void DisplayResult(string[][] res)
    {
        foreach (string[] row in res)
            Console.WriteLine(row[0] + " " + row[1] + " " + row[2] + " " + row[3]);
    }
}

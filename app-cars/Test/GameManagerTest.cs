using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Test;
using Commons.Lang;

[TestClass]
public class GameManagerUnitTest
{
    [TestMethod]
    public void Usecase_01_init()
    {
        string[][] inp = GameTest.readFile("../../usecase_01_init-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_01_init-o.txt");
        string[][] res = new GameManager().Init(inp);
        Assert.IsNotNull(exp);
        Assert.IsNotNull(res);
        DisplayResult(res);
        Assert.AreEqual(exp.Length, res.Length);
        Assert.IsTrue(14 <= Count(res, "0"));
        Assert.AreEqual(2, Count(res, "2"));
    }

    [TestMethod]
    public void Usecase_02_move_mid_to_left()
    {
        string[][] inp = GameTest.readFile("../../usecase_02_move_mid_to_left-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_02_move_mid_to_left-o.txt");
        string[][] res = new GameManager().Turn(inp);
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }

    [TestMethod]
    public void Usecase_02_move_mid_to_right()
    {
        string[][] inp = GameTest.readFile("../../usecase_02_move_mid_to_right-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_02_move_mid_to_right-o.txt");
        string[][] res = new GameManager().Turn(inp);
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }

    [TestMethod]
    public void Usecase_03_move_twins_left()
    {
        string[][] inp = GameTest.readFile("../../usecase_03_move_twins_left_to_left-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_03_move_twins_left_to_left-o.txt");
        string[][] res = new GameManager().Turn(inp);
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }

    [TestMethod]
    public void Usecase_03_move_twins_mid_to_right()
    {
        string[][] inp = GameTest.readFile("../../usecase_03_move_twins_mid_to_right-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_03_move_twins_mid_to_right-o.txt");
        string[][] res = new GameManager().Turn(inp);
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }

    [TestMethod]
    public void Usecase_04_move_blocked_twins_mid_to_right()
    {
        string[][] inp = GameTest.readFile("../../usecase_04_move_blocked_twins_mid_to_right-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_04_move_blocked_twins_mid_to_right-o.txt");
        string[][] res = new GameManager().Turn(inp);
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
    }

    [TestMethod]
    public void Usecase_04_move_spaced_twins_mid_to_right()
    {
        string[][] inp = GameTest.readFile("../../usecase_04_move_spaced_twins_mid_to_right-i.txt");
        string[][] exp = GameTest.readFile("../../usecase_04_move_spaced_twins_mid_to_right-o.txt");
        string[][] res = new GameManager().Turn(inp);
        DisplayResult(res);
        GameAssert.AreEqual(exp, res);
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
            Console.WriteLine(Joiner.On(" ").Join(row));
    }
}

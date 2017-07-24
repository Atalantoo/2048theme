using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Test;

[TestClass]
public class GameManagerUnitTest
{
    [TestMethod]
    public void usecase_01_init()
    {
        string[][] inp = CommonsTest.readFile("../../usecase_01_init-i.txt");
        string[][] exp = CommonsTest.readFile("../../usecase_01_init-o.txt");
        string[][] res = new GameManager().init(inp);
        Assert.IsNotNull(exp);
        Assert.IsNotNull(res);
        displayResult(res);
        Assert.AreEqual(exp.Length, res.Length);
        Assert.IsTrue(14 <= count(res, "0"));
        Assert.AreEqual(2, count(res, "2"));
    }

    [TestMethod]
    public void usecase_02_move_left()
    {
        string[][] inp = CommonsTest.readFile("../../usecase_02_move_left-i.txt");
        string[][] exp = CommonsTest.readFile("../../usecase_02_move_left-o.txt");
        string[][] res = new GameManager().turn(inp);
        displayResult(res);
        Assert2.AreEqual(exp, res);
    }

    [TestMethod]
    public void usecase_02_move_right()
    {
        string[][] inp = CommonsTest.readFile("../../usecase_02_move_right-i.txt");
        string[][] exp = CommonsTest.readFile("../../usecase_02_move_right-o.txt");
        string[][] res = new GameManager().turn(inp);
        displayResult(res);
        Assert2.AreEqual(exp, res);
    }

    [TestMethod]
    public void usecase_02_move_twins_right()
    {
        string[][] inp = CommonsTest.readFile("../../usecase_03_move_twins_right-i.txt");
        string[][] exp = CommonsTest.readFile("../../usecase_04_move_twins_right-o.txt");
        string[][] res = new GameManager().turn(inp);
        displayResult(res);
        Assert2.AreEqual(exp, res);
    }

    private int count(string[][] matrix, string match)
    {
        int res = 0;
        for (int y = 0; y < matrix.Length; y++)
        {
            for (int x = 0; x < matrix[0].Length; x++)
            {
                if (match.Equals(matrix[y][x]))
                    res++;
            }
        }
        return res;
    }

    private void displayResult(string[][] res)
    {
        foreach (string[] y in res)
        {
            string line = "";
            foreach (string x in y)
            {
                line += x + " ";
            }
            Console.WriteLine(line);
        }
    }
}

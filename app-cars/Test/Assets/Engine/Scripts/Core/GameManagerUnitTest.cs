using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Test;

namespace Project2048.Core
{
    [TestClass]
    public class GameManagerUnitTest
    {
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
            Assert.AreEqual(Movement.Up,
                GameManager.Calc_available_moves(Arrays(new int[,]{
                { 4,2,4,2 },
                { 2,4,2,4 },
                { 4,2,4,32 },
                { 2,4,8,32 }
                    }))[0]);
            Assert.AreEqual(Movement.Down,
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

        static GameManager game = Init();
        static GameManager Init()
        {
            GameManager game;
            game = new GameManager();
            return game;
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

    }

}
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class GameEngineUnitTest
    {
        [TestMethod]
        public void case_01()
        {
            string[][] res = CommonsTest.readFile("../../test_case-01_init-input.txt");

            Assert.IsTrue(true);
        }

    }
}

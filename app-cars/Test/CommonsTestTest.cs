using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class CommonsTestTest
    {
        [TestMethod]
        public void readFile()
        {
            string[][] res = CommonsTest.readFile("../../test_case-00-input.txt");
            Assert.IsNotNull(res);
            Assert.AreEqual(5, res.Length);
            Assert_AreEqual(new string[] { "4", "3" }, res[0]);
            Assert_AreEqual(new string[] { "0", "0", "0", "0" }, res[1]);
            Assert_AreEqual(new string[] { "0", "1", "0", "0" }, res[2]);
            Assert_AreEqual(new string[] { "0", "0", "X", "0" }, res[3]);
            Assert_AreEqual(new string[] { "R" }, res[4]);
        }



        public void Assert_AreEqual(string[] expected, string[] actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}

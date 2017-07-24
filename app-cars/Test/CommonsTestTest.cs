using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commons.Test
{
    [TestClass]
    public class CommonsTestTest
    {
        [TestMethod]
        public void usecase_00_input()
        {
            string[][] res = CommonsTest.readFile("../../usecase_00_input-i.txt");
            Assert.IsNotNull(res);
            Assert.AreEqual(5, res.Length);
            Assert2.AreEqual(new string[] { "4", "3" }, res[0]);
            Assert2.AreEqual(new string[] { "0", "0", "0", "0" }, res[1]);
            Assert2.AreEqual(new string[] { "0", "1", "0", "0" }, res[2]);
            Assert2.AreEqual(new string[] { "0", "0", "X", "0" }, res[3]);
            Assert2.AreEqual(new string[] { "R" }, res[4]);
        }

        [TestMethod]
        public void usecase_00_input_spaces()
        {
            string[][] res = CommonsTest.readFile("../../usecase_00_input_spaces-i.txt");
            Assert.IsNotNull(res);
            Assert.AreEqual(3, res.Length);
            Assert2.AreEqual(new string[] { "4", "3" }, res[0]);
            Assert2.AreEqual(new string[] { "5", "6" }, res[1]);
            Assert2.AreEqual(new string[] { "7", "8" }, res[2]);
        }


    }
}

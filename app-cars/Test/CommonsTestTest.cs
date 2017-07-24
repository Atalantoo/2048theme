using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class CommonsTestTest
    {
        [TestMethod]
        public void loadInput()
        {
            string[][] res = CommonsTest.loadInput("../../test_case-00-input.txt");
            Assert.IsNotNull(res);
            Assert.AreEqual(5, res.Length);
        }
    }
}

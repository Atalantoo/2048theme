using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Lang;

namespace Commons.Test
{
    [TestClass]
    public class CommonsLangUnitTest
    {
        [TestMethod]
        public void Joiner_str()
        {
            string[] arg = new string[] { "a", "b", "c" };
            string  res = Joiner.on("-").join(arg);
            Assert.AreEqual("a-b-c", res);
        }

        [TestMethod]
        public void Arrays_outOfBound_before()
        {
            string[] arg1 = new string[5];
            int arg2 = -1;
            bool res = Arrays.outOfBound(arg1, arg2);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Arrays_outOfBound_after()
        {
            string[] arg1 = new string[5];
            int arg2 = 5;
            bool res = Arrays.outOfBound(arg1, arg2);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Arrays_outOfBound_ok()
        {
            string[] arg1 = new string[5];
            int arg2 = 2;
            bool res = Arrays.outOfBound(arg1, arg2);
            Assert.IsFalse(res);
        }

    }
}

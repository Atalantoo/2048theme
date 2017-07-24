using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Lang;

namespace Commons.Test
{
    [TestClass]
    public class CommonsLangUnitTest
    {
        [TestMethod]
        public void Arrays_join_str()
        {
            string[] arg = new string[] { "a", "b", "c" };
            string  res = Arrays.join(arg, "-");
            Assert.AreEqual("a-b-c", res);
        }

        [TestMethod]
        public void Arrays_join_int()
        {
            int[] arg = new int[] { 1, 2, 3 };
            string res = Arrays.join(arg, ",");
            Assert.AreEqual("1,2,3", res);
        }
    }
}

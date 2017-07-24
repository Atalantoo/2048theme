using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commons.Lang;

namespace Commons.Test
{
    [TestClass]
    public class CommonsLangUnitTest
    {
        [TestMethod]
        public void Strings_replaceAll()
        {
            Assert.AreEqual("abc", Strings.replaceAll("abc", " ", ""));
            Assert.AreEqual("abc", Strings.replaceAll("a b c", " ", ""));
            Assert.AreEqual("abc", Strings.replaceAll("a b   c", " ", ""));
        }

        [TestMethod]
        public void Strings_isNullOrEmpty()
        {
            Assert.IsTrue(Strings.isNullOrEmpty(null));
            Assert.IsTrue(Strings.isNullOrEmpty(""));
            Assert.IsFalse(Strings.isNullOrEmpty(" "));
            Assert.IsFalse(Strings.isNullOrEmpty("  "));
            Assert.IsFalse(Strings.isNullOrEmpty("a"));
            Assert.IsFalse(Strings.isNullOrEmpty(" a"));
            Assert.IsFalse(Strings.isNullOrEmpty(" a "));
        }

        [TestMethod]
        public void Strings_isNullOrBlank()
        {
            Assert.IsTrue(Strings.isNullOrBlank(null));
            Assert.IsTrue(Strings.isNullOrBlank(""));
            Assert.IsTrue(Strings.isNullOrBlank(" "));
            Assert.IsTrue(Strings.isNullOrBlank("  "));
            Assert.IsFalse(Strings.isNullOrBlank("a"));
            Assert.IsFalse(Strings.isNullOrBlank(" a"));
            Assert.IsFalse(Strings.isNullOrBlank(" a "));
        }

        [TestMethod]
        public void Joiner_str()
        {
            Assert.AreEqual("a", Joiner.on("-").join(new string[] { "a" }));
            Assert.AreEqual("a-b", Joiner.on("-").join(new string[] { "a", "b" }));
            Assert.AreEqual("a-b-c", Joiner.on("-").join(new string[] { "a", "b", "c" }));
            Assert.AreEqual("a  b  c", Joiner.on("  ").join(new string[] { "a", "b", "c" }));
        }

        [TestMethod]
        public void Arrays_outOfBound()
        {
            string[] arg = new string[5];
            Assert.IsTrue(Arrays.outOfBound(arg, -1));
            Assert.IsFalse(Arrays.outOfBound(arg, 0));
            Assert.IsFalse(Arrays.outOfBound(arg, 1));
            Assert.IsFalse(Arrays.outOfBound(arg, 2));
            Assert.IsFalse(Arrays.outOfBound(arg, 3));
            Assert.IsFalse(Arrays.outOfBound(arg, 4));
            Assert.IsTrue(Arrays.outOfBound(arg, 5));
            Assert.IsTrue(Arrays.outOfBound(arg, 6));
            Assert.IsTrue(Arrays.outOfBound(arg, 7));
        }

    }
}

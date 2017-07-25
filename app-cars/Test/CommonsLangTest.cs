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
            Assert.AreEqual("abc", Strings.ReplaceAll("abc", " ", ""));
            Assert.AreEqual("abc", Strings.ReplaceAll("a b c", " ", ""));
            Assert.AreEqual("abc", Strings.ReplaceAll("a b   c", " ", ""));
        }

        [TestMethod]
        public void Strings_isNullOrEmpty()
        {
            Assert.IsTrue(Strings.NullOrEmpty(null));
            Assert.IsTrue(Strings.NullOrEmpty(""));
            Assert.IsFalse(Strings.NullOrEmpty(" "));
            Assert.IsFalse(Strings.NullOrEmpty("  "));
            Assert.IsFalse(Strings.NullOrEmpty("a"));
            Assert.IsFalse(Strings.NullOrEmpty(" a"));
            Assert.IsFalse(Strings.NullOrEmpty(" a "));
        }

        [TestMethod]
        public void Strings_isNullOrBlank()
        {
            Assert.IsTrue(Strings.NullOrBlank(null));
            Assert.IsTrue(Strings.NullOrBlank(""));
            Assert.IsTrue(Strings.NullOrBlank(" "));
            Assert.IsTrue(Strings.NullOrBlank("  "));
            Assert.IsFalse(Strings.NullOrBlank("a"));
            Assert.IsFalse(Strings.NullOrBlank(" a"));
            Assert.IsFalse(Strings.NullOrBlank(" a "));
        }

        [TestMethod]
        public void Joiner_str()
        {
            Assert.AreEqual("a", Joiner.On("-").Join(new string[] { "a" }));
            Assert.AreEqual("a-b", Joiner.On("-").Join(new string[] { "a", "b" }));
            Assert.AreEqual("a-b-c", Joiner.On("-").Join(new string[] { "a", "b", "c" }));
            Assert.AreEqual("a  b  c", Joiner.On("  ").Join(new string[] { "a", "b", "c" }));
        }

        [TestMethod]
        public void Arrays_outOfBound()
        {
            string[] arg = new string[5];
            Assert.IsTrue(Arrays.OutOfBound(arg, -1));
            Assert.IsFalse(Arrays.OutOfBound(arg, 0));
            Assert.IsFalse(Arrays.OutOfBound(arg, 1));
            Assert.IsFalse(Arrays.OutOfBound(arg, 2));
            Assert.IsFalse(Arrays.OutOfBound(arg, 3));
            Assert.IsFalse(Arrays.OutOfBound(arg, 4));
            Assert.IsTrue(Arrays.OutOfBound(arg, 5));
            Assert.IsTrue(Arrays.OutOfBound(arg, 6));
            Assert.IsTrue(Arrays.OutOfBound(arg, 7));
        }

    }
}

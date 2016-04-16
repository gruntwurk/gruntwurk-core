using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GruntWurk.StringUtils;

namespace GruntWurk.Tests {
    [TestClass()]
    public class StringUtilsTests {
        [TestMethod()]
        public void MidTest() {
            Assert.AreEqual("Bar", Mid("FooBarBaz", 4, 3));
            Assert.AreEqual("", Mid("Short", 12, 3));
            Assert.AreEqual("", Mid("", 12, 3));
        }

        [TestMethod()]
        public void LeftTest() {
            Assert.AreEqual("Foo", Left("FooBarBaz", 3));
            Assert.AreEqual("Short", Left("Short", 12));
            Assert.AreEqual("", Left("FooBarBaz", 0));
        }
        [TestMethod()]
        public void RightTest() {
            Assert.AreEqual("Baz", Right("FooBarBaz", 3));
            Assert.AreEqual("Short", Right("Short", 12));
            Assert.AreEqual("", Right("FooBarBaz", 0));
        }

        [TestMethod()]
        public void TestLTrimZeros() {
            Assert.AreEqual("1122", LTrimZeros("001122"));
            Assert.AreEqual("1122", LTrimZeros("1122"));
            Assert.AreEqual("A001122", LTrimZeros("A001122"));
            Assert.AreEqual("1000", LTrimZeros("001000"));
            Assert.AreEqual("1000 0000", LTrimZeros("001000 0000"));
            Assert.AreEqual("0", LTrimZeros("0"));
            Assert.AreEqual("0", LTrimZeros("00"));
            Assert.AreEqual("0", LTrimZeros("000000"));
        }

    }
}
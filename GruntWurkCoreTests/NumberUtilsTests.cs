using GruntWurk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GruntWurk.NumberUtils;

namespace GruntWurk.Tests {
    [TestClass()]
    public class NumberUtilsTests {
        [TestMethod()]
        public void IntOrDefaultTest() {
            Assert.AreEqual(27, IntOrDefault("27", 99));
            Assert.AreEqual(27, IntOrDefault("00027", 99));
            Assert.AreEqual(10027, IntOrDefault("10027", 99));
            Assert.AreEqual(99, IntOrDefault("10,027", 99));
            Assert.AreEqual(-27, IntOrDefault("-27", 99));
            Assert.AreEqual(99, IntOrDefault("", 99));
            Assert.AreEqual(99, IntOrDefault("FIVE", 99));
            Assert.AreEqual(27, IntOrDefault(" 27", 99));
            Assert.AreEqual(-27, IntOrDefault("  -27  ", 99));
            Assert.AreEqual(99, IntOrDefault("   ", 99));
        }

        [TestMethod()]
        public void StartOfRangeSpecTest() {
            Assert.AreEqual(1, StartOfRangeSpec("1..9"));
            Assert.AreEqual(1, StartOfRangeSpec("1..0"));
            Assert.AreEqual(1, StartOfRangeSpec("1..-5"));
            Assert.AreEqual(1, StartOfRangeSpec("1..N"));
            Assert.AreEqual(1, StartOfRangeSpec("1..N-5"));
            Assert.AreEqual(11, StartOfRangeSpec("11..9"));
            Assert.AreEqual(11, StartOfRangeSpec("11..0"));
            Assert.AreEqual(11, StartOfRangeSpec("11..-5"));
            Assert.AreEqual(11, StartOfRangeSpec("11..N"));
            Assert.AreEqual(11, StartOfRangeSpec("11..N-5"));
        }

        [TestMethod()]
        public void EndOfRangeSpecTest() {
            Assert.AreEqual(9, EndOfRangeSpec("1..9"));
            Assert.AreEqual(0, EndOfRangeSpec("1..0"));
            Assert.AreEqual(-5, EndOfRangeSpec("1..-5"));
            Assert.AreEqual(0, EndOfRangeSpec("1..N"));
            Assert.AreEqual(-5, EndOfRangeSpec("1..N-5"));
            Assert.AreEqual(9, EndOfRangeSpec("11..9"));
            Assert.AreEqual(0, EndOfRangeSpec("11..0"));
            Assert.AreEqual(-5, EndOfRangeSpec("11..-5"));
            Assert.AreEqual(0, EndOfRangeSpec("11..N"));
            Assert.AreEqual(-5, EndOfRangeSpec("11..N-5"));
        }
    }
}
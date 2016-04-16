using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GruntWurk.NumberUtils;

namespace GruntWurk.Tests
{
    [TestClass()]
    public class NumberUtilsTests
    {
        [TestMethod()]
        public void IntOrDefaultTest()
        {
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
    }
}
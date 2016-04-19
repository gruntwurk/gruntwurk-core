using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static GruntWurk.FileUtils;

namespace GruntWurk.Tests {
    [TestClass()]
    public class FileUtilsTests {
        [TestMethod()]
        public void TestAppendToFileName()
        {
            DateTime DEC_31_1999 = new DateTime(1999, 12, 31);
            DateTime JAN_01_2000 = new DateTime(2000, 1, 1);
            DateTime JAN_01_2015 = new DateTime(2015, 1, 1);
            DateTime SEP_09_2015 = new DateTime(2015, 9, 9);

            // Second argument is a string
            Assert.AreEqual("MyFile_Part_1.txt", AppendToFileName("MyFile.txt", "Part_1"));
            Assert.AreEqual("NoExtension_Part_1", AppendToFileName("NoExtension", "Part_1"));
            Assert.AreEqual("_Part_1.txt", AppendToFileName(".txt", "Part_1"));
            Assert.AreEqual("_Part_1", AppendToFileName("", "Part_1"));
            Assert.AreEqual("Periods.in.Name_Part_1.txt", AppendToFileName("Periods.in.Name.txt", "Part_1"));

            // Second argument is a DateTime
            Assert.AreEqual("MyFile_1999_12_31.txt", AppendToFileName("MyFile.txt", DEC_31_1999));
            Assert.AreEqual("MyFile_2000_01_01.txt", AppendToFileName("MyFile.txt", JAN_01_2000));
            Assert.AreEqual("NoExtension_2015_09_09", AppendToFileName("NoExtension", SEP_09_2015));
            Assert.AreEqual("_1999_12_31.txt", AppendToFileName(".txt", DEC_31_1999));
            Assert.AreEqual("_1999_12_31", AppendToFileName("", DEC_31_1999));
            Assert.AreEqual("Periods.in.Name_2015_01_01.txt", AppendToFileName("Periods.in.Name.txt", JAN_01_2015));
        }
    }
}
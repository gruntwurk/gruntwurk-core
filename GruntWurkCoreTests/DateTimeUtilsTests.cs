using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static GruntWurk.DateTimeUtils;

namespace GruntWurk.Tests {
    [TestClass()]
    public class DateTimeUtilsTests {
        [TestMethod()]
        public void FourDigitYearTest() {
            Assert.AreEqual(2000, FourDigitYear(0));
            Assert.AreEqual(2001, FourDigitYear(1));
            Assert.AreEqual(2014, FourDigitYear(14));
            Assert.AreEqual(2015, FourDigitYear(15));
            Assert.AreEqual(1920, FourDigitYear(20));
            Assert.AreEqual(1950, FourDigitYear(50));
            Assert.AreEqual(1999, FourDigitYear(99));
        }

        [TestMethod()]
        public void TestDateFromFileName() {
            DateTime MAY_12_2009 = new DateTime(2009, 5, 12);
            DateTime DEC_31_2011 = new DateTime(2011, 12, 31);
            DateTime APR_01_2014 = new DateTime(2014, 4, 1);
            DateTime DEC_31_2014 = new DateTime(2014, 12, 31);
            DateTime JAN_31_2015 = new DateTime(2015, 1, 31);
            DateTime FEB_27_2015 = new DateTime(2015, 2, 27);
            DateTime DEC_31_2015 = new DateTime(2015, 12, 31);
            DateTime JAN_31_2016 = new DateTime(2016, 1, 31);

            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile 20111231.mp3"));
            Assert.AreEqual(MAY_12_2009, DateFromFileName("FY10 MyFile.5.12.txt"));
            Assert.AreEqual(DEC_31_2014, DateFromFileName("FY15 MyFile.12.31.txt"));
            Assert.AreEqual(FEB_27_2015, DateFromFileName("FY15 MyFile.2.27.txt"));
            Assert.AreEqual(APR_01_2014, DateFromFileName("FY15 MyFile.4.1.txt"));
            Assert.AreEqual(DEC_31_2015, DateFromFileName("FY16 MyFile.12.31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile20111231.mp3"));
            Assert.AreEqual(DEC_31_2015, DateFromFileName("MyFile1231.mp3"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile.2011.12.31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile.2011_12_31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile+2011_12_31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile-2011-12-31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile 2011 12 31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile 2011.12.31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile_2011_12_31.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile_2011_12_31.mp3"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile 12.31.2011.txt"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile_111231.mp3"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile_11_12_31.mp3"));
            Assert.AreEqual(DEC_31_2011, DateFromFileName("MyFile_12_31_11.mp3"));

            // IMPORTANT: The following tests assume that the current date (now) is between 2/1/2016 and 12/30/2016
            // Assert.AreEqual(JAN_31_2016, DateFromFileName("MyFile_0131.mp3"));
            // Assert.AreEqual(DEC_31_2015, DateFromFileName("MyFile_1231.mp3"));
        }
    }
}
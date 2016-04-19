using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace GruntWurk.Tests {
    [TestClass()]
    public class PageReaderTests {
        private String MakeUpATestFile(int pageCount) {
            string tempFilename = Path.GetTempFileName();
            return tempFilename;
        }

        [TestMethod()]
        public void PageReaderTest_Empty() {
            String testFilename = MakeUpATestFile(0);
            PageReader page = new PageReader(testFilename);
            try {
                page.FetchNextPage();
                Assert.IsTrue(page.AtLastPage);
            } finally {
                File.Delete(testFilename);
            }
        }

        // TODO Some substantial tests here, please
   }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GruntWurk.Tests
{
    [TestClass()]
    public class IniFileTests {
        [TestMethod()]
        public void IniFileTest() {
            IniFile INI = new IniFile();
            Assert.AreEqual(0,INI.Sections.Count);
        }

        [TestMethod()]
        public void SimpleTest() {
            IniFile INI = new IniFile();
            INI.LoadText("[A]\nFoo=Bar");
            Assert.AreEqual(1, INI.Sections.Keys.Count);
            Assert.AreEqual(1, INI.Sections["A"].Keys.Count);
            Assert.AreEqual("Bar", INI.GetString("A","Foo",""));
        }
        [TestMethod()]
        public void ThreeSectionsOneEmptyTest() {
            IniFile INI = new IniFile();
            // [b] and [c] are lower case on purpose
            // [A] is continued at the end on purpose
            INI.LoadText("[A]\nFoo=Something\nBar=Something Else\n[b]\n[c]\nBing=12354\nBang=98765\nBong=5555\n[A]\nBaz=Another Thing\n");
            Assert.AreEqual(2, INI.Sections.Keys.Count);
            Assert.AreEqual(3, INI.Sections["A"].Keys.Count);
            Assert.IsFalse(INI.Sections.ContainsKey("B"));
            Assert.AreEqual(3, INI.Sections["C"].Keys.Count);
            Assert.AreEqual("Something", INI.GetString("A","Foo",""));
            Assert.AreEqual("Another Thing", INI.GetString("A","Baz",""));
        }


        [TestMethod()]
        public void CommentsThroughoutTest() {
            IniFile INI = new IniFile();
            INI.LoadText("; Opening comment\n[A]\nFoo=Something\n; Middle comment\nBar=Something Else\n[b]\n[c]\nBing=12354\nBang=98765\nBong=5555\n[A]\nBaz=Another Thing\n; Ending comment");
            Assert.AreEqual(2, INI.Sections.Count);
            Assert.AreEqual(3, INI.Sections["A"].Count);
            Assert.IsFalse(INI.Sections.ContainsKey("B"));
            Assert.AreEqual(3, INI.Sections["C"].Count);
            Assert.AreEqual("Something", INI.GetString("A","Foo",""));
            Assert.AreEqual("Another Thing", INI.GetString("A","Baz",""));
        }

        [TestMethod()]
        public void FindMaxSectionNameTest() {
            IniFile INI = new IniFile();
            INI.LoadText("[Part 2]\nFoo=Two\n[Part 3]\nFoo=Three\n[Part 1]\nFoo=One\n");
            string s = INI.FindMaxSectionName("Part");
            Assert.AreEqual("PART 3", s);
            Assert.AreEqual("Three", INI.GetString(s,"Foo",""));
        }

    }
}
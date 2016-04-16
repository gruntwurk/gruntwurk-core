using System.IO;
using System.Collections.Generic;
using static GruntWurk.QuickLog;

namespace GruntWurk {
    /// <summary>
    /// Reads a TextStream page-by-page (FF breaks)
    /// </summary>
    public class PageReader {
        public static int UNCOUNTED = int.MaxValue;

        public static char DC1_CHAR = '\x017';
        public static char[] WHITESPACE = { ' ' };

        private StreamReader ReportTextStream;
        string Buf;

        public int PageNo;

        public int PageCount = UNCOUNTED;
        public int LineCount;
        public List<string> PageLines;
        public bool AtLastPage;

        public PageReader(string FullyQualifiedFilename) {
            PageLines = new List<string>();
            OpenStream(FullyQualifiedFilename);
        }

        private void OpenStream(string FullyQualifiedFilename) {
            ReportTextStream = new StreamReader(FullyQualifiedFilename);
            PageNo = 0;
            AtLastPage = false;
        }

        /// <summary>
        /// Loads the PageLines array with the contents of the next page. 
        /// IMPORTANT: This routine assumes that any FF that occurs in the file will be at the beginning of a line (i.e. that it is immediately preceeded by a linefeed).
        /// </summary>
        public void FetchNextPage() {
            ClearPage();

            // First, start with the line buffer that was left over from the last page fetch (if any)
            if (PageNo > 0) {
                LineCount = 1;
                PageLines.Add(Buf);
                Buf = "";
            }

            PageNo++;

            while (ReportTextStream.Peek() != 0) {
                Buf = ReportTextStream.ReadLine();
                if (Buf == null) {
                    ClosePageReader();
                    return;
                }
                Buf = Buf.TrimEnd(null);
                if (Buf.Length > 0 && (Buf[0] == '\f' || Buf[0] == DC1_CHAR)) {
                    Buf = Buf.Substring(1);

                    if (Buf.Length == 0 && ReportTextStream.Peek() == 0) {
                        // There was nothing after the last FF but a blank line
                        ClosePageReader();
                        return;
                    }
                    if (!(PageNo == 1 && LineCount == 0)) {
                        // This marks the end of the page (unless the FF is at the very begining of the file)
                        return;
                    }
                }
                LineCount++;
                PageLines.Add(Buf);
            }
            ClosePageReader();
        }


        public void ClearPage() {
            PageLines.Clear();
            LineCount = 0;
        }

        private void ClosePageReader() {
            ReportTextStream.Close();
            AtLastPage = true;
        }


        // TODO CountPages() -- Read though the file ASAP and count the FF's.
        // TODO Invoke CountPages() if there are any Page specs that use "N-nn".

    }
}

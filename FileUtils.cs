using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static GruntWurk.NumberUtils;

namespace GruntWurk {
    public class FileUtils {

        /// <summary>
        /// Use the standard Windows file-open dialog box to select a single file.
        /// Returns either the selected file or the given default value
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="dialogTitle"></param>
        /// <returns></returns>
        public static string FilePickerSingle(String defaultValue, String dialogTitle = "Please select the file to use.") {
            String result;
            // Create an instance of the open file dialog box.
            OpenFileDialog fd = new OpenFileDialog();
            result = defaultValue;
            fd.InitialDirectory = Path.GetDirectoryName(defaultValue);
            fd.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            fd.FileName = defaultValue;
            fd.Title = dialogTitle;
            if (fd.ShowDialog() == DialogResult.OK) {
                // SelectedItems is not zero based
                result = fd.FileName;
            }
            return result;
        }
        public static string SystemTempFolderName() {
            return Path.GetTempPath();
        }

        public static string MyDocumentsFolderName() {
            return Environment.GetEnvironmentVariable("USERPROFILE") + "\\My Documents";
        }

        /// <summary>
        /// Combines the given file name with the given suffix while preserving the file extension.
        /// </summary>
        /// <returns></returns>
        public static string AppendToFileName(string FileName, string Suffix) {
            string BaseName = Path.GetFileNameWithoutExtension(FileName);
            string NameExt = Path.GetExtension(FileName);
            return BaseName + "_" + Suffix + NameExt;
        }

        /// <summary>
        /// Combines the given file name with the given date.
        /// </summary>
        /// <returns></returns>
        public static string AppendToFileName(string FileName, DateTime AssociatedDate) {
            return AppendToFileName(FileName, AssociatedDate.ToString("yyyy_MM_dd"));
        }


        public static string[] FilesInFolder(string FolderPath, string FileMask) {
            // var AllFileNames;
            string[] FileNames;
            DirectoryInfo FolderRef;

            if (FolderPath == "") {
                return new string[0];
            }
            FolderRef = new DirectoryInfo(FolderPath);
            FileInfo[] AllFiles = FolderRef.GetFiles(FileMask);
            FileNames = new string[AllFiles.Length];
            int FileCount = 0;
            foreach (FileInfo FileEntry in AllFiles) {
                FileNames[FileCount] = FileEntry.Name;
                FileCount++;
            }
            return FileNames;
        }

        /// <summary>
        /// Strips off the last period in the file name and everything after it (if there is one).
        /// </summary>
        /// <returns></returns>
        public static string FileNameSansExt(string FileName) {
            return Path.GetFileNameWithoutExtension(FileName);
        }


        /// <summary>
        /// Pulls a best-guess date out of a filename (e.g. "LogInfo_v001_151231.txt" becomes 12/31/2015).
        /// If nothing that looks like a date exists in the filename, then it returns a Zero date (30-Dec-1899).
        /// Punctuation is completely ignored. Leading zeroes are required to make the month and day 2 digits each and the year must be 0, 2, or 4 digits.
        /// The rightmost 2 digits are assumed to be the day.
        /// The middle 2 digits are assumed to be the month.
        /// The leftmost 0 to 4 digits are assumed to be the year.
        /// (If there are more than 8 digits, then the leading digits are ignored.)
        /// If the year is 2 digits, then +2000 is assumed.
        /// If no year is given then the current year is assumed, unless that would put the date in the future, in which case last year is assumed.
        /// </summary>
        /// <returns>The determined date; or DateTime.MinValue if a date cannot be determined.</returns>
        public static DateTime DateFromFileName(string FileName) {
            string OrigFileName;
            string[] Tokens;
            string[] Digits;
            int AllDigitsTokenCount = 0;
            int TokenIndex;
            MatchCollection Matches;
            int FiscalYear = 0;
            int m = 0;
            int d = 0;
            int Y = 0;
            DateTime LimitDate;
            LimitDate = DateTime.Now;

            OrigFileName = FileName;
            FileName = FileNameSansExt(FileName);

            // Fiscal Year is the only token with letters that we care about, so look for that first
            Matches = StringUtils.RegExpFindAll(FileName, "FY[0-9]+", false);
            if (Matches.Count >= 1) {
                FiscalYear = IntOrDefault(Matches[0].ToString().Substring(2), 0);
                // Strip out the token we just found so that we don't process it again
                FileName = StringUtils.RegExpReplace(FileName, "FY([0-9]+)", " ", false);
            }

            // Strip out all non-digits, replacing them with spaces, and then tokenize on the spaces
            FileName = StringUtils.RegExpReplace(FileName, "[^0-9]+", " ").ToUpper();
            Tokens = FileName.Split(StringUtils.JUST_SPACE);

            Digits = new string[(Tokens.Length > 2) ? Tokens.Length : 2];

            foreach (string Token in Tokens) {
                if (Token.Length > 0) {
                    AllDigitsTokenCount++;
                    Digits[AllDigitsTokenCount - 1] = Token;
                }
            } // Token

            if (AllDigitsTokenCount <= 0) {
                // The file name contains no digits at all, so nothing that looks like a date
                return DateTime.MinValue;
            }

            if (AllDigitsTokenCount == 1) {
                // The digits are all scrunched together, so split them apart
                if (Digits[0].Length == 4) {
                    Digits[1] = StringUtils.Right(Digits[0], 2);
                    Digits[0] = Digits[0].Substring(0, 2);
                    AllDigitsTokenCount = 2;
                } else if (Digits[0].Length >= 6) {
                    Digits[2] = StringUtils.Right(Digits[0], 2);
                    Digits[1] = StringUtils.Mid(Digits[0], Digits[0].Length - 3, 2);
                    Digits[0] = StringUtils.Left(Digits[0], Digits[0].Length - 4);
                    AllDigitsTokenCount = 3;
                } else {
                    // A single number of 1, 2, 3, or 5 digits makes no sense as a date
                    return DateTime.MinValue;
                }
            }

            if (AllDigitsTokenCount == 2) {
                if (FiscalYear > 0) {
                    Y = FiscalYear;
                    LimitDate = new DateTime(FiscalYear, 3, 31);
                } else {
                    Y = DateTime.Now.Year;
                }
                m = IntOrDefault(Digits[0], m);
                d = IntOrDefault(Digits[1], d);
            } else if (AllDigitsTokenCount >= 3) {
                // If there are more than 3 all-digit tokens, then we'll assume the date was tacked onto the end, so use the 3-rightmost tokens
                TokenIndex = AllDigitsTokenCount - 3;
                Y = IntOrDefault(Digits[TokenIndex], Y);
                m = IntOrDefault(Digits[TokenIndex + 1], m);
                d = IntOrDefault(Digits[TokenIndex + 2], d);
                if (Y > 2099 || d > 31 || m > 12) {
                    m = IntOrDefault(Digits[TokenIndex], m);
                    d = IntOrDefault(Digits[TokenIndex + 1], d);
                    Y = IntOrDefault(Digits[TokenIndex + 2], Y);
                    if (Y > 2099 || d > 31 || m > 12) {
                        // Neither Y/M/D or M/D/Y work, so giving up
                        return DateTime.MinValue;
                    }
                }
            }

            if (Y < 100) {
                Y += 2000;
            }
            if (new DateTime(Y, m, d) > LimitDate) {
                Y--;
            }
            return new DateTime(Y, m, d);
        }

        /// <summary>
        /// Tries to ensure that all of the parent folders for given file (or folder) name exist
        /// Returns true if the folder (now) exists; otherwise, false if there was a problem trying to create it.
        /// </summary>
        /// <returns></returns>
        public static bool EnsureBaseFolderExists(string QualifiedFileName) {
            string ParentFolderName = Path.GetDirectoryName(QualifiedFileName);

            if (ParentFolderName.Length == 0) {
                return true;
            }

            if (Directory.Exists(ParentFolderName)) {
                return true;
            }

            if (!EnsureBaseFolderExists(ParentFolderName)) {
                return false;
            }
            try {
                Directory.CreateDirectory(ParentFolderName);

            } catch (Exception) {

                return false;
            }
            return true;
        }


}

}

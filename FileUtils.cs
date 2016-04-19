using System;
using System.IO;
using System.Windows.Forms;

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
        [System.Obsolete("Call Path.GetFileNameWithoutExtension directly")]
        public static string FileNameSansExt(string FileName) {
            return Path.GetFileNameWithoutExtension(FileName);
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

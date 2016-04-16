using System.IO;
using System.Collections.Generic;
using static GruntWurk.QuickLog;

namespace GruntWurk {
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile {
        static char[] EQUALSIGN = { '=' };
        static char[] NEWLINE = { '\n' };

        public string QualifiedFileName;
        public List<IniFileEntry> AllFileEntriesInOrder;
        // The following serves as an index.
        public Dictionary<string, Dictionary<string, IniFileEntry>> Sections;
        string CurrentSection = "";
        IniFileEntry currentEntry;

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        public IniFile() {
            Clear();
        }

        public void Clear() {
            AllFileEntriesInOrder = new List<IniFileEntry>();
            Sections = new Dictionary<string, Dictionary<string, IniFileEntry>>();
            currentEntry = new IniFileEntry();
            CurrentSection = "";
        }

        /// <summary>
        /// Read the contents of the INI file into the entry list and cross-index the entries in
        /// Sections field (a map of maps).
        /// </summary>
        /// <PARAM name="QualifiedFileName">Name and location of the INI file.</PARAM>
        public void LoadFile(string QualifiedFileName) {
            Clear();
            this.QualifiedFileName = QualifiedFileName;

            using (StreamReader sr = new StreamReader(QualifiedFileName)) {
                while (sr.Peek() >= 0) {
                    ParseLine(sr.ReadLine().Trim());
                }
            }
            flushCurrentEntry();
        }
        /// <summary>
        /// Alternative method for loading the INI information.
        /// </summary>
        public void LoadText(string IniText) {
            string[] IniLines = IniText.Split(NEWLINE);
            CurrentSection = "";
            foreach (string line in IniLines) {
                ParseLine(line.Trim());
            }
            flushCurrentEntry();
        }

        private void ParseLine(string lineText) {
            if (lineText == "" || StringUtils.Left(lineText, 1) == ";") {
                // Comment Line
                currentEntry.leadingComment += lineText + "\n";
            } else if (lineText.Length >= 2 && lineText.Substring(0, 1) == "[" && lineText.Substring(lineText.Length - 1, 1) == "]") {
                flushCurrentEntry();
                // Section Header
                CurrentSection = lineText.Substring(1, lineText.Length - 2).ToUpper();
                currentEntry.section = CurrentSection;
            } else {
                string[] definitionParts = lineText.Split(EQUALSIGN, 2);
                currentEntry.key = definitionParts[0].Trim();
                currentEntry.value = (definitionParts.Length > 1) ? definitionParts[1].Trim() : "";
                flushCurrentEntry();
            }
        }

        private void flushCurrentEntry() {
            if (!currentEntry.IsEmpty()) {
                // Even if the entry only contains comments, always add it to the AllFileEntriesInOrder list
                AllFileEntriesInOrder.Add(currentEntry);
                string sectionName = currentEntry.section.ToUpper();
                string entryKey = currentEntry.key.ToUpper();

                // Only index the entry if it has an entry key
                if (!(entryKey == "")) {
                    if (!Sections.ContainsKey(sectionName)) {
                        Sections.Add(CurrentSection, new Dictionary<string, IniFileEntry>());
                    }
                    Dictionary<string, IniFileEntry> sectionMap = Sections[sectionName];
                    sectionMap[entryKey] = currentEntry;
                }
                currentEntry = new IniFileEntry();
                currentEntry.section = CurrentSection;
            }
        }

        public string FindMaxSectionName(string Prefix) {
            Prefix = Prefix.ToUpper();
            string HighestSection = "";
            foreach (string section in Sections.Keys) {
                if (StringUtils.Left(section, Prefix.Length) == Prefix) {
                    if (section.CompareTo(HighestSection) > 0) {
                        HighestSection = section;
                    }
                }
            }
            return HighestSection;
        }
        public string GetString(string section, string key, string defaultValue) {
            section = section.ToUpper();
            key = key.ToUpper();
            if (Sections[section].ContainsKey(key)) {
                return Sections[section][key].value;
            }
            return defaultValue;
        }
        public int GetInt(string section, string key, int defaultValue) {
            section = section.ToUpper();
            key = key.ToUpper();
            if (Sections[section].ContainsKey(key)) {
                // TODO error handling
                return int.Parse(Sections[section][key].value);
            }
            return defaultValue;
        }
        public bool GetBool(string section, string key, bool defaultValue) {
            section = section.ToUpper();
            key = key.ToUpper();
            if (Sections[section].ContainsKey(key)) {
                string s = StringUtils.Left(Sections[section][key].value.ToUpper(), 1);
                return (s == "Y" || s == "T" || s == "1");
            }
            return defaultValue;
        }

        // TODO If we ever need to update INI values and save them back, then implement a Save() method here

        public void Dump() {
            log("===== INI File {0} Contents =====", QualifiedFileName);
            foreach (string section in Sections.Keys) {
                log("Section: {0}", section);
                foreach (string key in Sections[section].Keys) {
                    log("{0}={1}", key, Sections[section][key].value);
                }
            }
            log("");
        }
    }

    /// <summary>
    /// Any entries that appear above the first section will have section of "".
    /// Any comments that appear after the last entry in a section (above the
    /// next section header) will have a separate entry with a key of "".
    /// </summary>
    public class IniFileEntry {
        public string leadingComment = ""; // any comment line(s) that appear above this entry
        public string section = ""; // The section to which this entry belongs
        public string key = ""; // The key (to the left of the =), case sensitive, trimmed
        public string value = ""; // The value (to the right of the =), case sensitive, trimmed
        public bool IsEmpty() {
            return leadingComment == "" && section == "" && key == "" && value == "";
        }
    }
}
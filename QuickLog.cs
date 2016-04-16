using System;
using System.IO;

namespace GruntWurk {
    public class QuickLog {
        public static bool DebugEnabled { get; set; } = false;
        public static bool InfoEnabled { get; set; } = false;
        public static bool UseConsole { get; set; } = true;
        public static string LogFilename { get; set; } = "";
        private static DateTime previousTimestamp = DateTime.MinValue;

        public static void log(string msg) {
            if (UseConsole) {
                Console.WriteLine(msg);
            }
            if (LogFilename != null && LogFilename.Length > 0) {
                using (StreamWriter sw = File.AppendText(LogFilename)) {
                    sw.WriteLine(msg);
                }
            }
        }
        public static void log(string msg, params object[] arg) {
            if (UseConsole) {
                Console.WriteLine(msg, arg);
            }
            if (LogFilename != null && LogFilename.Length > 0) {
                using (StreamWriter sw = File.AppendText(LogFilename)) {
                    sw.WriteLine(msg, arg);
                }
            }
        }
        public static void info(string msg) {
            if (InfoEnabled) {
                log(msg);
            }
        }
        public static void info(string msg, params object[] arg) {
            if (InfoEnabled) {
                log(msg, arg);
            }
        }
        public static void debug(string msg) {
            if (DebugEnabled) {
                log(msg);
            }
        }
        public static void debug(string msg, params object[] arg) {
            if (DebugEnabled) {
                log(msg, arg);
            }
        }
        public static void timestamp(string msg) {
            DateTime stamp = DateTime.Now;
            if (previousTimestamp != DateTime.MinValue) {
                TimeSpan span = stamp - previousTimestamp;
                msg = string.Format("{0} (elapsed {1})", msg, span.ToString());
            }
            if (InfoEnabled) {
                if (UseConsole) {
                    Console.WriteLine("{0}: {1}", stamp, msg);
                }
                if (LogFilename != null && LogFilename.Length > 0) {
                    using (StreamWriter sw = File.AppendText(LogFilename)) {
                        sw.WriteLine("{0}: {1}", stamp, msg);
                    }
                }
            }
            previousTimestamp = stamp;
        }
    }
}

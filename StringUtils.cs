using System;
using System.Text.RegularExpressions;

namespace GruntWurk
{
    /// <summary>
    /// Misc. methods for manipulating strings.
    /// </summary>
    public class StringUtils {

    	// This is how delimiters need to be specified when using the Split() function.
        public static readonly char[] JUST_NEWLINE = { '\n' };
        public static readonly char[] JUST_COMMA = { ',' };
        public static readonly char[] JUST_PERIOD = { '.' };
        public static readonly char[] JUST_SPACE = { ' ' };

        /// <summary>
        /// This mimics the VisualBasic Mid function in 2 ways:
        /// (1) it is forgiving of the original string being shorter than the Start + Width, returning a short or empty string, rather than throwing an exception,
        /// (2) Start is one-based.
        /// </summary>
        public static string Mid(string S, int Start, int Width) {
            int L = S.Length;
            if (L < Start) {
                return "";
            }
            if (L < Start + Width - 1) {
                return S.Substring(Start - 1, L - Start + 1);
            }
            return S.Substring(Start - 1, Width);
        }

        /// <summary>
        /// This mimics the VisualBasic Left function in that it is forgiving of the original string being shorter than the requested Width, returning a short or empty string, rather than throwing an exception.
        /// </summary>
        /// <param name="S"></param>
        /// <param name="Width"></param>
        /// <returns></returns>
        public static string Left(string S, int Width) {
            return Mid(S, 1, Width);
        }

        /// <summary>
        /// This mimics the VisualBasic Right function in that it is forgiving of the original string being shorter than the requested Width, returning a short or empty string, rather than throwing an exception.
        /// </summary>
        /// <param name="S"></param>
        /// <param name="Width"></param>
        /// <returns></returns>
        public static string Right(string S, int Width) {
            int StartPos = S.Length - Width + 1;
            return Mid(S, (StartPos < 1) ? 1 : StartPos, Width);
        }


        /// <summary>
        /// A generic function to do a simple RegEx search/replace.
        /// </summary>
        /// <param name="WhichString"></param>
        /// <param name="Pattern"></param>
        /// <param name="ReplaceWith"></param>
        /// <param name="IsCaseSensitive"></param>
        /// <returns></returns>
        public static string RegExpReplace(string WhichString, string Pattern, string ReplaceWith, bool IsCaseSensitive = true) {
            return new Regex(Pattern, (IsCaseSensitive) ? 0 : RegexOptions.IgnoreCase).Replace(WhichString, ReplaceWith);
        }


        /// <summary>
        /// A generic function to do a simple RegEx test.
        /// </summary>
        /// <param name="WhichString"></param>
        /// <param name="Pattern"></param>
        /// <param name="IsCaseSensitive"></param>
        /// <returns>True if the string matches the pattern; otherwsie, false.</returns>
        public static bool RegExpMatch(string WhichString, string Pattern, bool IsCaseSensitive = true) {
            return new Regex(Pattern, (IsCaseSensitive) ? 0 : RegexOptions.IgnoreCase).IsMatch(WhichString);
        }


        /// <summary>
        /// A generic function to do a simple RegEx find-all.
        /// </summary>
        /// <param name="WhichString"></param>
        /// <param name="Pattern"></param>
        /// <param name="IsCaseSensitive"></param>
        /// <returns>A MatchCollection with any/all matches.</returns>
        public static MatchCollection RegExpFindAll(string WhichString, string Pattern, bool IsCaseSensitive = true) {
            return new Regex(Pattern, (IsCaseSensitive) ? 0 : RegexOptions.IgnoreCase).Matches(WhichString);
        }

        // LTrimZeros() returns the given string less any leading zeros
        public static string LTrimZeros(string Original ) {
            for (int p = 0; p < Original.Length; p++) {
                if (Original[p] != '0') {
                    return Mid(Original, p+1, 9999);
                }
            }
            return "0";
        }




    }
}


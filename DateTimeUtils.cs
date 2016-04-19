using System;
using System.IO;
using System.Text.RegularExpressions;
using static GruntWurk.NumberUtils;

namespace GruntWurk {
    public class DateTimeUtils {
        /// <summary>
        /// Converts a 2-digit year (12, or 98) to a 4 digit year (2012, or 1998), making sure it is not beyond the current year.
        /// </summary>
        /// <param name="TwoDigitYear">If already larger than 2 digits, then returned as-is.</param>
        /// <returns></returns>
        public static int FourDigitYear(int TwoDigitYear) {
            if (TwoDigitYear > 99) {
                return TwoDigitYear;
            }
            int result = 2000 + TwoDigitYear;
            if (result > DateTime.Now.Year) {
                result -= 100;
            }
            return result;
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
            DateTime LimitDate = DateTime.Now;

            OrigFileName = FileName;
            FileName = Path.GetFileNameWithoutExtension(FileName);

            // Fiscal Year is the only token with letters that we care about, so look for that first
            Matches = StringUtils.RegExpFindAll(FileName, "FY[0-9]+", false);
            if (Matches.Count >= 1) {
                FiscalYear = DateTimeUtils.FourDigitYear(IntOrDefault(Matches[0].ToString().Substring(2), 0));
                // Strip out the token we just found so that we don't process it again
                FileName = StringUtils.RegExpReplace(FileName, "FY([0-9]+)", " ", false);
            }

            // Strip out all non-digits, replacing them with spaces, and then tokenize on the spaces
            FileName = StringUtils.RegExpReplace(FileName, "[^0-9]+", " ").ToUpper();
            Tokens = FileName.Split(StringUtils.JUST_SPACE);

            Digits = new string[(Tokens.Length > 3) ? Tokens.Length : 3];

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

    }
}

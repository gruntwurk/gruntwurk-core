using System;

namespace GruntWurk
{
    public class NumberUtils
    {
        /// <summary>
        /// Same as int.Parse(), but with a default value, rather than throwing an exception.
        /// </summary>
        /// <param name="SourceValue">The string to parse.</param>
        /// <param name="DefaultValue">The value to use if the string cannot be parsed.</param>
        /// <returns></returns>
        public static int IntOrDefault(string SourceValue, int DefaultValue)
        {
            if (SourceValue.Length == 0)
            {
                return DefaultValue;
            }
            try
            {
                return int.Parse(SourceValue);
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Tests whether the given operand is between the given start and end (inclusive). 
        /// The hitch is that the specified end of range might be zero or negative, 
        /// which means it needs to be compared against the count (i.e. -3 means "N minus 3", 
        /// and 0 means N).
        /// </summary>
        /// <param name="operand">The value to be compared againste the range.</param>
        /// <param name="specStart">The start of the range.</param>
        /// <param name="specEnd">The end of the range. If zero or negative, then relative to the actual count.</param>
        /// <param name="actualCount">The factor to use when processing and end-of-range that is relative to the end (of the actual count of data in question).</param>
        /// <returns></returns>
        public static bool InRange(int operand, int specStart, int specEnd, int actualCount)
        {
            int adjustedEnd = (specEnd < 1) ? actualCount + specEnd : specEnd;
            return operand >= specStart && operand <= adjustedEnd;
        }

    }
}

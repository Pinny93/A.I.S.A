using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A.Utils
{
    public static class StringExtensions
    {
        public static string Truncate(this string str, int maxLength, bool addTripleDot = true)
        {
            int maxLen = str.Length < maxLength ? str.Length : maxLength;

            string newStr = str.Substring(0, maxLen);

            if (addTripleDot)
            {
                newStr += "...";
            }

            return newStr;
        }

        public static string EscapeNewLine(this string str)
        {
            string newString = str.Replace("\r", "`r").Replace("\n", "`n");

            return newString;
        }

        public static string ToDelimiterSeparatedString(this string[] stringArray, string delimiter = ", ")
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < stringArray.Length; i++)
            {
                result.Append(stringArray[i]);
                if (!String.IsNullOrEmpty(delimiter) &&
                    i < stringArray.Length - 1)
                {
                    result.Append(delimiter);
                }
            }

            return result.ToString();
        }
    }
}
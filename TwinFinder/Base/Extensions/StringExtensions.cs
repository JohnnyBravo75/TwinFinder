using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TwinFinder.Base.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Null safe ToString()
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringOrEmpty(this object value)
        {
            return (value ?? String.Empty).ToString();
        }

        /// <summary>
        /// Appends the specified a string to another strin, with the given separator (default is Environment.NewLine)
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="append">The append.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string Append(this string str, string append, string separator = "\r\n")
        {
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(append) && !string.IsNullOrEmpty(separator))
            {
                str += separator;
            }

            str += append;
            return str;
        }

        /// <summary>
        /// convert a byte array to string using default encoding
        /// </summary>
        /// <param name="data">the content of the array</param>
        /// <returns>converted string</returns>
        public static string BytesToString(this byte[] data)
        {
            return System.Text.Encoding.GetEncoding(0).GetString(data);
        }

        /// <summary>
        /// Chars at.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static string CharAt(this string str, int index)
        {
            if (index >= str.Length)
            {
                return "";
            }

            return str[index].ToString();
        }

        /// <summary>
        /// Clears the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void Clear(this string[] array)
        {
            Array.Clear(array, 0, array.Length);
        }

        /// <summary>
        /// Clears the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        public static void Clear(this char[] array)
        {
            Array.Clear(array, 0, array.Length);
        }

        /// <summary>
        /// Checks if a string is contained and gives the found string back
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="toCheck">To check.</param>
        /// <param name="matchCase">if set to <c>true</c> [match case].</param>
        /// <param name="wholeString">if set to <c>true</c> [whole string].</param>
        /// <returns></returns>
        public static string ContainedString(this string str, string[] toCheck, bool matchCase = true, bool wholeString = false)
        {
            if (toCheck == null || str == null)
            {
                return "";
            }

            if (toCheck.Length == 0 || str == string.Empty)
            {
                return "";
            }

            foreach (var currStr in toCheck)
            {
                if (wholeString)
                {
                    if (str == currStr)
                    {
                        return currStr;
                    }
                }
                else
                {
                    if (matchCase)
                    {
                        if (!string.IsNullOrEmpty(currStr) && str.IndexOf(currStr, StringComparison.Ordinal) >= 0)
                        {
                            return currStr;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currStr) && str.IndexOf(currStr, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return currStr;
                        }
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Like the original builtin Contains(), but with a stringcomparasion option for caseinsensitve comparing.
        /// Checks, if the given string is in the original string.
        /// Ift can be used with a stringcomparision option, for caseinsensitive checking
        /// e.g.  str.Contains("test", StringComparison.OrdinalIgnoreCase);
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="toCheck">To check.</param>
        /// <param name="comp">The comp.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string str, string toCheck, StringComparison comp)
        {
            if (toCheck == null || str == null)
            {
                return false;
            }

            if (toCheck == string.Empty || str == string.Empty)
            {
                return true;
            }

            return str.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// checks, if one of the given strings is in the original string
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="toCheck">To check.</param>
        /// <param name="matchCase">if set to <c>true</c> [match case].</param>
        /// <param name="wholeString">if set to <c>true</c> [whole string].</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string str, string[] toCheck, bool matchCase = true, bool wholeString = false)
        {
            if (toCheck == null || str == null)
            {
                return false;
            }

            if (toCheck.Length == 0 || str == string.Empty)
            {
                return true;
            }

            foreach (var currStr in toCheck)
            {
                if (wholeString)
                {
                    if (str == currStr)
                    {
                        return true;
                    }
                }
                else
                {
                    if (matchCase)
                    {
                        if (!string.IsNullOrEmpty(currStr) && str.IndexOf(currStr, System.StringComparison.Ordinal) >= 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currStr) && str.IndexOf(currStr, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified string contains any of the given string in the array.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="toCheck">To array of strings to check.</param>
        /// <param name="pos">The found position.</param>
        /// <param name="length">The found length.</param>
        /// <param name="matchCase">if set to <c>true</c> [match case].</param>
        /// <returns>
        ///   <c>true</c> if the specified string contains any of the given string in the array; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string str, string[] toCheck, out int pos, out int length, bool matchCase = true)
        {
            length = 0;
            pos = -1;
            bool result = false;
            int idx = 0;
            while (idx < toCheck.Length)
            {
                int foundPos = -1;
                if (matchCase)
                {
                    foundPos = str.IndexOf(toCheck[idx], StringComparison.Ordinal);
                }
                else
                {
                    foundPos = str.IndexOf(toCheck[idx], StringComparison.OrdinalIgnoreCase);
                }

                if (foundPos != -1)
                {
                    length = toCheck[idx].Length;
                    pos = foundPos;
                    result = true;
                }
                break;
            }

            idx = idx + 1;

            return result;
        }

        /// <summary>
        /// Counts the specified STRS.
        /// </summary>
        /// <param name="strs">The STRS.</param>
        /// <returns></returns>
        public static int Count(this string[] strs)
        {
            return strs.Length;
        }

        /// <summary>
        ///  Gets only the  digits (0-9) from the string e.g. "qq2 895 - 5tb" -> "28955"
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string Digits(this string str)
        {
            return new string(str.Where(x => char.IsDigit(x)).ToArray());
        }

        /// <summary>
        /// Determines whether [is char at] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="strings">The strings.</param>
        /// <returns>
        ///   <c>true</c> if [is char at] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCharAt(this string str, int startIndex, params string[] strings)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            foreach (string str1 in strings)
            {
                if (str.IndexOf(str1, startIndex, StringComparison.OrdinalIgnoreCase) >= startIndex)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the string is a digit
        /// </summary>
        /// <param name="str">input string</param>
        /// <returns>true, if the string is a digit</returns>
        public static bool IsDigit(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            char[] chars = str.ToCharArray();
            foreach (char c in chars)
            {
                if (!c.IsDigit())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the string is a digit
        /// </summary>
        /// <param name="chr">The CHR.</param>
        /// <returns>
        /// true, if the string is a digit
        /// </returns>
        public static bool IsDigit(this char chr)
        {
            if (chr == '0' || chr == '1' || chr == '2' || chr == '3' || chr == '4'
            || chr == '5' || chr == '6' || chr == '7' || chr == '8' || chr == '9')
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// true, if the string can be parse as Double respective Int32
        /// Spaces are not considred.
        /// </summary>
        /// <param name="str">input string</param>
        /// <param name="floatpoint">true, if Double is considered,
        /// otherwhise Int32 is considered.</param>
        /// <returns>true, if the string contains only digits or float-point</returns>
        public static bool IsNumber(this string str, bool floatpoint = true)
        {
            int i;
            double d;

            string withoutWhiteSpace = str.Replace(" ", "");

            try
            {
                if (floatpoint)
                {
                    return double.TryParse(withoutWhiteSpace, NumberStyles.Any, Thread.CurrentThread.CurrentUICulture, out d);
                }
                else
                {
                    return int.TryParse(withoutWhiteSpace, out i);
                }
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Joins the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> array, string seperator)
        {
            if (array == null)
            {
                return string.Empty;
            }

            return string.Join(seperator, array.ToArray());
        }

        //private string StringBytesToString(this byte[] dataToEncode)
        //{
        //    string encodedString = "";
        //    foreach (byte bite in dataToEncode)
        //    {
        //        encodedString = encodedString + (char)bite;
        //    }
        //    return encodedString;
        //}
        /// <summary>
        /// Joins the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string Join(this string[] array, string seperator)
        {
            if (array == null)
            {
                return string.Empty;
            }

            return string.Join(seperator, array);
        }

        /// <summary>
        /// Returns the first few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the
        /// given length the complete string is returned. If length is zero or
        /// less an empty string is returned
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            length = Math.Max(length, 0);

            return (str.Length <= length
                       ? str
                       : str.Substring(0, length)
                   );
        }

        /// <summary>
        /// Lineses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string[] Lines(this string input, StringSplitOptions options = StringSplitOptions.None)
        {
            return input.Split(new String[] { "\r\n", "\n" }, options);
        }

        /// <summary>
        /// checks if an range matches
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="offset1">The offset1.</param>
        /// <param name="str2">The STR2.</param>
        /// <param name="offset2">The offset2.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static bool RangeMatches(this string str, int offset1, string str2, int offset2, int length)
        {
            if (length != str2.Length || str.Length < offset1 + length)
            {
                return false;
            }

            for (int i = 0; i < length; ++i)
            {
                if (str[offset1 + i] != str2[offset2 + i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes the chars.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="removeChars">The remove chars.</param>
        /// <returns></returns>
        public static string RemoveChars(this string str, params char[] removeChars)
        {
            var sb = new StringBuilder(str.Length);
            foreach (char c in str)
            {
                if (!removeChars.Contains(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes double char from a string e.g. Hello -&gt; Helo, aassdddd -&gt; asd
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string RemoveDuplicateChars(this string str)
        {
            return Regex.Replace(str, @"(.)(\1)+", "$1");
        }

        /// <summary>
        /// Reverses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        //    return sb.ToString();
        //}
        /// <summary>
        /// Returns the last few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the
        /// given length the complete string is returned. If length is zero or
        /// less an empty string is returned
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string str, int length)
        {
            length = Math.Max(length, 0);

            if (str.Length > length)
            {
                return str.Substring(str.Length - length, length);
            }
            else
            {
                return str;
            }
        }

        //    // step 2, convert byte array to hex string
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < hash.Length; i++)
        //    {
        //        sb.Append(hash[i].ToString("X2"));
        //    }
        /// <summary>
        /// Returns an enumerable collection of the specified type containing the substrings in this instance that are delimited by elements of a specified Char array
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separator">
        /// An array of Unicode characters that delimit the substrings in this instance, an empty array containing no delimiters, or null.
        /// </param>
        /// <typeparam name="T">
        /// The type of the elemnt to return in the collection, this type must implement IConvertible.
        /// </typeparam>
        /// <returns>
        /// An enumerable collection whose elements contain the substrings in this instance that are delimited by one or more characters in separator.
        /// </returns>
        public static IEnumerable<T> SplitTo<T>(this string str, params char[] separator) where T : IConvertible
        {
            foreach (var s in str.Split(separator, StringSplitOptions.None))
            {
                yield return (T)Convert.ChangeType(s, typeof(T));
            }
        }

        /// <summary>
        /// Checks whether the string starts with the given strings
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="comparison">The comparison.</param>
        /// <param name="strings">The strings.</param>
        /// <returns></returns>
        public static bool StartsWith(this string str, StringComparison comparison, params string[] strings)
        {
            foreach (string str1 in strings)
            {
                if (str.StartsWith(str1, comparison))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// get the byte array from a string using default encoding
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>converted array</returns>
        public static byte[] StringToBytes(this string str)
        {
            return Encoding.GetEncoding(0).GetBytes(str);
        }

        /// <summary>
        /// Takes the specified the string.
        /// </summary>
        /// <param name="theString">The string.</param>
        /// <param name="count">The count.</param>
        /// <param name="ellipsis">if set to <c>true</c> [ellipsis].</param>
        /// <returns></returns>
        public static string Take(this string theString, int count, bool ellipsis)
        {
            return Take(theString, 0, count, ellipsis);
        }

        /// <summary>
        /// Like linq take - takes the first x characters
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="startindex">The startindex.</param>
        /// <param name="count">The count.</param>
        /// <param name="ellipsis">if set to <c>true</c> [ellipsis].</param>
        /// <returns></returns>
        public static string Take(this string str, int startindex, int count, bool ellipsis)
        {
            if (str.Length < startindex)
            {
                return "";
            }

            int lengthToTake = Math.Min(count, str.Length);
            var cutDownString = str.Substring(startindex, Math.Min(lengthToTake, str.Length - startindex));

            if (ellipsis && lengthToTake < str.Length)
            {
                cutDownString += "...";
            }

            return cutDownString;
        }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string str)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static decimal ToDecimal(this string str, string error)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
            {
                return result;
            }

            throw new FormatException(error);
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static double? ToDouble(this string str)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static double ToDouble(this string str, string error)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }

            throw new FormatException(error);
        }

        /// <summary>
        /// To the float.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static float? ToFloat(this string str)
        {
            float result;
            if (float.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To the float.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static float? ToFloat(this string str, string error)
        {
            float result;
            if (float.TryParse(str, out result))
            {
                return result;
            }

            throw new FormatException(error);
        }

        /// <summary>
        /// To the int.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static int? ToInt(this string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To the int.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static int ToInt(this string str, string error)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return result;
            }

            throw new FormatException(error);
        }

        /// <summary>
        /// To the long.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static long? ToLong(this string str)
        {
            long result;
            if (long.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To the long.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static long ToLong(this string str, string error)
        {
            long result;
            if (long.TryParse(str, out result))
            {
                return result;
            }

            throw new FormatException(error);
        }

        /// <summary>
        /// To the short.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static short? ToShort(this string str)
        {
            short result;
            if (short.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To the short.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static short ToShort(this string str, string error)
        {
            short result;
            if (short.TryParse(str, out result))
            {
                return result;
            }

            throw new FormatException(error);
        }

        /// <summary>
        /// Converts a string array into a single string
        /// </summary>
        /// <param name="strs">the string array</param>
        /// <param name="separator">The separator.</param>
        /// <returns>
        /// the string
        /// </returns>
        public static string ToSingleString(this string[] strs, string separator = "")
        {
            string result = string.Empty;
            int index = 0;
            foreach (string str in strs)
            {
                result += str;

                if (!string.IsNullOrEmpty(separator) && index < strs.Length - 1)
                {
                    result += separator;
                }

                index++;
            }

            return result;
        }

        //public static string ToMD5(this string str)
        //{
        //    // step 1, calculate MD5 hash from input
        //    MD5 md5 = System.Security.Cryptography.MD5.Create();
        //    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
        //    byte[] hash = md5.ComputeHash(inputBytes);
        /// <summary>
        /// Converts a string into a string array
        /// </summary>
        /// <param name="str">the string</param>
        /// <returns>the string array</returns>
        public static string[] ToStringArray(this string str)
        {
            char[] chars = str.ToCharArray();
            string[] strings = new string[chars.Length];
            int i = 0;
            foreach (char chr in chars)
            {
                strings[i] = chr.ToString();
                i++;
            }

            return strings;
        }

        //private byte[] StringToBytes(this string dataToEncode)
        //{
        //    List<byte> bytes = new List<byte>();
        //    foreach (char character in dataToEncode.ToCharArray())
        //    {
        //        bytes.Add((byte)character);
        //    }
        //    return bytes.ToArray();
        //}
        /// <summary>
        /// To the title case.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string ToTitleCase(this string str)
        {
            if (str == null)
            {
                return str;
            }

            // Does not work!!
            // return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str);

            string[] words = str.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0)
                {
                    continue;
                }

                char firstChar = char.ToUpper(words[i][0]);
                string rest = "";

                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }

                words[i] = firstChar + rest;
            }

            return string.Join(" ", words);
        }

        /// <summary>
        /// Like SubString, but does not throw an error, if the string is shorter than the given length
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="length">number of chars to take</param>
        /// <returns>
        /// the cutted string
        /// </returns>
        public static string TrySubstring(this string str, int length)
        {
            return Take(str, length, false);
        }

        /// <summary>
        /// Tries the sub string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="startindex">The startindex.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string TrySubString(this string str, int startindex, int length)
        {
            return Take(str, startindex, length, false);
        }

        /// <summary>
        /// Tries the trim.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string TryTrim(this string str)
        {
            if (str == null)
            {
                return str;
            }

            return str.Trim();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TwinFinder.Base.Extensions;

namespace TwinFinder.Base.Utils
{
    /// <summary>
    /// Hilfsklasse für spezielle Stringfunktionen (Wörter, Namenstrennung,...)
    /// </summary>
    public static class StringUtil
    {
        private static readonly char[] ws = { ' ', '\n', '\t', '\r' };

        public static string Repeat(char chr, int count)
        {
            return string.Concat(Enumerable.Repeat(chr, count));
        }

        public static string Repeat(string str, int count)
        {
            return string.Concat(Enumerable.Repeat(str, count));
        }

        /// <summary>
        /// Counts the character.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="toCount">To count.</param>
        /// <returns></returns>
        public static int CountCharacter(string str, char toCount)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char chr = str[i];
                if (chr == toCount)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Finds the postion of the next non numeric char.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="occurance">The occurance.</param>
        /// <returns>
        /// the postion
        /// </returns>
        public static int FindNonNumeric(string str, int occurance = 1)
        {
            int pos = -1;
            int idx = 0;

            if (string.IsNullOrEmpty(str))
            {
                return pos;
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (!str[i].IsDigit())
                {
                    pos = i;
                    idx++;

                    // is the right postion found
                    if (idx == occurance)
                    {
                        break;
                    }
                }
            }

            // when not found so often as needed, set to error
            if (idx < occurance)
            {
                pos = -1;
            }

            return pos;
        }

        /// <summary>
        /// _Returns the index of the first word found
        /// </summary>
        /// <param name="str">the string</param>
        /// <param name="word">the wortd to find</param>
        /// <returns></returns>
        public static int IndexOfWord(string str, string word)
        {
            Match match = Regex.Match(str, @"\b" + word + @"\b", RegexOptions.IgnoreCase);
            return match.Index;
        }

        /// <summary>
        /// Determines whether the str is alpanumeric.
        /// </summary>
        /// <param name="str">>the input string.</param>
        /// <returns>
        ///   <c>true</c> if [is alpha numeric] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            str = str.ToLower();

            Regex alphaNumRegex = new Regex("^[a-z0-9]+$", RegexOptions.IgnoreCase);
            return alphaNumRegex.IsMatch(str);
        }

        /// <summary>
        /// Determines whether [is valid regex] [the specified pattern].
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>
        ///   <c>true</c> if [is valid regex] [the specified pattern]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes the accents. e.g. Société Générale -> Societe Generale
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>the cleaned string</returns>
        public static string RemoveAccents(string str)
        {
            // normalize the string to its full compatibilitity decomposition (formKD)
            str = str.Normalize(NormalizationForm.FormKD);

            Encoding encoding = Encoding.GetEncoding(Encoding.ASCII.CodePage, new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
            byte[] bytes = encoding.GetBytes(str);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Removes the diacritics and accents e.g. Société Générale -> Societe Generale
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>the cleaned string</returns>
        public static string RemoveDiacritics(string str)
        {
            // normalize the string to its full cannonical decomposition (formD)
            string formD = str.Normalize(NormalizationForm.FormD);
            var result = new StringBuilder();

            foreach (char chr in formD)
            {
                // checks what type of unicode it is
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(chr);

                // we leave out the nonSpacing Mark codes (diacritics, accents)
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    result.Append(chr);
                }
            }

            // convert back to the formC
            return (result.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Removes non AlphaNumericCharacters from the string
        /// </summary>
        /// <param name="str">the input string</param>
        /// <returns></returns>
        public static string RemoveNonAlphaNumericCharacters(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            str = str.ToLower();

            Regex nonAlphaNumRegex = new Regex("^[^a-z0-9]$", RegexOptions.IgnoreCase);
            return nonAlphaNumRegex.Replace(str, "");
        }

        /// <summary>
        /// Replaces a string value but only a whole word
        /// (\b means a word boundary)
        /// </summary>
        /// <param name="str">the string</param>
        /// <param name="oldWord">the old word (to search)</param>
        /// <param name="newWord">the new word (replacement)</param>
        /// <returns></returns>
        public static string ReplaceWord(string str, string oldWord, string newWord)
        {
            return Regex.Replace(str, @"\b" + oldWord + @"\b", newWord, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Separates a string into two pieces by a separator
        /// </summary>
        /// <param name="str"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string Separate(ref string str, string seperator)
        {
            string seperatedString = "";
            int indexOf = str.IndexOf(seperator, StringComparison.Ordinal);
            if (indexOf != -1)
            {
                seperatedString = str.Left(indexOf + 1);
                str = str.Right(str.Length - indexOf - 1);
            }
            else
            {
                seperatedString = str;
                str = "";
            }

            return seperatedString;
        }

        /// <summary>
        /// Splits a string into n pieces (but always with complete words)
        /// e.g. ""
        /// </summary>
        /// <param name="str">the string</param>
        /// <param name="pieceCount">the number of pieces</param>
        /// <param name="maxPieceLength">maximum legth of a piece</param>
        /// <returns></returns>
        public static string[] SplitWordPieces(string str, int pieceCount, int maxPieceLength)
        {
            string left = "";
            int i = 0;
            string[] pieces = new string[pieceCount];

            if (string.IsNullOrEmpty(str))
            {
                return pieces;
            }

            // in n Teile zu je XX Zeichen aufteilen
            while (str.Length > 0 && i < pieceCount)
            {
                left = Separate(ref str, " ");

                // Wenn Stück zu lang, oder gar kein Space gefunden wird
                if (left.TrimStart(' ').TrimEnd(' ').Length > maxPieceLength || string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(str))
                {
                    // Wieder zusammenfügen und dann nach Bindestrich als Trenner suchen
                    str = left + str;
                    left = Separate(ref str, "-");
                }

                // solange < pnPieceLength Zeichen, dranhängen
                if ((pieces[i] + left).TrimStart(' ').TrimEnd(' ').Length <= maxPieceLength)
                {
                    pieces[i] += left;
                }
                else
                {
                    if (pieces[i] == null && string.IsNullOrEmpty(str))
                    {
                        pieces[i] = left;
                    }

                    // trim whitespaces
                    pieces[i] = pieces[i].TrimStart(' ').TrimEnd(' ');

                    // sonst passt das Wort nicht mehr, ab in die nächste Zeile (Word Warp)
                    i++;

                    if (i < pieceCount)
                    {
                        if ((pieces[i] + left).Length <= maxPieceLength)
                        {
                            pieces[i] += left;
                        }
                    }
                }
            }

            return pieces;
        }

        /// <summary>
        /// Converts a string to a alpha numeric string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string ToAlphaNumeric(string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string str1 = str;
            for (int i = 0; i < str1.Length; i++)
            {
                char chr = str1[i];
                if (char.IsLetterOrDigit(chr))
                {
                    stringBuilder.Append(chr);
                }
            }

            return stringBuilder.ToString();
        }

        public static string Translate(string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string[] WordWrap(string str, int width, string[] seperators)
        {
            if (str == null)
            {
                throw new NullReferenceException();
            }

            if (seperators == null)
            {
                throw new ArgumentNullException("seperators", "The seperators array can not be null");
            }

            if (seperators.Length == 0)
            {
                throw new ArgumentException("The seperators array can not be empty", "seperators");
            }

            if (width <= 0)
            {
                throw new ArgumentException("The width must be greater than zero.", "width");
            }

            StringBuilder lineBuilder = new StringBuilder(width);
            List<string> wrappedLines = new List<string>();

            string[] words = str.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                bool wordWritten = false;

                while (!wordWritten)
                {
                    if (lineBuilder.Length > 0)
                    {
                        if (lineBuilder.Length + word.Length + 1 <= width)
                        {
                            lineBuilder.Append(" " + word);
                            wordWritten = true;
                        }
                        else
                        {
                            wrappedLines.Add(lineBuilder.ToString());
                            lineBuilder.Length = 0;
                        }
                    }
                    else
                    {
                        if (word.Length <= width)
                        {
                            lineBuilder.Append(word);
                            wordWritten = true;
                        }
                        else
                        {
                            string tooLongWordRemains = word;
                            while (tooLongWordRemains.Length > width)
                            {
                                lineBuilder.Append(tooLongWordRemains.Substring(0, tooLongWordRemains.Length < width ? tooLongWordRemains.Length : width));
                                wrappedLines.Add(lineBuilder.ToString());
                                lineBuilder.Length = 0;

                                int startIdx = tooLongWordRemains.Length < width ? tooLongWordRemains.Length : width;
                                tooLongWordRemains = tooLongWordRemains.Substring(startIdx, tooLongWordRemains.Length - startIdx);
                            }
                            lineBuilder.Append(tooLongWordRemains);
                            wordWritten = true;
                        }
                    }
                }
            }

            if (lineBuilder.Length > 0)
                wrappedLines.Add(lineBuilder.ToString());

            return wrappedLines.ToArray();
        }

        public static string[] WordWrap(string str, int width)
        {
            return WordWrap(str, width, new string[] { " ", "\t", "\r", "\n" });
        }

        /// <summary>
        /// Wraps the string at the given column index.
        /// </summary>
        /// <param name="input">The string to process.</param>
        /// <param name="column">The column at which to wrap the string.</param>
        /// <returns>A stream of strings representing the wrapped lines. String.Length is &lt;= column.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if argument is null.</exception>
        /// <remarks>
        /// This is an extension method that inserts newlines at the whitespace boundary nearest but
        /// less than the provided column parameter. In other words, contrary to Strings.HardWrapAt
        /// which inserts the newline at the specified index, this method searches back for the
        /// nearest whitespace boundary less than the column boundary:
        /// <code>
        /// string text = "Lorem ipsum dolor sit amet, consectetur";
        /// foreach (var line in text.WordWrapAt(8))
        /// {
        ///     Console.WriteLine(line);
        /// }
        /// // output:
        /// // Lorem
        /// //  ipsum
        /// //  dolor
        /// //  sit
        /// //  amet,
        /// //  consectetur
        /// </code>
        /// </remarks>
        public static IEnumerable<string> WordWrapAt(this string input, int column)
        {
            foreach (var line in input.Lines())
            {
                if (line.Length <= column)
                {
                    yield return line;
                }
                else
                {
                    // the line is longer than the allowed column width, so we start
                    // at the column index, then search backwards for the last whitespace
                    // char; we return that string, then advance the marker and repeat
                    // if there is no whitespace char, then just return the whole string
                    int mark = 0;
                    for (int i = line.LastIndexOfAny(ws, column);
                        i < line.Length && i >= 0 && mark < i;
                        mark = i, i = line.LastIndexOfAny(ws, Math.Min(i + column - 1, line.Length - 1)))
                    {
                        yield return line.Substring(mark, i - mark);
                    }
                    if (mark < line.Length)
                    {
                        yield return line.Substring(mark, line.Length - mark);
                    }
                }
            }
        }
    }
}
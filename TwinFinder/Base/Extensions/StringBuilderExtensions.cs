﻿using System.Text;

namespace TwinFinder.Base.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void Clear(this StringBuilder sb)
        {
            sb.Length = 0;
        }

        /// <summary>
        /// Get index of a char
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, char value)
        {
            return IndexOf(sb, value, 0);
        }

        /// <summary>
        /// Get index of a char starting from a given index
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            for (int i = startIndex; i < sb.Length; i++)
            {
                if (sb[i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get index of a string
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string value)
        {
            return IndexOf(sb, value, 0, false);
        }

        /// <summary>
        /// Get index of a string from a given index
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string value, int startIndex)
        {
            return IndexOf(sb, value, startIndex, false);
        }

        /// <summary>
        /// Get index of a string with case option
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string value, bool ignoreCase)
        {
            return IndexOf(sb, value, 0, ignoreCase);
        }

        /// <summary>
        /// Get index of a string from a given index with case option
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
        {
            int num3;
            int length = value.Length;
            int num2 = (sb.Length - length) + 1;
            if (ignoreCase == false)
            {
                for (int i = startIndex; i < num2; i++)
                {
                    if (sb[i] == value[0])
                    {
                        num3 = 1;
                        while ((num3 < length) && (sb[i + num3] == value[num3]))
                        {
                            num3++;
                        }
                        if (num3 == length)
                        {
                            return i;
                        }
                    }
                }
            }
            else
            {
                for (int j = startIndex; j < num2; j++)
                {
                    if (char.ToLower(sb[j]) == char.ToLower(value[0]))
                    {
                        num3 = 1;
                        while ((num3 < length) && (char.ToLower(sb[j + num3]) == char.ToLower(value[num3])))
                        {
                            num3++;
                        }
                        if (num3 == length)
                        {
                            return j;
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Trim left spaces of string
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static StringBuilder LTrim(this StringBuilder sb)
        {
            if (sb.Length != 0)
            {
                int length = 0;
                int num2 = sb.Length;
                while ((sb[length] == ' ') && (length < num2))
                {
                    length++;
                }
                if (length > 0)
                {
                    sb.Remove(0, length);
                }
            }

            return sb;
        }

        public static StringBuilder RemoveChars(this StringBuilder sb, char ch)
        {
            return sb.Remove(ch, 0);
        }

        public static StringBuilder RemoveChars(this StringBuilder sb, char ch, int startPos)
        {
            if (startPos > sb.Length)
            {
                return sb;
            }

            for (int i = startPos; i < sb.Length; )
            {
                if (sb[i] == ch)
                {
                    sb.Remove(i, 1);
                }
                else
                {
                    i++;
                }
            }

            return sb;
        }

        /// <summary>
        /// Removes duplicates from the string
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static StringBuilder RemoveDuplicates(this StringBuilder code)
        {
            // remove duplicates
            int p = 0;
            char lastChar = ' ';

            while (p < code.Length)
            {
                char c = code[p];

                if (c == lastChar)
                {
                    code.Remove(p, 1);
                }
                else
                {
                    lastChar = c;
                    p++;
                }
            }

            return code;
        }

        public static StringBuilder RemoveFromEnd(this StringBuilder sb, int num)
        {
            return sb.Remove(sb.Length - num, num);
        }

        /// <summary>
        /// Reverses a given text
        /// </summary>
        public static StringBuilder Reverse(this StringBuilder sb)
        {
            char[] charArray = sb.ToString().ToCharArray();
            StringBuilder reverse = new StringBuilder();
            for (int i = charArray.Length - 1; i > -1; i--)
            {
                reverse.Append(charArray[i]);
            }

            return reverse;
        }

        /// <summary>
        /// Trim right spaces of string
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static StringBuilder RTrim(this StringBuilder sb)
        {
            if (sb.Length != 0)
            {
                int length = sb.Length;
                int num2 = length - 1;
                while ((sb[num2] == ' ') && (num2 > -1))
                {
                    num2--;
                }
                if (num2 < (length - 1))
                {
                    sb.Remove(num2 + 1, (length - num2) - 1);
                }
            }

            return sb;
        }

        /// <summary>
        /// Determine whether a string starts with a given text
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StartsWith(this StringBuilder sb, string value)
        {
            return StartsWith(sb, value, 0, false);
        }

        /// <summary>
        /// Determine whether a string starts with a given text (with case option)
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool StartsWith(this StringBuilder sb, string value, bool ignoreCase)
        {
            return StartsWith(sb, value, 0, ignoreCase);
        }

        /// <summary>
        /// Determine whether a string is begin with a given text
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool StartsWith(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
        {
            int length = value.Length;
            int num2 = startIndex + length;
            if (ignoreCase == false)
            {
                for (int i = startIndex; i < num2; i++)
                {
                    if (sb[i] != value[i - startIndex])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int j = startIndex; j < num2; j++)
                {
                    if (char.ToLower(sb[j]) != char.ToLower(value[j - startIndex]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string Substring(this StringBuilder sb, int startIndex, int length)
        {
            return sb.ToString(startIndex, length);
        }

        /// <summary>
        /// Trim spaces around string
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            if (sb.Length != 0)
            {
                int length = 0;
                int num2 = sb.Length;
                while ((sb[length] == ' ') && (length < num2))
                {
                    length++;
                }
                if (length > 0)
                {
                    sb.Remove(0, length);
                    num2 = sb.Length;
                }
                length = num2 - 1;
                while ((sb[length] == ' ') && (length > -1))
                {
                    length--;
                }
                if (length < (num2 - 1))
                {
                    sb.Remove(length + 1, (num2 - length) - 1);
                }
            }

            return sb;
        }
    }
}
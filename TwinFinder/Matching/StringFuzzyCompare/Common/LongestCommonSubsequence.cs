using System;
using System.Collections.Generic;
using System.Text;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    public class LongestCommonSubsequence : StringFuzzyComparer
    {
        public override float Compare(string str1, string str2)
        {
            if (!this.CaseSensitiv)
            {
                str1 = str1.ToLower();
                str2 = str2.ToLower();
            }

            string sequence = "";
            var value = this.BuildLongestCommonSubsequence(str1, str2, out sequence);

            return this.NormalizeScore(str1, str2, value);
        }

        private float NormalizeScore(string str1, string str2, float score)
        {
            // get the max score
            float maxLen = Math.Max(str1.Length, str2.Length);

            if (maxLen == 0)
            {
                return 1;
            }

            // return actual / possible distance to get 0-1 range
            return (score / maxLen);
        }

        public float BuildLongestCommonSubsequence(string str1, string str2, out string sequence)
        {
            sequence = "";
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return 0;
            }

            int[,] num = new int[str1.Length, str2.Length];
            int maxlen = 0;
            int lastSubsBegin = 0;
            var sequenceBuilder = new StringBuilder();

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] != str2[j])
                        num[i, j] = 0;
                    else
                    {
                        if ((i == 0) || (j == 0))
                            num[i, j] = 1;
                        else
                            num[i, j] = 1 + num[i - 1, j - 1];

                        if (num[i, j] > maxlen)
                        {
                            maxlen = num[i, j];
                            int thisSubsBegin = i - num[i, j] + 1;
                            if (lastSubsBegin == thisSubsBegin)
                            {
                                //if the current LCS is the same as the last time this block ran
                                sequenceBuilder.Append(str1[i]);
                            }
                            else
                            {
                                //this block resets the string builder if a different LCS is found
                                lastSubsBegin = thisSubsBegin;
                                sequenceBuilder.Length = 0; //clear it
                                sequenceBuilder.Append(str1.Substring(lastSubsBegin, (i + 1) - lastSubsBegin));
                            }
                        }
                    }
                }
            }
            sequence = sequenceBuilder.ToString();
            return maxlen;
        }

        public string Explain(string str1, string str2)
        {
            if (!this.CaseSensitiv)
            {
                str1 = str1.ToLower();
                str2 = str2.ToLower();
            }

            string sequence = "";
            var value = this.BuildLongestCommonSubsequence(str1, str2, out sequence);
            return sequence;
        }

        //public int LCS1(string str1, string str2)
        //{
        //    int[,] arr = new int[str1.Length + 1, str2.Length + 1];

        //    for (int i = 0; i <= str2.Length; i++)
        //    {
        //        arr[0, i] = 0;
        //    }
        //    for (int i = 0; i <= str1.Length; i++)
        //    {
        //        arr[i, 0] = 0;
        //    }

        //    for (int i = 1; i <= str1.Length; i++)
        //    {
        //        for (int j = 1; j <= str2.Length; j++)
        //        {
        //            if (str1[i - 1] == str2[j - 1])
        //            {
        //                arr[i, j] = arr[i - 1, j - 1] + 1;
        //            }
        //            else
        //            {
        //                arr[i, j] = MathExtension.Max(arr[i - 1, j], arr[i, j - 1]);
        //            }
        //        }
        //    }
        //    return arr[str1.Length, str2.Length];
        //}

        ///// <summary>
        ///// Longest Common Subsequence. A good value is greater than 0.33.
        ///// </summary>
        ///// <param name="str1">The first string.</param>
        ///// <param name="str2">The second string.</param>
        ///// <returns>
        ///// Returns a Tuple of the sub sequence string and the match coeficient.
        ///// </returns>
        //public KeyValuePair<string, float> BuildLongestCommonSubsequence(string str1, string str2)
        //{
        //    if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
        //    {
        //        return new KeyValuePair<string, float>(string.Empty, 0.0f);
        //    }

        //    if (!this.CaseSensitiv)
        //    {
        //        str1 = str1.ToLower();
        //        str2 = str2.ToLower();
        //    }

        //    int[,] matrix = new int[str1.Length + 1, str2.Length + 1];
        //    LcsDirection[,] tracks = new LcsDirection[str1.Length + 1, str2.Length + 1];
        //    int[,] w = new int[str1.Length + 1, str2.Length + 1];
        //    int i, j;

        //    for (i = 0; i <= str1.Length; i++)
        //    {
        //        matrix[i, 0] = 0;
        //        tracks[i, 0] = LcsDirection.North;
        //    }
        //    for (j = 0; j <= str2.Length; j++)
        //    {
        //        matrix[0, j] = 0;
        //        tracks[0, j] = LcsDirection.West;
        //    }

        //    for (i = 1; i <= str1.Length; i++)
        //    {
        //        for (j = 1; j <= str2.Length; j++)
        //        {
        //            if (str1[i - 1].Equals(str2[j - 1]))
        //            {
        //                int k = w[i - 1, j - 1];
        //                matrix[i, j] = matrix[i - 1, j - 1] + MathExtension.Square(k + 1) - MathExtension.Square(k);
        //                tracks[i, j] = LcsDirection.NorthWest;
        //                w[i, j] = k + 1;
        //            }
        //            else
        //            {
        //                matrix[i, j] = matrix[i - 1, j - 1];
        //                tracks[i, j] = LcsDirection.None;
        //            }

        //            if (matrix[i - 1, j] >= matrix[i, j])
        //            {
        //                matrix[i, j] = matrix[i - 1, j];
        //                tracks[i, j] = LcsDirection.North;
        //                w[i, j] = 0;
        //            }

        //            if (matrix[i, j - 1] >= matrix[i, j])
        //            {
        //                matrix[i, j] = matrix[i, j - 1];
        //                tracks[i, j] = LcsDirection.West;
        //                w[i, j] = 0;
        //            }
        //        }
        //    }

        //    i = str1.Length;
        //    j = str2.Length;

        //    string subsequence = "";
        //    float p = matrix[i, j];

        //    //trace the backtracking matrix.
        //    while (i > 0 || j > 0)
        //    {
        //        if (tracks[i, j] == LcsDirection.NorthWest)
        //        {
        //            i--;
        //            j--;
        //            subsequence = str1[i] + subsequence;
        //            //Trace.WriteLine(i + " " + input1[i] + " " + j);
        //        }
        //        else if (tracks[i, j] == LcsDirection.North)
        //        {
        //            i--;
        //        }
        //        else if (tracks[i, j] == LcsDirection.West)
        //        {
        //            j--;
        //        }
        //    }

        //    float coef = p / (str1.Length * str2.Length);

        //    var retval = new KeyValuePair<string, float>(subsequence, coef);
        //    return retval;
        //}

        //internal enum LcsDirection
        //{
        //    None,
        //    North,
        //    West,
        //    NorthWest
        //}
    }
}
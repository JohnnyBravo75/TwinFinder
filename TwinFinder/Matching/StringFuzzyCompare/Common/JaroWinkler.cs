using System;
using System.Text;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    public class JaroWinkler : StringFuzzyComparer
    {
        // ***********************Fields***********************

        /// <summary>
        /// this is the threshold, where the Winkler boost is applied to the normal Jaro score
        /// </summary>
        private float boostThreshold = 0.7f;

        private int prefixLength = 4;

        // ***********************Functions***********************

        public override float Compare(string str1, string str2)
        {
            if (str1 == null || str2 == null)
            {
                return 0;
            }

            if (!this.CaseSensitiv)
            {
                str1 = str1.ToLower();
                str2 = str2.ToLower();
            }

            float score = this.BuildJaroWinkler(str1, str2);

            return this.NormalizeScore(str1, str2, score);
        }

        private float NormalizeScore(string str1, string str2, float score)
        {
            // no normalization needed
            return score;
        }

        public float BuildJaroWinkler(string str1, string str2)
        {
            if (str1 == null || str2 == null)
            {
                return 0;
            }

            // if they equal return 1
            if (str1 == str2)
            {
                return 1.0f;
            }

            int length1 = str1.Length;
            int length2 = str2.Length;

            // the distance used for acceptable transpositions (about the half string length)
            int matchRange = ((Math.Min(str1.Length, str2.Length)) / 2) +
                             ((Math.Min(str1.Length, str2.Length)) % 2);

            // get common characters e.g.
            StringBuilder common1 = this.GetCommonCharacters(str1, str2, matchRange);
            StringBuilder common2 = this.GetCommonCharacters(str2, str1, matchRange);

            // check for zero in common
            if (common1.Length == 0 || common2.Length == 0)
            {
                return 0.0f;
            }

            //if (common1.Length != common2.Length)
            //{
            //    return 0.0f;
            //}

            // get the number of transpositions
            int transpositions = this.GetTranspositions(common1, common2);

            // calculate jaro metric
            float jaroSimilarity = (common1.Length / ((float)str1.Length) +
                                    common2.Length / ((float)str2.Length) +
                                    (common1.Length - transpositions) / ((float)common1.Length)) / 3.0f;

            // calculate jaroWinkler metric, by adding the Winkler boost factor to the jaro score, if similarity is over the boost limit
            float jaroWinklerSimilarity = jaroSimilarity <= this.boostThreshold
                                       ? jaroSimilarity
                                       : jaroSimilarity + 0.1f * (float)this.GetCommonPrefix(str1, str2) * (1.0f - jaroSimilarity);

            return jaroWinklerSimilarity;
        }

        private int GetCommonPrefix(String str1, String str2)
        {
            if (str1 == null || str2 == null)
            {
                return this.prefixLength;
            }

            int commonPrefix = 0;
            int length = MathExtension.Min(str1.Length,
                                           str2.Length,
                                           this.prefixLength);

            // check for prefix similarity of length n
            for (int i = 0; i < length; i++)
            {
                // check the prefix is the same so far
                if (str1[i] == str2[i])
                {
                    // count up
                    commonPrefix++;
                }
            }

            return commonPrefix;
        }

        private int GetTranspositions(StringBuilder common1, StringBuilder common2)
        {
            // get the number of transpositions
            int transpositions = 0;
            int length = Math.Min(common1.Length,
                                  common2.Length);

            for (int i = 0; i < length; i++)
            {
                if (common1[i] != common2[i])
                {
                    transpositions++;
                }
            }

            transpositions = transpositions / 2;

            return transpositions;
        }

        /// <summary>
        /// return a string buffer of characters from string1 within string2 if they are of a given
        /// distance seperation from the position in string1
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="matchRange"></param>
        /// <returns></returns>
        private StringBuilder GetCommonCharacters(string str1, string str2, int matchRange)
        {
            if (str1 == null || str2 == null)
            {
                return null;
            }

            // create a return buffer of characters
            var commonChars = new StringBuilder();

            // create a copy of string2 for processing
            var copy = new StringBuilder(str2);

            // iterate over string1
            for (int i = 0; i < str1.Length; i++)
            {
                char chr1 = str1[i];

                // set boolean for quick loop exit if found
                bool found = false;

                // compare char with the range of characters to either side
                int startPos = Math.Max(0, i - matchRange);
                int endPos = Math.Min(i + matchRange, str2.Length);

                for (int j = startPos; !found && j < endPos; j++)
                {
                    // check if found
                    if (copy[j] == chr1)
                    {
                        // append character found
                        commonChars.Append(chr1);

                        // alter copied string2 for processing
                        copy[j] = (char)0;

                        found = true;
                    }
                }
            }

            return commonChars;
        }
    }
}
using System;
using System.Linq;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringTokenize;
using TwinFinder.Matching.StringTokenize.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    /// <summary>
    /// Build the Monge-Elkan score.
    /// This function is a hybrid function, it first tokenizes the string into (Word or NGrams)
    /// and then compares each tokens by a fuzzy string compare function (e.g. Levenshtein).
    /// str1 is compared to str2 and it is only only the highest score for each token1 in str1
    /// compared to token2 in str2 taken.
    /// </summary>
    public class MongeElkan : StringFuzzyComparer
    {
        // ***********************Fields***********************

        private StringFuzzyComparer fuzzyComparer = new DamerauLevenshteinDistance();

        private StringTokenizer tokenizer = new WordTokenizer();

        private bool useSpecialAbbreviationCheck = true;

        // *********************Properties*********************

        public StringFuzzyComparer FuzzyComparer
        {
            get { return this.fuzzyComparer; }
            set { this.fuzzyComparer = value; }
        }

        public bool UseSpecialAbbreviationCheck
        {
            get { return this.useSpecialAbbreviationCheck; }
            set { this.useSpecialAbbreviationCheck = value; }
        }

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

            if (str1.Equals(str2))
            {
                return 1.0f;
            }

            // building tokens of words

            // "Henri Waternoose" -> "Henri", "Waternoose"
            string[] tokens1 = this.tokenizer.Tokenize(str1);

            // "Henry Peter Waternose" -> "Henry", "Peter", "Waternose"
            string[] tokens2 = this.tokenizer.Tokenize(str2);

            float distance = this.BuildMongeElkan(tokens1, tokens2);

            return distance;

            //return this.NormalizeScore(tokens1, tokens2, distance);
        }

        private float NormalizeScore(string[] tokens1, string[] tokens2, float score)
        {
            float maxDistance = tokens1.Count() + tokens2.Count();

            return (maxDistance - score) / maxDistance;
        }

        private float BuildMongeElkan(string[] tokens1, string[] tokens2)
        {
            float[] maxSimilarities = new float[tokens1.Length];

            float similarity = 0.0f;

            // compare the tokens to each other, but take only the max similarity.
            // e.g. "Henri"  matches best to "Henry" and "Waternoose" to "Waternose"
            for (int i = 0; i < tokens1.Length; i++)
            {
                float maxSimilarity = 0.0f;
                for (int j = 0; j < tokens2.Length; j++)
                {
                    similarity = this.CompareTokens(tokens1[i], tokens2[j]);
                    maxSimilarity = Math.Max(maxSimilarity,
                                             similarity);
                }

                // store maximum similarity for token1
                maxSimilarities[i] = maxSimilarity;
            }

            // Mogne-Elkan similarity is defined as: SUM( maxTokenSim(ti,tj) )  / count(tokens1)
            float mongeElkan = maxSimilarities.Sum() / (float)tokens1.Length;

            return mongeElkan;
        }

        /// <summary>
        /// Compares two tokens.
        /// </summary>
        /// <param name="token1">The token1.</param>
        /// <param name="token2">The token2.</param>
        /// <returns></returns>
        private float CompareTokens(string token1, string token2)
        {
            // Check if one token is abbreviated (e.g "A." <-> "Albert"
            if (!string.IsNullOrEmpty(token1) && !string.IsNullOrEmpty(token2))
            {
                string abbreviate1 = token1.Substring(0, 1) + ".";
                string abbreviate2 = token2.Substring(0, 1) + ".";

                if (abbreviate1 == token2 || abbreviate2 == token1)
                {
                    return 0.9f;
                }
            }

            // standard compare
            float similarity = this.fuzzyComparer.Compare(token1, token2);
            return similarity;
        }
    }
}
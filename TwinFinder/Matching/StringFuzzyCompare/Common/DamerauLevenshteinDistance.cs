using System;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    /// <summary>
    /// Build the Damerau-Levenshtein Edit Distance.
    /// The algorithm does only recognizes differences in string, NOT matching parts.
    /// e.g. Lev(a,ab) = 1 = Lev(abcde, abcdef)
    /// The differences (substitution of chars, missing chars, inserted chars) can be different weighted (Costfunction).
    ///
    /// </summary>
    public class DamerauLevenshteinDistance : StringFuzzyComparer
    {
        public DamerauLevenshteinDistance()
        {
            this.CostFunction = new DefaultSubstitutionCostFunction();
        }

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

            float distance = this.BuildDamerauLevenshteinDistance(str1, str2);

            return this.NormalizeScore(str1, str2, distance);
        }

        private float NormalizeScore(string str1, string str2, float score)
        {
            if (str1 == null || str2 == null)
            {
                return 0;
            }

            // get the max score
            float maxLen = Math.Max(str1.Length, str2.Length);

            if (maxLen == 0)
            {
                return 1;
            }

            // return actual / possible levenstein distance to get 0-1 range
            return (1 - (score / maxLen));
        }

        public int BuildDamerauLevenshteinDistance(string str1, string str2)
        {
            if (str1 == null || str2 == null)
            {
                return 0;
            }

            int length1 = str1.Length;
            int length2 = str2.Length;

            if (length1 == 0)
            {
                return length2;
            }

            if (length2 == 0)
            {
                return length1;
            }

            int[,] matrix = new int[str1.Length + 1, str2.Length + 1];

            // initialize matrix
            for (int i = 0; i <= length1; i++) { matrix[i, 0] = i; }
            for (int j = 0; j <= length2; j++) { matrix[0, j] = j; }

            // analyze
            for (int i = 1; i <= length1; i++)
            {
                for (int j = 1; j <= length2; j++)
                {
                    int deletionCost = 1;
                    int insertionCost = 1;

                    // substitution is based on a cost function for different chars. e.g. m->n has lower cost than a->X
                    int substitionCost = (int)this.CostFunction.GetCost(str1, i - 1, str2, j - 1);

                    int del = matrix[i - 1, j];         // above is deletion
                    int ins = matrix[i, j - 1];         // left is insertion
                    int subst = matrix[i - 1, j - 1];   // diagonal is substitution

                    // new cell is the minium of all three precessors (left, diagonal, above).
                    // So the minumium moves to this cell plus the cost of the operation
                    int cell = MathExtension.Min(del + deletionCost,
                                                 ins + insertionCost,
                                                 subst + substitionCost);

                    // transposition e.g. "ab" <-> "ba" (Damerau-extension to Levensthein)
                    if (i > 1 && j > 1)
                    {
                        int transposition = matrix[i - 2, j - 2] + 1;
                        if (str1[i - 2] != str2[j - 1]) { transposition++; }
                        if (str1[i - 1] != str2[j - 2]) { transposition++; }
                        //trans += (int)this.CostFunction.GetCost(str1, i - 2, str2, j - 1);
                        //trans += (int)this.CostFunction.GetCost(str1, i - 1, str2, j - 2);
                        cell = MathExtension.Min(cell,
                                                transposition);
                    }

                    // put the new cellvalue back in the matrix
                    matrix[i, j] = cell;
                }
            }

            // return the last cell (right, bottom) of the matrix
            return matrix[length1, length2];
        }
    }
}
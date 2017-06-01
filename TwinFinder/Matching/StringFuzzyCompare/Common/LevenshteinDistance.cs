using System;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    /// <summary>
    /// Build the Levenshtein Edit Distance.
    /// The algorithm does only recognizes differences in string, NOT matching parts.
    /// e.g. Lev(a,ab) = 1 = Lev(abcde, abcdef)
    /// The differences (substitution of chars, missing chars, inserted chars) can be different weighted (Costfunction).
    ///
    /// </summary>
    public class LevenshteinDistance : StringFuzzyComparer
    {
        public LevenshteinDistance()
        {
            this.CostFunction = new DefaultSubstitutionCostFunction();
        }

        public override float Compare(string str1, string str2)
        {
            if (!this.CaseSensitiv)
            {
                str1 = str1.ToLower();
                str2 = str2.ToLower();
            }

            float distance = this.BuildLevenshteinDistance(str1, str2);

            return this.NormalizeScore(str1, str2, distance);
        }

        private float NormalizeScore(string str1, string str2, float score)
        {
            // get the max score
            float maxLen = Math.Max(str1.Length, str2.Length);

            if (maxLen == 0)
            {
                return 1;
            }

            // return actual / possible levenstein distance to get 0-1 range
            return (1 - (score / maxLen));
        }

        public int BuildLevenshteinDistance(string str1, string str2)
        {
            int[,] matrix = new int[str1.Length + 1, str2.Length + 1];

            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return -1;
            }

            // initialize matrix
            for (int i = 0; i <= str1.Length; i++) { matrix[i, 0] = i; }
            for (int j = 0; j <= str2.Length; j++) { matrix[0, j] = j; }

            // analyze
            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    int delCost = 1;
                    int insCost = 1;

                    // substituion is based on a cost function for different chars. e.g. m->n has lower cost than a->X
                    int substCost = (int)this.CostFunction.GetCost(str1, i - 1, str2, j - 1);

                    int del = matrix[i - 1, j];         // above is deletion
                    int ins = matrix[i, j - 1];         // left is insertion
                    int subst = matrix[i - 1, j - 1];   // diagonal is substitution

                    // new cell is the minium of all three precessors (left, diag, above).
                    // So the minumium moves to this cell plus the cost of the operation
                    int cell = MathExtension.Min(del + delCost,
                                                  ins + insCost,
                                                  subst + substCost);

                    // transposition e.g. "ab" <-> "ba"
                    if (i > 1 && j > 1)
                    {
                        int trans = matrix[i - 2, j - 2] + 1;
                        if (str1[i - 2] != str2[j - 1]) { trans++; }
                        if (str1[i - 1] != str2[j - 2]) { trans++; }
                        //trans += (int)this.CostFunction.GetCost(str1, i - 2, str2, j - 1);
                        //trans += (int)this.CostFunction.GetCost(str1, i - 1, str2, j - 2);
                        cell = MathExtension.Min(cell,
                                                trans);
                    }

                    // put the new cellvalue back in the matrix
                    matrix[i, j] = cell;
                }
            }

            // return the last cell (right, bottom) of the matrix
            return matrix[str1.Length, str2.Length];
        }
    }
}
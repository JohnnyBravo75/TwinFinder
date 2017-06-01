using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Enumerates the cost for matching
    ///
    /// match = 0
    /// mismatch = 1
    /// soft match (char is in the same chargroup) = 0.5
    /// </summary>
    public class CharGroupsSubstitutionCostFunction : StringCostFunction
    {
        private static List<string[]> charsGroups = new List<string[]>();

        public CharGroupsSubstitutionCostFunction()
        {
            charsGroups.Add(new[] { "d", "t" });
            charsGroups.Add(new[] { "g", "j" });
            charsGroups.Add(new[] { "l", "r" });
            charsGroups.Add(new[] { "m", "n" });
            charsGroups.Add(new[] { "b", "p", "v", "f" });
            charsGroups.Add(new[] { "a", "e", "i", "o", "u", "y" });
        }

        public override float GetCost(string str1, int str1Index, string str2, int str2Index)
        {
            if (str1.Length <= str1Index || str1Index < 0)
            {
                return 1.0f;
            }
            if (str2.Length <= str2Index || str2Index < 0)
            {
                return 1.0f;
            }

            if (str1[str1Index] == str2[str2Index])
            {
                return 0.0f;
            }
            else
            {
                string char1 = str1.ToLower()[str1Index].ToString();
                string char2 = str2.ToLower()[str2Index].ToString();

                foreach (var charGroup in charsGroups)
                {
                    if (charGroup.Contains(char1) && charGroup.Contains(char2))
                    {
                        return 0.5f;
                    }
                }

                return 1.0f;
            }
        }

        public override float MaxCost
        {
            get
            {
                return 1.0f;
            }
        }

        public override float MinCost
        {
            get
            {
                return 0.0f;
            }
        }
    }
}
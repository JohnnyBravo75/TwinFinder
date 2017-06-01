using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions
{
    /// <summary>
    /// Enumerates the cost for matching
    /// 
    /// match = 0
    /// mismatch = 1
    /// </summary>
    public class DefaultSubstitutionCostFunction : StringCostFunction
    {
        public override float GetCost(string str1, int str1Index, string str2, int str2Index)
        {
            if (str1[str1Index] == str2[str2Index])
            {
                return 0.0f;
            }
            else
            {
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

using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions
{
    public class SmithWatermanCostFunction : StringCostFunction
    {
        public override float GetCost(string str1, int index1, string str2, int index2)
        {
            // check within range
            if (str1.Length <= index1 || index1 < 0)
            {
                return 0;
            }
            if (str2.Length <= index2 || index2 < 0)
            {
                return 0;
            }

            if (str1[index1] == str2[index2])
            {
                return 1.0f;
            }
            else
            {
                return -2.0f;
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
                return -2.0f;
            }
        }
    }
}
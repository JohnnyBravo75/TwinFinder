namespace TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions.Base
{
    public interface IStringCostFunction
    {
        float GetCost(string str1, int str1Index, string str2, int str2Index);

        float MaxCost { get; }

        float MinCost { get; }
    }
}

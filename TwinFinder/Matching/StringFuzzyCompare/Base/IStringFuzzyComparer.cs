namespace TwinFinder.Matching.StringFuzzyCompare.Base
{
    public interface IStringFuzzyComparer
    {
        float Compare(string str1, string str2);
    }
}

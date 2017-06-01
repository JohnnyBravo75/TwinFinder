namespace TwinFinder.Matching.StringTokenize.Base
{
    public interface IStringTokenizer
    {
        string[] Tokenize(string str1);

        int MaxLength
        {
            get;
            set;
        }
    }
}
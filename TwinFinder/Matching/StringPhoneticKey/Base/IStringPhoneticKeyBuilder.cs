namespace TwinFinder.Matching.StringPhoneticKey.Base
{
    public interface IStringPhoneticKeyBuilder
    {
        string BuildKey(string str1);

        int MaxLength
        {
            get;
            set;
        }
    }
}

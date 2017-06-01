namespace TwinFinder.Matching.StringTokenize.Base
{
    public abstract class StringTokenizer : IStringTokenizer
    {
        // ***********************Fields***********************

        public abstract string[] Tokenize(string str1);

        private int maxLength = -1;

        // *********************Properties*********************

        public int MaxLength
        {
            get { return this.maxLength; }
            set { this.maxLength = value; }
        }
    }
}
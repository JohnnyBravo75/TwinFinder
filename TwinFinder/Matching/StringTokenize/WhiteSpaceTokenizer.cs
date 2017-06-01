using System.Text.RegularExpressions;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringTokenize.Base;

namespace TwinFinder.Matching.StringTokenize
{
    /// <summary>
    /// Tokenizes by whitespace.
    ///
    /// Examples:
    /// length = -1: "This.are some-words" -> "This.are" "some-words"
    /// length =  2: "This.are some-words" -> "Th" "so"
    /// </summary>
    public class WhiteSpaceTokenizer : StringTokenizer
    {
        public override string[] Tokenize(string str1)
        {
            return this.Tokens(str1, this.MaxLength);
        }

        public string[] Tokens(string str1, int maxLength)
        {
            // split by whitepaces
            var regEx = new Regex(@"\s+");
            string[] tokens = regEx.Split(str1);

            // cut to the length
            if (maxLength > 0)
            {
                int i = 0;
                foreach (string token in tokens)
                {
                    tokens[i] = token.TrySubstring(maxLength);
                    i++;
                }
            }

            return tokens;
        }
    }
}
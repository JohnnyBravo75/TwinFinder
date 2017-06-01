using System.Linq;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringTokenize.Base;

namespace TwinFinder.Matching.StringTokenize
{
    /// <summary>
    /// Takes the first N chars of each word.
    ///
    /// Examples:
    /// length = 1: "Hello world" -> "H w"
    /// length = 2: "Hello world" -> "He wo"
    /// </summary>
    public class FirstNCharsTokenizer : StringTokenizer
    {
        // ***********************Constructors***********************

        public FirstNCharsTokenizer()
        {
            this.MaxLength = 1;
        }

        public FirstNCharsTokenizer(int maxLength)
        {
            this.MaxLength = maxLength;
        }

        // ***********************Functions***********************

        public override string[] Tokenize(string str1)
        {
            // tokenize the first chars from a word "This is a-sentence" -> "Tias"
            string[] words = new WordTokenizer().Tokenize(str1);
            string[] firstChars = words.Select(d => StringExtensions.TrySubstring(d, this.MaxLength)).ToArray();
            return firstChars;
        }
    }
}
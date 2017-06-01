using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.StringPhoneticKey
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Builds a simple text key.
    /// 
    /// Rules:
    /// 1. removes vowels
    /// 2. Replaces duplicate characters
    /// 
    /// Examples:
    /// length = 4: "Müller" -> "MLR"
    /// </summary>
    public class SimpleTextKey : StringPhoneticKeyBuilder
    {
        public override string BuildKey(string str1)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return "";
            }

            return this.BuildSimpleTextKey(str1);
        }

        private string BuildSimpleTextKey(string str1)
        {
            str1 = str1.ToUpper();

            // replace umlauts "MÜLLER" -> "MUELLER"
            str1 = str1.Replace('Ä', 'A')
                       .Replace('Ö', 'O')
                       .Replace('Ü', 'U')
                       .Replace('ß', 'S');

            // strip all vowels "MUELLER" -> "MLLR"
            str1 = Regex.Replace(str1, "[AEIOU]", "", RegexOptions.IgnoreCase);

            // strip everything except a-z and 0-9
            str1 = Regex.Replace(str1, "[^a-zA-Z0-9]", "", RegexOptions.IgnoreCase);

            // removes duplicate chars "MLLR" - > "MLR"
            str1 = str1.RemoveDuplicateChars();

            // cut to the maxlength
            str1 = str1.TrySubstring(this.MaxLength);

            return str1;
        }
    }
}

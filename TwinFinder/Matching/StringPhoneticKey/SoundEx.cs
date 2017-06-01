using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.StringPhoneticKey
{
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Builds the soundex phonetic key of a string.
    ///
    /// Examples:
    /// length=4: "Miller"   -> "M460"
    /// length=4: "Peterson" -> "P362"
    /// length=4: "Peters"   -> "P362"
    ///
    /// Rules:
    /// 1. Replace all but the first letter of the string by its phonetic code.
    /// 2. Eliminate any adjacent repetitions of codes.
    /// 3. Eliminate all occurrences of code 0 (that is, eliminate vowels).
    /// 4. Return the first four characters of the resulting string.
    ///
    /// Soundex Table:
    /// 1 b,f,p,v
    /// 2 c,g,j,k,q,s,x,z
    /// 3 d, t
    /// 4 l
    /// 5 m, n
    /// 6 r
    /// </summary>
    public class SoundEx : StringPhoneticKeyBuilder
    {
        public override string BuildKey(string str1)
        {
            return this.BuildSoundEx(str1);
        }

        private string BuildSoundEx(string str1)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return string.Concat(Enumerable.Repeat("0", this.MaxLength));
            }

            // "Miller" -> "miller"
            str1 = str1.ToLower();

            bool firstCharRemains = true;
            int startIndex = 0;

            var result = new StringBuilder();

            if (str1 != null && str1.Length > 0)
            {
                string previousCode = "";
                string currentCode = "";
                string currentLetter = "";

                // replace umlauts "jörg" -> "jorg"
                str1 = str1.Replace('ä', 'a')
                           .Replace('ö', 'o')
                           .Replace('ü', 'u')
                           .Replace('ß', 's');

                // First letter remains "miller" -> "m"
                if (firstCharRemains)
                {
                    result.Append(str1.Substring(0, 1));
                    startIndex++;
                }

                for (int i = startIndex; i < str1.Length; i++)
                {
                    // take every letter
                    currentLetter = str1.Substring(i, 1).ToLower();
                    currentCode = "";

                    // encode letter by soundex table to a code
                    if ("bfpv".Contains(currentLetter))
                    {
                        currentCode = "1";
                    }
                    else if ("cgjkqsxz".Contains(currentLetter))
                    {
                        currentCode = "2";
                    }
                    else if ("dt".Contains(currentLetter))
                    {
                        currentCode = "3";
                    }
                    else if ("l".Contains(currentLetter))
                    {
                        currentCode = "4";
                    }
                    else if ("mn".Contains(currentLetter))
                    {
                        currentCode = "5";
                    }
                    else if ("r".Contains(currentLetter))
                    {
                        currentCode = "6";
                    }

                    // only add, when changes (elimates double characters)
                    if (currentCode != previousCode)
                    {
                        result.Append(currentCode);
                    }

                    // cancel, when length is reached
                    if (result.Length == this.MaxLength)
                    {
                        break;
                    }

                    if (currentCode != "")
                    {
                        previousCode = currentCode;
                    }
                }
            }

            // fill with zeros e.g. 000 to reach the maxlength
            if (result.Length < this.MaxLength)
            {
                result.Append(new string('0', (int)this.MaxLength - result.Length));
            }

            return result.ToString();
        }
    }
}
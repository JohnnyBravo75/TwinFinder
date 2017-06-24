using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.StringPhoneticKey
{
    using System.Text;

    /// <summary>
    /// Builds the phonix phonetic key of a string.
    ///
    /// Examples:
    /// length=-1: "Miller"   -> "50404"
    /// length=-1: "Peterson" -> "10304805"
    /// length=-1: "Peters"   -> "103048"
    ///
    /// phonix Table:
    /// 0 a,e,i,o,u,y,h,w
    /// 1 b,p
    /// 2 c,g,j,k,q
    /// 3 d,t
    /// 4 l
    /// 5 m,n
    /// 6 r
    /// 7 f,v
    /// 8 s,x,z
    /// </summary>
    public class Phonix : StringPhoneticKeyBuilder
    {
        public Phonix()
        {
            this.MaxLength = -1;
        }

        public override string BuildKey(string str1)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return "";
            }

            return this.BuildPhonix(str1);
        }

        private string BuildPhonix(string str1)
        {
            bool firstCharRemains = false;
            int startIndex = 0;

            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(str1))
            {
                string previousCode = "";
                string currentCode = "";
                string currentLetter = "";

                // replace umlauts
                str1 = str1.Replace('ä', 'a')
                           .Replace('ö', 'o')
                           .Replace('ü', 'u')
                           .Replace('ß', 's');

                // First letter as it is e.g. "M"
                if (firstCharRemains)
                {
                    result.Append(str1.Substring(0, 1));
                    startIndex++;
                }

                for (int i = startIndex; i < str1.Length; i++)
                {
                    currentLetter = str1.Substring(i, 1).ToLower();
                    currentCode = "";

                    // encode letter by phonix table to a code
                    if ("aeiouyhw".Contains(currentLetter))
                    {
                        currentCode = "0";
                    }
                    else if ("bp".Contains(currentLetter))
                    {
                        currentCode = "1";
                    }
                    else if ("cgjkq".Contains(currentLetter))
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
                    else if ("fv".Contains(currentLetter))
                    {
                        currentCode = "7";
                    }
                    else if ("sxz".Contains(currentLetter))
                    {
                        currentCode = "8";
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
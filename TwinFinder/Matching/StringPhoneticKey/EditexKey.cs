using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.StringPhoneticKey
{
    using System.Text;

    /// <summary>
    /// Builds the editex phonetic key of a string.
    ///
    /// Examples:
    /// length=-1: "Miller"   -> "50404"
    /// length=-1: "Peterson" -> "10304805"
    /// length=-1: "Peters"   -> "103048"
    ///
    /// editex Table:
    /// 0 a,e,i,o,u,y
    /// 1 b,p
    /// 2 c,k,q
    /// 3 d,t
    /// 4 l,r
    /// 5 m,n
    /// 6 g,j
    /// 7 f,p,v
    /// 8 s,x,z
    /// </summary>
    public class EditexKey : StringPhoneticKeyBuilder
    {
        public EditexKey()
        {
            this.MaxLength = -1;
        }

        public override string BuildKey(string str1)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return "";
            }

            return this.BuildEditex(str1);
        }

        private string BuildEditex(string str1)
        {
            str1 = str1.ToLower();

            bool firstCharRemains = false;
            int startIndex = 0;

            var result = new StringBuilder();

            if (str1 != null && str1.Length > 0)
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

                    // encode letter by editex table to a code
                    if ("aeiouy".Contains(currentLetter))
                    {
                        currentCode = "0";
                    }
                    else if ("bp".Contains(currentLetter))
                    {
                        currentCode = "1";
                    }
                    else if ("ckq".Contains(currentLetter))
                    {
                        currentCode = "2";
                    }
                    else if ("dt".Contains(currentLetter))
                    {
                        currentCode = "3";
                    }
                    else if ("lr".Contains(currentLetter))
                    {
                        currentCode = "4";
                    }
                    else if ("mn".Contains(currentLetter))
                    {
                        currentCode = "5";
                    }
                    else if ("gj".Contains(currentLetter))
                    {
                        currentCode = "6";
                    }
                    else if ("fpv".Contains(currentLetter))
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
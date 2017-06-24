/*	* * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * http://www.java2s.com/Open-Source/CSharp/Silverlight/bpm2/Common/Utilities/TextMatch/MetaphoneConverter.cs.htm
 * TODO: New implemention with rule table (http://www.zdnetasia.com/the-rules-of-metaphone-39200579.htm)
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.StringPhoneticKey
{
    using System.Text;

    /// <summary>
    /// Metaphone codes use the 16 consonant symbols 0BFHJKLMNPRSTWXY.[2]
    /// The '0' represents "th" (as an ASCII approximation of Θ), 'X' represents "sh" or "ch", and the others represent their usual English pronunciations. The vowels AEIOU are also used, but only at the beginning of the code.[3]
    ///
    /// Examples:
    ///
    /// length = -1: "Stephens" -> STFNS
    /// length = -1: "Stevens"  -> STFNS
    /// length = -1: "Theodore" -> 0TR
    /// length = -1: "Alvin"    -> Alvin
    ///
    /// Rules:
    /// 1.  Drop duplicate adjacent letters, except for C.
    /// 2.  If the word begins with 'KN', 'GN', 'PN', 'AE', 'WR', drop the first letter.
    /// 3.  Drop 'B' if after 'M' at the end of the word.
    /// 4.  'C' transforms to 'X' if followed by 'IA' or 'H' (unless in latter case, it is part of '-SCH-', in which case it transforms to 'K'). 'C' transforms to 'S' if followed by 'I', 'E', or 'Y'. Otherwise, 'C' transforms to 'K'.
    /// 5.  'D' transforms to 'J' if followed by 'GE', 'GY', or 'GI'. Otherwise, 'D' transforms to 'T'.
    /// 6.  Drop 'G' if followed by 'H' and 'H' is not at the end or before a vowel. Drop 'G' if followed by 'N' or 'NED' and is at the end.
    /// 7.  'G' transforms to 'J' if before 'I', 'E', or 'Y', and it is not in 'GG'. Otherwise, 'G' transforms to 'K'.
    /// 8.  Drop 'H' if after vowel and not before a vowel.
    /// 9.  'CK' transforms to 'K'.
    /// 10. 'PH' transforms to 'F'.
    /// 11. 'Q' transforms to 'K'.
    /// 12. 'S' transforms to 'X' if followed by 'H', 'IO', or 'IA'.
    /// 13. 'T' transforms to 'X' if followed by 'IA' or 'IO'. 'TH' transforms to '0'. Drop 'T' if followed by 'CH'.
    /// 14. 'V' transforms to 'F'.
    /// 15. 'WH' transforms to 'W' if at the beginning. Drop 'W' if not followed by a vowel.
    /// 16. 'X' transforms to 'S' if at the beginning. Otherwise, 'X' transforms to 'KS'.
    /// 17. Drop 'Y' if not followed by a vowel.
    /// 18. 'Z' transforms to 'S'.
    /// 19. Drop all vowels unless it is the beginning.
    /// </summary>
    public class Metaphone : StringPhoneticKeyBuilder
    {
        public override string BuildKey(string str1)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return "";
            }

            return this.BuildMetaphone(str1);
        }

        private string BuildMetaphone(string str1)
        {
            if (str1 == null)
            {
                return "";
            }

            var encodingBuilder = new StringBuilder();
            int charIndex = 0;

            //Do the BOW checks
            if (!string.IsNullOrEmpty(str1) && str1.Length >= 2)
                this.BeginningOfWordChecks(str1, encodingBuilder, ref charIndex);

            //Setup the previous char
            char previousChar = char.MinValue;
            if (charIndex > 0)
                previousChar = str1[charIndex - 1];

            while (charIndex < str1.Length)
            {
                char currentChar = str1[charIndex];
                char nextChar = (charIndex + 1 < str1.Length ? str1[charIndex + 1] : char.MinValue);
                char afterNextChar = (charIndex + 2 < str1.Length ? str1[charIndex + 2] : char.MinValue);

                //Silence the vowels
                if (this.IsVowel(currentChar) || (previousChar == currentChar && currentChar != 'C'))
                    charIndex++;
                else if (currentChar == 'B')
                {
                    if (charIndex < str1.Length)
                        encodingBuilder.Append("B");
                    charIndex++;
                }
                else if (currentChar == 'C')
                {
                    if (nextChar == 'H')
                    {
                        encodingBuilder.Append("X");
                        charIndex++;
                    }
                    else if (nextChar == 'I' && afterNextChar == 'A')
                    {
                        encodingBuilder.Append("X");
                        charIndex = charIndex + 2;
                    }
                    else if (nextChar == 'I' || nextChar == 'E' || nextChar == 'Y')
                    {
                        encodingBuilder.Append("S");
                        charIndex++;
                    }
                    else
                    {
                        encodingBuilder.Append("K");
                        if (nextChar == 'H' && previousChar == 'S')
                            charIndex++;
                    }
                    charIndex++;
                }
                else if (currentChar == 'D')
                {
                    if (nextChar == 'G' && (afterNextChar == 'E' || afterNextChar == 'Y' || afterNextChar == 'I'))
                    {
                        encodingBuilder.Append("J");
                        charIndex = charIndex + 2;
                    }
                    else
                    {
                        encodingBuilder.Append("T");
                    }
                    charIndex++;
                }
                //PH to F
                else if (currentChar == 'P')
                {
                    if (nextChar == 'H')
                    {
                        encodingBuilder.Append("F");
                        charIndex++;
                    }
                    else
                    {
                        encodingBuilder.Append("P");
                    }
                    charIndex++;
                }
                //Z to S
                else if (currentChar == 'Z')
                {
                    encodingBuilder.Append("S");
                    charIndex++;
                }
                //Silent if not followed be vowel
                else if (currentChar == 'Y' || currentChar == 'W')
                {
                    if (this.IsVowel(nextChar))
                    {
                        encodingBuilder.Append(currentChar);
                        charIndex++;
                    }
                    charIndex++;
                }
                //X to KS
                else if (currentChar == 'X')
                {
                    encodingBuilder.Append("KS");
                    charIndex++;
                }
                //V to F
                else if (currentChar == 'V')
                {
                    encodingBuilder.Append("F");
                    charIndex++;
                }
                //Q to K
                else if (currentChar == 'Q')
                {
                    encodingBuilder.Append("K");
                    charIndex++;
                }
                //K silent after C
                else if (currentChar == 'K')
                {
                    if (previousChar != 'C')
                        encodingBuilder.Append("K");
                    charIndex++;
                }
                //H silent if after vowel with no following vowel
                else if (currentChar == 'H')
                {
                    if (!this.IsVowel(previousChar) && !this.IsVowel(nextChar))
                        encodingBuilder.Append("H");
                    charIndex++;
                }
                //S to X or S
                else if (currentChar == 'S')
                {
                    if (nextChar == 'H')
                    {
                        encodingBuilder.Append("X");
                        charIndex++;
                    }
                    else if (nextChar == 'I' && (afterNextChar == 'A' || afterNextChar == 'O'))
                    {
                        encodingBuilder.Append("X");
                        charIndex = charIndex + 2;
                    }
                    else
                        encodingBuilder.Append("S");
                    charIndex++;
                }
                //T to X or 0 or T or silent
                else if (currentChar == 'T')
                {
                    if (nextChar == 'H')
                    {
                        encodingBuilder.Append("0");
                        charIndex++;
                    }
                    else if (nextChar == 'I' && (afterNextChar == 'A' || afterNextChar == 'O'))
                    {
                        encodingBuilder.Append("X");
                        charIndex = charIndex + 2;
                    }
                    else if (nextChar == 'C' && afterNextChar == 'H')
                    {
                        //DO nothing
                    }
                    else
                        encodingBuilder.Append("T");
                    charIndex++;
                }
                //G
                else if (currentChar == 'G')
                {
                    if (nextChar == 'H' && !(this.IsVowel(afterNextChar) || charIndex == str1.Length))
                        charIndex++;
                    else if (nextChar == 'N')
                    {
                        encodingBuilder.Append("N");
                        charIndex++;
                        if (str1.Length >= (charIndex + 1 + 2) && str1.Substring(charIndex + 1, 2) == "ED")
                            charIndex = charIndex + 2;
                    }
                    else if (nextChar == 'I' || nextChar == 'E' || nextChar == 'Y')
                    {
                        encodingBuilder.Append("J");
                        charIndex++;
                    }
                    else
                        encodingBuilder.Append("K");
                    charIndex++;
                }
                //Self to Self
                else
                {
                    encodingBuilder.Append(currentChar);
                    charIndex++;
                }

                //Exit if we've met our output limit
                if (encodingBuilder.Length == this.MaxLength)
                    break;

                //Retain thh last char
                previousChar = currentChar;
            }

            return encodingBuilder.ToString();
        }

        /// <summary>
        /// Returns, i fthe char is a vowel
        /// </summary>
        /// <param name="c">the character</param>
        /// <returns></returns>
        protected bool IsVowel(char c)
        {
            c = char.ToUpper(c);
            return ((c == 'A') || (c == 'E') || (c == 'I')
                 || (c == 'O') || (c == 'U'));
        }

        //Check the beginning of the word for special cases...
        private void BeginningOfWordChecks(string value, StringBuilder encodingBuilder, ref int charIndex)
        {
            string beginningOfWord = value.Substring(0, 2);

            if (beginningOfWord == "AE" || beginningOfWord == "GN"
                || beginningOfWord == "KN" || beginningOfWord == "PN"
                || beginningOfWord == "WR")
            {
                //Remove the first character if the word starts: AE, GN, KN, PN or WR.
                encodingBuilder.Append(beginningOfWord.Substring(1, 1));
                charIndex = 2;
            }
            else if (beginningOfWord == "WH")
            {
                //WH at the beginning of a w  ord becomes W.
                encodingBuilder.Append("W");
                charIndex = 2;
            }
            else
            {
                if (beginningOfWord[0] == 'X')
                {
                    encodingBuilder.Append("S");
                    charIndex = 1;
                }
                else if (this.IsVowel(beginningOfWord[0]))
                {
                    encodingBuilder.Append(beginningOfWord[0]);
                    charIndex = 1;
                }
            }
        }
    }
}
using System;
using System.Linq;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Common;
using TwinFinder.Matching.StringTokenize;
using UnidecodeSharpFork;

namespace TwinFinder.Matching.StringFuzzyCompare.AddressSpecific
{
    /// <summary>
    /// Compares two companies.
    /// 1. Normalize (lower, remove noise chars,...)
    /// 2. Check if company is shortened (e.g. IBM) and equals
    /// 3. Build fuzzy score from the shortened version vs the nomral strings
    /// 4. return the better score
    /// </summary>
    public class CompanyComparer : StringFuzzyComparer
    {
        public override float Compare(string str1, string str2)
        {
            string name1 = this.Normalize(str1);
            string name2 = this.Normalize(str2);

            // check if company is shortened like "International Business Machines" -> "IBM"
            string firstChars1 = this.GetFirstCharsFromWords(name1).Join("");
            string firstChars2 = this.GetFirstCharsFromWords(name2).Join("");

            if (firstChars1 == firstChars2)
            {
                // company name is shortened an equals (e.g. "IBM" == "IBM")
                return 0.9f;
            }

            StringFuzzyComparer comparer = new DamerauLevenshteinDistance();
            float similarityShortened = comparer.Compare(firstChars1, firstChars2);
            float similarityNormal = comparer.Compare(name1, name2);

            // return what is better: the shortened version vs. the normal version
            return Math.Max(similarityShortened, similarityNormal);
        }

        /// <summary>
        /// Return the first char from each word in the string
        /// </summary>
        /// <param name="str">the string</param>
        /// <returns>the chars</returns>
        private string[] GetFirstCharsFromWords(string str)
        {
            string[] words = new WordTokenizer().Tokenize(str);
            string[] firstChars = null;
            if (StringExtensions.Count(words) <= 2)
            {
                firstChars = words[0].ToStringArray();
            }
            else
            {
                firstChars = words.Select(d => d.TrySubstring(1)).ToArray();
            }

            return firstChars;
        }

        private string Normalize(string str)
        {
            // Transliterate
            str = str.Unidecode();

            // replace umlauts
            str = Normalizer.ReplaceUmlauts(str);

            // remove diacritics and accents e.g. Société Générale -> Societe Generale
            str = Normalizer.RemoveDiacritics(str);

            // replaces separation chars with spaces
            str = Normalizer.RemoveNoiseChars("-.,()", " ");

            // remove multiple spaces
            str = Normalizer.RemoveMultipleSpaces(str);

            // lower the string
            str = str.ToLower();

            // trim whitespaces
            str = str.Trim();

            return str;
        }
    }
}
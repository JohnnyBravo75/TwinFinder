using System;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Common;
using UnidecodeSharpFork;

namespace TwinFinder.Matching.StringFuzzyCompare.AddressSpecific
{
    /// http://www.basistech.com/text-analytics/rosette/name-indexer/

    public class NameComparer : StringFuzzyComparer
    {
        // Typos on first letter are much more rare.  Max score 0.6
        public static float MAX_SCORE_FOR_NO_FIRST_LETTER_MATCH = 0.6f;

        public override float Compare(string str1, string str2)
        {
            float similarity = 0.0f;

            string name1 = str1;
            string name2 = str2;

            // check if name is shortened like "Müller" -> "M."
            if (name1.EndsWith(".") || name2.EndsWith("."))
            {
                // normalize "M.-Thurgau" -> "m thurgau"
                name1 = this.Normalize(name1);
                name2 = this.Normalize(name2);

                // take length of the shortened name "M"
                int minLength = Math.Min(name1.Length, name2.Length);
                name1 = name1.TrySubstring(minLength);
                name2 = name2.TrySubstring(minLength);

                StringFuzzyComparer comparer = new DamerauLevenshteinDistance();
                similarity = comparer.Compare(name1, name2);

                // reduce similarity, 100% cannot be reached, when one name is shortened
                similarity = similarity * 0.8f;
            }
            else
            {
                // normalize "M.-Thurgau" -> "m thurgau"
                name1 = this.Normalize(name1);
                name2 = this.Normalize(name2);

                StringFuzzyComparer comparer = new DamerauLevenshteinDistance();
                similarity = comparer.Compare(name1, name2);

                // Reduce the score if the first letters don't match
                //if (name1.CharAt(0) != name2.CharAt(0))
                //{
                //    similarity = Math.Min(similarity, MAX_SCORE_FOR_NO_FIRST_LETTER_MATCH);
                //}
            }

            return similarity;
        }

        private string Normalize(string str)
        {
            // Transliterate
            str = str.Unidecode();

            // replace umlauts
            str = Normalizer.ReplaceUmlauts(str);

            // remove diacritics and accents e.g. Pathé -> Pathe
            str = Normalizer.RemoveDiacritics(str);

            // replaces separation chars with spaces "Müller-Thurgau" -> "Müller Thurgau"
            str = Normalizer.RemoveNoiseChars(str, "-.,()", ' ');

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
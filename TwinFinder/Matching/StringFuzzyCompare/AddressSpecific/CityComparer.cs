using System;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Common;
using UnidecodeSharpFork;

namespace TwinFinder.Matching.StringFuzzyCompare.AddressSpecific
{
    public class CityComparer : StringFuzzyComparer
    {
        //private StringBuilder msg = new StringBuilder();

        public override float Compare(string str1, string str2)
        {
            float isMainCityFactor = 1f;
            float similarity = 0.0f;

            // normalize "Wiesbaden-Dotzheim" -> "wiesbaden-dotzheim"
            string city1 = this.Normalize(str1);
            string city2 = this.Normalize(str2);
            //msg.AppendLine("Normalize1:" + str1 + " -> " + city1);
            //msg.AppendLine("Normalize2:" + str2 + " -> " + city2);

            // "Mainz-Bingen/Bingen" -> "Bingen"
            if (city1.Contains("/"))
            {
                city1 = this.GetLeftPart(city1, "/");
                isMainCityFactor = 0.9f;
            }
            else if (city1.Contains("-"))
            {
                // e.g. "Wiesbaden-Dotzheim"
                // e.g. "Mainz-Bingen"
                city1 = this.GetLeftPart(city1, "-");
                isMainCityFactor = 0.9f;
            }

            if (city2.Contains("/"))
            {
                city2 = this.GetLeftPart(city2, "/");
                isMainCityFactor = 0.9f;
            }
            else if (city2.Contains("-"))
            {
                city2 = this.GetLeftPart(city2, "-");
                isMainCityFactor = 0.9f;
            }

            StringFuzzyComparer comparer = new DamerauLevenshteinDistance();
            similarity = comparer.Compare(city1, city2);

            // reduce similarity, 100% cannot be reached, when one city is only part of the other
            similarity = similarity * isMainCityFactor;

            return similarity;
        }

        private string GetLeftPart(string city, string separator = "")
        {
            if (string.IsNullOrEmpty(separator))
            {
                return city;
            }

            // get left part (main city) "wiesbaden-dotzheim" -> "wiesbaden"
            int separatorIndex = city.IndexOf(separator, StringComparison.Ordinal);
            if (separatorIndex != -1)
            {
                city = city.Left(separatorIndex);
            }
            return city;
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
            str = Normalizer.RemoveNoiseChars(str, "-.,()", ' ');

            // remove multiple spaces
            str = Normalizer.RemoveMultipleSpaces(str);

            // lower the string
            str = str.ToLower();

            // trim whitespaces
            str = str.Trim();

            return str;
        }

        //public string Explain(string str1, string str2)
        //{
        //    this.Compare(str1, str2);
        //    return msg.ToString();
        //}
    }
}
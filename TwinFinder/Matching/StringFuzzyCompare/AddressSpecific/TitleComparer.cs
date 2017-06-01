using System.Linq;
using System.Text.RegularExpressions;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringTokenize;
using TwinFinder.Matching.StringTokenize.Base;
using UnidecodeSharpFork;

namespace TwinFinder.Matching.StringFuzzyCompare.AddressSpecific
{
    public class TitleComparer : StringFuzzyComparer
    {
        public override float Compare(string str1, string str2)
        {
            float similarity = 0.0f;

            if (str1.Trim().ToLower() == str2.Trim().ToLower())
            {
                // exact match
                return 1.0f;
            }

            // normalize the titles
            string title1 = this.Normalize(str1);
            string title2 = this.Normalize(str2);

            // split the titles into a list
            StringTokenizer tokenizer = new WordTokenizer();
            string[] titles1 = tokenizer.Tokenize(title1);
            string[] titles2 = tokenizer.Tokenize(title2);

            // take only distinct values (e.g. "Dr Dr" -> "Dr")
            titles1 = titles1.Distinct().ToArray();
            titles2 = titles2.Distinct().ToArray();

            if (titles1.Length == title2.Length && titles1.ContainsAll(titles2) && titles2.ContainsAll(titles1))
            {
                // exact match (e.g. "Prof Dr" == "Prof Dr")
                similarity = 1;
            }
            else if (titles1.ContainsAll(titles2) || titles2.ContainsAll(titles1))
            {
                // one title is subset of the other titles (e.g. "Prof Dr Ing" <-> "Dr Ing")
                similarity = 0.5f;
            }
            else
            {
                // no matching of the titles
                similarity = 0f;
            }

            return similarity;
        }

        private string Normalize(string str)
        {
            // Transliterate
            str = str.Unidecode();

            // remove diacritics and accents e.g. Pathé -> Pathe
            str = Normalizer.RemoveDiacritics(str);

            // replaces separation chars with spaces
            //   break up Dr.-Ing and Dipl-Ing into their parts
            //   could be spelled Dipl-Ing, Dipl.-Ing., DiplIng, ...
            //   therefore, replace all possible dividers (-, .) by blanks
            str = Normalizer.RemoveNoiseChars("-.,", " ");

            // use camelCase to break the title (e.g. "ProfDrIng" -> "Prof Dr Ing")
            str = Regex.Replace(str, @"(?<=[a-z])(?=[A-Z])", " ");

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
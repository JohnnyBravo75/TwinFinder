using System;
using System.Text.RegularExpressions;
using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringFuzzyCompare.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.AddressSpecific
{
    public class PhoneComparer : StringFuzzyComparer
    {
        public override float Compare(string str1, string str2)
        {
            string phone1 = this.Normalize(str1);
            string phone2 = this.Normalize(str2);

            // reverse the number, we look from the back, e.g
            // (+49)7622 7239208 -> 8029327 2267)94+(
            // 07622-7239208 -> 8029327-22670

            phone1 = phone1.Reverse();
            phone2 = phone2.Reverse();

            int matchingDigits = 0;
            int minLength = Math.Min(phone1.Length, phone2.Length);

            // count how many digits match
            for (int i = 0; i < minLength; i++)
            {
                if (phone1.CharAt(i) == phone2.CharAt(i))
                {
                    matchingDigits++;
                }
                else
                {
                    // when not matching, cancel! No second chance..., only exact matching counts.
                    break;
                }
            }

            float maxLen = Math.Max(phone1.Length, phone2.Length);

            float similarity = matchingDigits / (1.0f * maxLen);

            return similarity;
        }

        private string Normalize(string str)
        {
            // trim whitespaces
            str = str.Trim();

            str = str.Replace("+", "00");

            // replace all non digits with nothing
            Regex regEx = new Regex("[^0-9]");

            str = regEx.Replace(str, "");

            return str;
        }
    }
}
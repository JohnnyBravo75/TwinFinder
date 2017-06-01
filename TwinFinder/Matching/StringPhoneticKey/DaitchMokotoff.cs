using TwinFinder.Base.Extensions;
using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.StringPhoneticKey
{
    using System.Text;

    /// <summary>
    /// Builds the DaitchMokotoff key of a string.
    ///
    /// Examples:
    /// length = 6: "Peters"    -> 739400, 734000
    /// length = 6: "Peterson"  -> 739400, 734000
    /// length = 6: "Moskowitz" -> 645740
    /// length = 6: "Jackson"   -> 154600, 454600, 145460, 445460
    ///
    /// Rules:
    /// 1. Town names are coded to six digits, each digit representing a sound listed in the Coding Chart below.
    /// 2. The letters A, E, I, O, U, J and Y are always coded at the beginning of a name, as in Augsburg (054795). In any other situation, they are ignored except when two of them form a pair and the pair comes before a vowel, as in Breuer (791900), but not Freud. The letter "H" is coded at the beginning of a name, as in Halberstadt (587943) or preceding a vowel as in Mannheim (665600), otherwise it is not coded.
    /// 3. When adjacent sounds can combine to form a larger sound, they are given the code number of the larger sound, as in Chernowitz, which is not coded Chernowi-t- z (496734) but Chernowi-tz (496740).
    /// 4. When adjacent letters have the same code number, they are coded as one sound, as in Cherkassy, which is not coded Cherka-s-sy (495440) but Cherkassy (495400). Exceptions to this rule are the letter combinations "MN" and "NM" whose letters are coded separately, as in Kleinman which is coded 586660 not 586600.
    /// 5. When a name consists of more than one word, it is coded as if one word, such as Nowy Targ, which is treated as Nowytarg.
    /// 6. Several letters and letter combinations pose the problem that they may sound in one of two ways. The letter and letter combinations CH, CK, C, J and RZ (see chart below), are assigned two possible code numbers. Be sure to try both possibilities.
    /// 7. When a name lacks enough coded sounds to fill the six digits, the remaining digits are coded "0" as in Berlin (798600) which has only four coded sounds (B-R- L-N).
    /// </summary>
    public class DaitchMokotoff : StringPhoneticKeyBuilder
    {
        public override string BuildKey(string str1)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return "";
            }

            return this.BuildDaitchMokotoff(str1);
        }

        private string BuildDaitchMokotoff(string str1)
        {
            if (this.MaxLength == -1)
            {
                this.MaxLength = 6;
            }

            // empty string
            if (string.IsNullOrEmpty(str1.Trim()))
            {
                return "".PadRight(this.MaxLength, '0');
            }

            int i;
            int n = 0;
            int pos = 0;
            string sound;
            string lastSound = null;
            var result = new StringBuilder();

            str1 = str1.ToUpper() + "*";

            while ((this.daimok_rules[n].Length > 0) && (str1.Length > 0))
            {
                if (str1.RangeMatches(pos, this.daimok_rules[n], 0, this.daimok_rules[n].Length))
                {
                    /* check the position of the sound */
                    if (pos == 0)
                    {
                        /* the beginning */
                        sound = this.daimok_rules[n + 1];
                        pos = pos + this.daimok_rules[n].Length;
                    }
                    else
                    {
                        pos = pos + this.daimok_rules[n].Length;

                        if ((str1[pos] == 'A') || (str1[pos] == 'E') || (str1[pos] == 'I') || (str1[pos] == 'O') || (str1[pos] == 'U'))
                        {
                            /* vor einem Vokal */
                            sound = this.daimok_rules[n + 2];
                        }
                        else
                        {
                            sound = this.daimok_rules[n + 3];
                        }
                    }

                    // no doubles
                    if (!sound.Equals(lastSound))
                    {
                        result.Append(sound);
                        lastSound = sound;
                    }

                    n = 0;
                }
                else
                {
                    n = n + 4; // jump to next rule
                }
            }

            if (result.Length > this.MaxLength)
            {
                result.Length = (int)this.MaxLength;
            }
            else
            {
                // fill up with zeros
                for (i = result.Length; i < this.MaxLength; i++)
                {
                    result.Append(0);
                }
            }

            return result.ToString();
        }

        // rule table for DaitchMokotoff algorithmus.
        // "sound", "code when in beginning of the string", "code when before a vowel", "code when the others don´t match"
        private readonly string[] daimok_rules = {
                                                            "ZSCH", "4", "4", "4",
                                                            "ZSH", "4", "4", "4",
                                                            "TCH", "4", "4", "4",
                                                            "TTCH", "4", "4", "4",
                                                            "TTSCH", "4", "4", "4",
                                                            "TH", "3", "3", "3",
                                                            "TRZ", "4", "4", "4",
                                                            "TRS", "4", "4", "4",
                                                            "TSCH", "4", "4", "4",
                                                            "TSH", "4", "4", "4",
                                                            "TC", "4", "4", "4",
                                                            "SCHTSH", "2", "4", "4",
                                                            "SCHTCH", "2", "4", "4",
                                                            "SCHTSCH", "2", "4", "4",
                                                            "SHTCH", "2", "4", "4",
                                                            "SHCH", "2", "4", "4",
                                                            "SHTSH", "2", "4", "4",
                                                            "SHT", "2", "43", "43",
                                                            "SCHT", "2", "43", "43",
                                                            "SCHD", "2", "43", "43",
                                                            "STCH", "2", "4", "4",
                                                            "STSCH", "2", "4", "4",
                                                            "STRZ", "2", "4", "4",
                                                            "STRS", "2", "4", "4",
                                                            "STSH", "2", "4", "4",
                                                            "SZCZ", "2", "4", "4",
                                                            "SZCS", "2", "4", "4",
                                                            "SZT", "2", "43", "43",
                                                            "SHD", "2", "43", "43",
                                                            "SZD", "2", "43", "43",
                                                            "SD", "2", "43", "43",
                                                            "STSCH", "2", "4", "4",
                                                            "SH", "4", "4", "4",
                                                            "SCH", "4", "4", "4",
                                                            "SC", "2", "4", "4",
                                                            "ZDZH", "2", "4", "2",
                                                            "ZHDZH", "2", "4", "4",
                                                            "ZDZ", "2", "4", "4",
                                                            "ZHD", "2", "43", "43",
                                                            "ZD", "2", "43", "43",
                                                            "ZH", "4", "4", "4",
                                                            "ZS", "4", "4", "4",
                                                            "AI", "0", "1", "",
                                                            "AJ", "0", "1", "",
                                                            "AY", "0", "1", "",
                                                            "AU", "0", "7", "",
                                                            "B", "7", "7", "7",
                                                            "CHS", "5", "54", "54",
                                                            "TCH", "4", "4", "4",
                                                            "CH", "5", "5", "5",
                                                            "CK", "5", "5", "5",
                                                            "CZS", "4", "4", "4",
                                                            "CSZ", "4", "4", "4",
                                                            "CZ", "4", "4", "4",
                                                            "C", "5", "5", "5",
                                                            "DRZ", "4", "4", "4",
                                                            "DRS", "4", "4", "4",
                                                            "DSZ", "4", "4", "4",
                                                            "DSH", "4", "4", "4",
                                                            "DS", "4", "4", "4",
                                                            "DZH", "4", "4", "4",
                                                            "DZS", "4", "4", "4",
                                                            "DZ", "4", "4", "4",
                                                            "DT", "3", "3", "3",
                                                            "D", "3", "3", "3",
                                                            "EI", "0", "1", "",
                                                            "EJ", "0", "1", "",
                                                            "EY", "0", "1", "",
                                                            "EU", "1", "1", "",
                                                            "IE", "1", "", "",
                                                            "UE", "0", "", "",
                                                            "E", "0", "", "",
                                                            "FB", "7", "7", "7",
                                                            "F", "7", "7", "7",
                                                            "G", "5", "5", "5",
                                                            "H", "5", "5", "",
                                                            "IA", "1", "", "",
                                                            "IO", "1", "", "",
                                                            "IU", "1", "", "",
                                                            "OI", "0", "1", "",
                                                            "OJ", "0", "1", "",
                                                            "UI", "0", "1", "",
                                                            "UJ", "0", "1", "",
                                                            "I", "0", "", "",
                                                            "J", "1", "1", "1",
                                                            "KS", "5", "54", "54",
                                                            "KH", "5", "5", "5",
                                                            "K", "5", "5", "5",
                                                            "L", "8", "8", "8",
                                                            "MN", "66", "66", "66",
                                                            "NM", "66", "66", "66",
                                                            "M", "6", "6", "6",
                                                            "N", "6", "6", "6",
                                                            "OY", "0", "1", "",
                                                            "O", "0", "", "",
                                                            "PF", "7", "7", "7",
                                                            "PH", "7", "7", "7",
                                                            "P", "7", "7", "7",
                                                            "Q", "5", "5", "5",
                                                            "RZ", "94", "94", "94",
                                                            "RS", "94", "94", "94",
                                                            "R", "9", "9", "9",
                                                            "ST", "2", "43", "43",
                                                            "SZ", "4", "4", "4",
                                                            "S", "4", "4", "4",
                                                            "TTS", "4", "4", "4",
                                                            "TTSZ", "4", "4", "4",
                                                            "TS", "4", "4", "4",
                                                            "TTZ", "4", "4", "4",
                                                            "TZS", "4", "4", "4",
                                                            "TSZ", "4", "4", "4",
                                                            "TZ", "4", "4", "4",
                                                            "T", "3", "3", "3",
                                                            "UY", "0", "1", "",
                                                            "V", "7", "7", "7",
                                                            "W", "7", "7", "7",
                                                            "X", "5", "54", "54",
                                                            "Y", "1", "", "",
                                                            "Z", "4", "4", "4",
                                                            "A", "0", "", "",
                                                            "ß", "4", "4", "4",
                                                            "Ä", "0", "", "",
                                                            "Ö", "0", "", "",
                                                            "Ü", "0", "", "",
                                                            " ", "", "", "",
                                                            "", "", "", "" };
    }
}
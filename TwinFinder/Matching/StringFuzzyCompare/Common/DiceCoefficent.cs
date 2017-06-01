using System.Linq;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringTokenize;
using TwinFinder.Matching.StringTokenize.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    /// <summary>
    /// Build the token based Dice coefficient, which is defined as:
    ///
    /// (2* |X & Y| ) / ( | X + Y | )
    /// 2* innerer Schnitt / Summe der übereinstimmenden Tokens
    /// </summary>
    public class DiceCoefficent : StringFuzzyComparer
    {
        // ***********************Fields***********************

        private int nGramLength = 2;

        private StringTokenizer tokenizer = new NGramTokenizer();

        // ***********************Functions***********************

        public override float Compare(string str1, string str2)
        {
            if (str1 == null || str2 == null)
            {
                return 0;
            }

            if (!this.CaseSensitiv)
            {
                str1 = str1.ToLower();
                str2 = str2.ToLower();
            }

            // building NGram of length=2

            this.tokenizer.MaxLength = this.nGramLength;

            // "Müller" -> #M Mü ül ll le er r#
            string[] ngrams1 = this.tokenizer.Tokenize(str1.ToLower());

            // "Meier" -> #M Me ei ie er r#
            string[] ngrams2 = this.tokenizer.Tokenize(str2.ToLower());

            float dice = this.BuildDiceCoefficient(ngrams1, ngrams2);

            return this.NormalizeScore(str1, str2, dice);
        }

        private float NormalizeScore(string str1, string str2, float score)
        {
            // no normalization is needed it has already the range 0-1
            return score;
        }

        /// <summary>
        /// Dice Coefficient used to compare nGrams arrays = 2 * matchingNGrams / totalNGrams
        /// </summary>
        /// <param name="nGrams1"></param>
        /// <param name="nGrams2"></param>
        /// <returns></returns>
        private float BuildDiceCoefficient(string[] nGrams1, string[] nGrams2)
        {
            // all tokens together -> #M Mü ül ll le er r#  #M Me ei ie er r#
            float totalNGrams = nGrams1.Count() + nGrams2.Count();

            // inner set of tokens -> #M r#
            float matchingNGrams = nGrams1.Where(x => nGrams2.Contains(x)).Count();

            if (matchingNGrams == 0)
            {
                return 0.0f;
            }

            // (2* |X & Y| ) / ( | X + Y | )
            // 2* innerer Schnitt / Summe der übereinstimmenden Tokens
            return (2 * matchingNGrams) / totalNGrams;
        }
    }
}
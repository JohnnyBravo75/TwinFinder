using System;
using System.Linq;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringTokenize;
using TwinFinder.Matching.StringTokenize.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Common
{
    public class NGramDistance : StringFuzzyComparer
    {
        // ***********************Fields***********************

        private int nGramLength = 2;

        private StringTokenizer tokenizer = new NGramTokenizer();

        // ***********************Properties***********************

        public int NGramLength
        {
            get { return this.nGramLength; }
            set { this.nGramLength = value; }
        }

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

            if (str1.Equals(str2))
            {
                return 1.0f;
            }

            // building NGram of length=3
            this.tokenizer.MaxLength = this.nGramLength;

            // tokenize into NGrams e.g "Müller" -> ##M #Mü Mül üll lle ler er# r##
            string[] nGrams1 = this.tokenizer.Tokenize(str1);

            // tokenize into NGrams e.g "Meier" -> ##M #Me Mei eie ier er# r##
            string[] nGrams2 = this.tokenizer.Tokenize(str2);

            float distance = this.BuildNGramDistance(nGrams1, nGrams2);

            return this.NormalizeScore(nGrams1, nGrams2, distance);
        }

        private float NormalizeScore(string[] nGrams1, string[] nGrams2, float score)
        {
            float maxDistance = nGrams1.Count() + nGrams2.Count();

            if (maxDistance == 0)
            {
                return 0;
            }

            return (maxDistance - score) / maxDistance;
        }

        private float BuildNGramDistance(string[] nGrams1, string[] nGrams2)
        {
            int distance = 0;

            // T = |X or Y|  Vereinigungsmenge
            string[] unionNGrams = nGrams1.Union(nGrams2).ToArray();

            foreach (string nGram in unionNGrams)
            {
                // create local variable (http://ericlippert.com/2009/11/12/closing-over-the-loop-variable-considered-harmful-part-one/)
                var currentNGram = nGram;

                // |T & X|  innerer Schnitt
                int countInStr1 = nGrams1.Where(d => d == currentNGram).Count();

                // |T & Y|  innerer Schnitt
                int countInStr2 = nGrams2.Where(d => d == currentNGram).Count();

                distance += Math.Abs(countInStr1 - countInStr2);
            }

            return distance;
        }
    }
}
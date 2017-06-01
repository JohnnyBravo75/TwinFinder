using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Common;

namespace TwinFinder.Test
{
    [TestClass]
    public class CompareTest
    {
        [TestMethod]
        public void NameCompareTest()
        {
            //name matching
            string input = "Albert Einstein";
            string[] testCases =  {
                "Albert Einstein",
                "A. Einstein",
                "Einstein Albert",
				"Feinstein Albert",
				"Kleinstein Albrecht",
                "Albert Feinstein",
                "Albrecht Kleinstein",
                "Kalbert Einstein",
                "Heinz Einstein",
                "Albert Zweistein"
				};

            this.StringCompareTest(input, testCases);
        }

        [TestMethod]
        public void AddressCompareTest()
        {
            //address matching
            string input = "2130 South Fort Union Blvd.";
            string[] testCases =  {
                "2130 South Fort Union Blvd.",
				"2689 East Milkin Ave.",
				"85 Morrison",
				"2350 North Main",
				"567 West Center Street",
				"2130 Fort Union Boulevard",
				"2310 S. Ft. Union Blvd.",
				"98 West Fort Union",
				"Rural Route 2 Box 29",
				"PO Box 2130",
				"30 Harvard Blvd."
				};

            this.StringCompareTest(input, testCases);
        }

        private void StringCompareTest(string input, string[] testCases)
        {
            Debug.WriteLine("Dice Coefficient for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer dice = new DiceCoefficent();
                double diceValue = dice.Compare(input, name);
                Debug.WriteLine("\t{0} against {1}", diceValue.ToString("###,###.00000"), name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("Jaccard Coefficient for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer jaccard = new Jaccard();
                double jaccardValue = jaccard.Compare(input, name);
                Debug.WriteLine("\t{0} against {1}", jaccardValue.ToString("###,###.00000"), name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("ExtendedJaccard Coefficient for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer exjaccard = new ExtendedJaccard();
                double exjaccardValue = exjaccard.Compare(input, name);
                Debug.WriteLine("\t{0} against {1}", exjaccardValue.ToString("###,###.00000"), name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("DamerauLevenshteinDistance for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer lev = new DamerauLevenshteinDistance();
                var levenStein = lev.Compare(input, name);
                Debug.WriteLine("\t{0}, against {1}", levenStein, name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("JaroWinkler for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer jw = new JaroWinkler();
                var jwValue = jw.Compare(input, name);
                Debug.WriteLine("\t{0}, against {1}", jwValue, name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("Monge-Elkan for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer me = new MongeElkan();
                var meValue = me.Compare(input, name);
                Debug.WriteLine("\t{0}, against {1}", meValue, name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("NGramDistance(2) for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer ngram2 = new NGramDistance();
                (ngram2 as NGramDistance).NGramLength = 2;
                var ngramValue2 = ngram2.Compare(input, name);

                Debug.WriteLine("\t{0}, against {1}", ngramValue2, name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("SmithWaterman for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer sw = new SmithWaterman();
                var swValue = sw.Compare(input, name);
                Debug.WriteLine("\t{0}, against {1}", swValue, name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("Extended Editex for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer edx = new ExtendedEditex();
                var edxValue = edx.Compare(input, name);
                Debug.WriteLine("\t{0}, against {1}", edxValue, name);
            }

            Debug.WriteLine("");
            Debug.WriteLine("Longest Common Subsequence for {0}:", input);
            foreach (var name in testCases)
            {
                StringFuzzyComparer lcs = new LongestCommonSubsequence();
                var lcsValue = lcs.Compare(input, name);
                Debug.WriteLine("\t{0}, against {1}", lcsValue.ToString("###,###.00000"), name);
            }

            Debug.WriteLine("");
        }
    }
}
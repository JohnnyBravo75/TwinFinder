using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwinFinder.Matching.StringPhoneticKey;
using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Test
{
    [TestClass]
    public class MatchKeyTest
    {
        [TestMethod]
        public void NameMatchKeyTest()
        {
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

            this.StringMatchKeyTest(testCases);
        }

        [TestMethod]
        public void AddressMatchKeyTest()
        {
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

            this.StringMatchKeyTest(testCases);
        }

        private void StringMatchKeyTest(string[] testCases)
        {
            Debug.WriteLine("EditexKey");
            StringPhoneticKeyBuilder keyBuilder = new EditexKey();
            foreach (var name in testCases)
            {
                string key = keyBuilder.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }

            Debug.WriteLine("DaitchMokotoff");
            StringPhoneticKeyBuilder keyBuilder2 = new DaitchMokotoff();
            foreach (var name in testCases)
            {
                string key = keyBuilder2.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }

            Debug.WriteLine("Phonix");
            StringPhoneticKeyBuilder keyBuilder3 = new Phonix();
            foreach (var name in testCases)
            {
                string key = keyBuilder3.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }

            Debug.WriteLine("SoundEx");
            StringPhoneticKeyBuilder keyBuilder4 = new SoundEx();
            foreach (var name in testCases)
            {
                string key = keyBuilder4.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }

            Debug.WriteLine("SimpleTextKey");
            StringPhoneticKeyBuilder keyBuilder5 = new SimpleTextKey();
            foreach (var name in testCases)
            {
                string key = keyBuilder5.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }

            Debug.WriteLine("Metaphone");
            StringPhoneticKeyBuilder keyBuilder6 = new Metaphone();
            foreach (var name in testCases)
            {
                string key = keyBuilder6.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }

            Debug.WriteLine("DoubleMetaphone");
            StringPhoneticKeyBuilder keyBuilder7 = new DoubleMetaphone();
            foreach (var name in testCases)
            {
                string key = keyBuilder7.BuildKey(name);
                Debug.WriteLine("\t{0} for {1}", key, name);
            }
        }
    }
}
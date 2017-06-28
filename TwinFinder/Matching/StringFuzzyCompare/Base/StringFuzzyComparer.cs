using System;
using System.Runtime.Serialization;
using TwinFinder.Matching.StringFuzzyCompare.AddressSpecific;
using TwinFinder.Matching.StringFuzzyCompare.Common;
using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions;
using TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Base
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(DamerauLevenshteinDistance))]
    [KnownType(typeof(ExtendedEditex))]
    [KnownType(typeof(Identity))]
    [KnownType(typeof(ExtendedJaccard))]
    [KnownType(typeof(JaroWinkler))]
    [KnownType(typeof(DiceCoefficent))]
    [KnownType(typeof(MongeElkan))]
    [KnownType(typeof(NGramDistance))]
    [KnownType(typeof(SmithWaterman))]
    [KnownType(typeof(NameComparer))]
    [KnownType(typeof(CityComparer))]
    [KnownType(typeof(CompanyComparer))]
    [KnownType(typeof(PhoneComparer))]
    [KnownType(typeof(TitleComparer))]
    public abstract class StringFuzzyComparer : IStringFuzzyComparer
    {
        // ***********************Fields***********************

        private StringCostFunction costFunction = new DefaultSubstitutionCostFunction();

        private bool caseSensitiv = false;

        // ***********************Properties***********************

        [DataMember]
        public StringCostFunction CostFunction
        {
            get
            {
                if (this.costFunction == null)
                {
                    this.costFunction = new DefaultSubstitutionCostFunction();
                }
                return this.costFunction;
            }
            set { this.costFunction = value; }
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }

        public bool CaseSensitiv
        {
            get { return this.caseSensitiv; }
            set { this.caseSensitiv = value; }
        }

        // ***********************Functions***********************

        /// <summary>
        /// Compares two strings, with a fuzzy string comparefunction
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <returns></returns>
        public abstract float Compare(string str1, string str2);

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
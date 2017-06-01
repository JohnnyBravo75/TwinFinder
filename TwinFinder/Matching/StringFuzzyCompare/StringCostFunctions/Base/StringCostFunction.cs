using System;
using System.Runtime.Serialization;

namespace TwinFinder.Matching.StringFuzzyCompare.StringCostFunctions.Base
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(DefaultSubstitutionCostFunction))]
    [KnownType(typeof(CharGroupsSubstitutionCostFunction))]
    [KnownType(typeof(SmithWatermanCostFunction))]
    public abstract class StringCostFunction : IStringCostFunction
    {
        public abstract float GetCost(string str1, int index1, string str2, int index2);

        public abstract float MaxCost { get; }

        public abstract float MinCost { get; }
    }
}
using System;
using System.Runtime.Serialization;

namespace TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(AverageAggregator))]
    [KnownType(typeof(HarmonicMeanAggregator))]
    [KnownType(typeof(MaximumAggregator))]
    [KnownType(typeof(MinimumAggregator))]
    public abstract class Aggregator : IAggregator
    {
        // ***********************Properties***********************

        public string Name
        {
            get { return this.GetType().Name; }
        }

        // ***********************Functions***********************

        /// <summary>
        /// Aggregates all single similarities with their weights together to one similarity
        /// </summary>
        /// <param name="similarities">The similarities.</param>
        /// <param name="weights">The weights.</param>
        /// <returns></returns>
        public abstract float AggregatedSimilarity(float[] similarities, float[] weights);

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
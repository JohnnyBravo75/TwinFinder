using TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Aggregators
{
    using System.Linq;

    public class MaximumAggregator : Aggregator
    {
        public override float AggregatedSimilarity(float[] similarities, float[] weights = null)
        {
            return similarities.Max();
        }
    }
}
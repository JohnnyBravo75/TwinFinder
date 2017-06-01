using TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Aggregators
{
    public class AverageAggregator : Aggregator
    {
        public override float AggregatedSimilarity(float[] similarities, float[] weights)
        {
            float weightsum = 0;
            float similaritySum = 0;

            for (int i = 0; i < similarities.Length; i++)
            {
                float weight = (weights == null) ? 1 : weights[i];
                weightsum += weight;
                similaritySum += weight * similarities[i];
            }

            if (weightsum == 0)
            {
                return 0.0f;
            }

            return similaritySum / weightsum;
        }
    }
}

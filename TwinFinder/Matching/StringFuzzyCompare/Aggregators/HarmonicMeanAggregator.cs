using TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base;

namespace TwinFinder.Matching.StringFuzzyCompare.Aggregators
{
    public class HarmonicMeanAggregator : Aggregator
    {
        public override float AggregatedSimilarity(float[] similarities, float[] weights)
        {
            float weightsum = 0;
            float invertedSimilaritySum = 0.0f;

            for (int i = 0; i < similarities.Length; i++)
            {
                float weight = (weights == null) ? 1 : weights[i];
                weightsum += weight;

                if (similarities[i] > 0.0f)
                {
                    invertedSimilaritySum += weight / similarities[i];
                }
            }

            if (invertedSimilaritySum <= 0.0f)
            {
                return 0.0f;
            }

            float harmonicMean = weightsum / invertedSimilaritySum;

            if (harmonicMean < 0.0f)
            {
                return 0.0f;
            }
            else if (harmonicMean > 1.0f)
            {
                return 1.0f;
            }

            return harmonicMean;
        }
    }
}

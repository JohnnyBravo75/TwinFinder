using System.Collections.Generic;
using TwinFinder.Matching.MatchingData.Model;

namespace TwinFinder.Matching.Cluster.Base
{
    public interface IClusterer<T>
    {
        List<MatchPair<T>> Matches {  get; set; }

        // List<List<T>> Clusters { get; }

        void Execute();
    }
}

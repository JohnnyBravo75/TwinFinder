using System;
using System.Collections.Generic;
using TwinFinder.Matching.MatchingData.Model;

namespace TwinFinder.Matching.Cluster.Base
{
    [Serializable]
    public abstract class Clusterer<T> : IClusterer<T>
    {
        // ***********************Fields***********************

        protected List<List<MatchPair<T>>> clusters = new List<List<MatchPair<T>>>();

        protected List<MatchPair<T>> matches = new List<MatchPair<T>>();

        // ***********************Properties***********************

        public List<MatchPair<T>> Matches
        {
            get { return this.matches; }
            set { this.matches = value; }
        }

        public List<List<MatchPair<T>>> Clusters
        {
            get { return this.clusters; }
        }

        // ***********************Functions***********************

        public abstract void Execute();
    }
}
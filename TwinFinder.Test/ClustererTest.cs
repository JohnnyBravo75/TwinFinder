using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwinFinder.Matching.Cluster.Base;
using TwinFinder.Matching.MatchingData.Model;

namespace TwinFinder.Test
{
    [TestClass]
    public class ClustererTest
    {
        [TestMethod]
        public void ConnectedComponentsClustererTest()
        {
            Clusterer<string> clusterAlgorithm = ClustererFactory.GetInstance<string>("ConnectedComponentsClusterer");

            clusterAlgorithm.Matches.Add(new MatchPair<string>("Frankfurt", "Wiesbaden", 40));
            clusterAlgorithm.Matches.Add(new MatchPair<string>("Frankfurt", "Mainz", 30));
            clusterAlgorithm.Matches.Add(new MatchPair<string>("Mainz", "Wiesbaden", 15));
            clusterAlgorithm.Matches.Add(new MatchPair<string>("Rüdesheim", "Geisenheim", 4));

            clusterAlgorithm.Execute();

            Assert.AreEqual(new MatchPair<string>("Frankfurt", "Wiesbaden", 40), clusterAlgorithm.Clusters[0][0]);
            Assert.AreEqual(new MatchPair<string>("Frankfurt", "Mainz", 30), clusterAlgorithm.Clusters[0][1]);
            Assert.AreEqual(new MatchPair<string>("Mainz", "Wiesbaden", 15), clusterAlgorithm.Clusters[0][2]);

            Assert.AreEqual(new MatchPair<string>("Rüdesheim", "Geisenheim", 4), clusterAlgorithm.Clusters[1][0]);
        }
    }
}
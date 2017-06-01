using TwinFinder.Base.Graph;
using TwinFinder.Base.Graph.Algorithm;
using TwinFinder.Matching.Cluster.Base;

namespace TwinFinder.Matching.Cluster
{
    using System.Collections.Generic;
    using System.Linq;
    using TwinFinder.Matching.MatchingData.Model;

    /// <summary>
    /// Builds clusters (subgraphs) of the given graph.
    /// It uses the FloydWarshall algorithm to compute the transitive closure of a graph
    /// and with which it is possible to deteremine if a way (path) between to nodes exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FloydWarshallClusterer<T> : Clusterer<T>
    {
        public override void Execute()
        {
            // build the graph
            Graph<T> graph = new Graph<T>();

            //graph.AddUndirectedEdge("Frankfurt", "Wiesbaden", 40);
            //graph.AddUndirectedEdge("Frankfurt", "Mainz", 30);
            //graph.AddUndirectedEdge("Mainz", "Wiesbaden", 15);
            //graph.AddUndirectedEdge("Rüdesheim", "Geisenheim", 4);

            foreach (var matchpair in this.matches)
            {
                graph.AddUndirectedEdge(matchpair.From, matchpair.To, matchpair.Cost);
            }

            // uses the FloyWarshall algorithm to compute the transitive closure this
            // means to determine all possible ways in a graph (so you can check, if nodes have a way to each other in the graph)
            FloydWarshall<T> transClosureAlgorithm = new FloydWarshall<T>();
            transClosureAlgorithm.Execute(graph);

            // now for all nodes in the graph e.g (Wiesbaden, Mainz, Frankfurt, Rüdesheim, Geisenheim)
            for (int i = 0; i < transClosureAlgorithm.CostMatrix.GetLength(0); i++)
            {
                var cluster = new List<MatchPair<T>>();

                T startNode = transClosureAlgorithm.Graph.Nodes[i].Value;

                // loop through all nodes in the graph e.g (Wiesbaden, Mainz, Frankfurt, Rüdesheim, Geisenheim)
                for (int j = 0; j < transClosureAlgorithm.CostMatrix.GetLength(0); j++)
                {
                    T endNode = transClosureAlgorithm.Graph.Nodes[j].Value;
                    var cost = transClosureAlgorithm.CostMatrix[i, j];

                    // only when a way in the graph exits (waycosts != Infinity)
                    // and it is not himself (startNode != endNode)
                    if (cost != transClosureAlgorithm.Graph.Infinity
                        && !startNode.Equals(endNode))
                    {
                        // a way exists
                        var matchPair = new MatchPair<T>(startNode, endNode, cost);

                        // checked if it is in the original list
                        if (this.matches.Contains(matchPair))
                        {
                            // check, if a connection point (startnode or endnode) is already in another cluster
                            var found = this.IsInOtherCluster(matchPair);

                            if (!found)
                            {
                                // add to the new cluster
                                cluster.Add(matchPair);
                            }
                        }
                    }
                }

                // when a cluster was build, add to the clusterlist
                if (cluster.Count > 0)
                {
                    this.clusters.Add(cluster);
                }

                // clusters will be e.g.
                // Cluster 1: (Frankfurt, Wiesbaden, Mainz)
                // Cluster 2: (Rüdesheim, Geisenheim)
            }
        }

        private bool IsInOtherCluster(MatchPair<T> matchPair)
        {
            // check, if a connection point (startnode or endnode) is already in another cluster
            bool found = false;
            foreach (var checkCluster in this.clusters)
            {
                if (checkCluster.Any(x => x.From.Equals(matchPair.From) || x.To.Equals(matchPair.To)))
                {
                    // when found add to this cluster
                    checkCluster.Add(matchPair);
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}
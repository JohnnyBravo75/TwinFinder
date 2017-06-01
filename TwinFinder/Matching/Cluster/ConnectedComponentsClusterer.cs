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
    /// It uses the BreadthFirstSearch algorithm to search all path in a graph from a given node.
    ///
    /// -> given Graph:
    /// Edge1 = ("Frankfurt", "Wiesbaden", 40);
    /// Edge2 = ("Frankfurt", "Mainz", 30);
    /// Edge3 = ("Mainz", "Wiesbaden", 15);
    /// Edge4 = ("Rüdesheim", "Geisenheim", 4);
    ///
    /// -> Clusters:
    /// Cluster 1: (Frankfurt, Wiesbaden, Mainz)
    /// Cluster 2: (Rüdesheim, Geisenheim)
    /// </summary>
    public class ConnectedComponentsClusterer<T> : Clusterer<T>
    {
        public override void Execute()
        {
            // build the graph
            Graph<T> graph = new Graph<T>();

            //graph.AddUndirectedEdge("Frankfurt", "Wiesbaden", 40);
            //graph.AddUndirectedEdge("Frankfurt", "Mainz", 30);
            //graph.AddUndirectedEdge("Mainz", "Wiesbaden", 15);
            //graph.AddUndirectedEdge("Rüdesheim", "Geisenheim", 4);

            foreach (MatchPair<T> matchpair in this.matches)
            {
                graph.AddUndirectedEdge(matchpair.From, matchpair.To, matchpair.Cost);
            }

            BreadthFirstSearch<T> bfsSearch = new BreadthFirstSearch<T>();

            // take every node from the graph as startnode for the search e.g. "Frankfurt", "Wiesbaden", "Mainz", "Rüdesheim", "Geisenheim"
            GraphNode<T> startNode = null;
            GraphNode<T> endNode = null;
            int cost = graph.Infinity;
            int i = 0;
            while (i < graph.Nodes.Count)
            {
                var cluster = new List<MatchPair<T>>();

                // take current node as startnode and search the whole graph for reachable nodes
                startNode = (GraphNode<T>)graph.Nodes[i];
                bfsSearch.Execute(startNode, null, null);

                // add the found nodes to the subgraph
                int j = 0;
                while (j < bfsSearch.AllNodes.Count)
                {
                    endNode = (GraphNode<T>)bfsSearch.AllNodes[j];
                    cost = startNode.CostToNeighbor(endNode);

                    if (!startNode.Equals(endNode))
                    {
                        var matchPair = new MatchPair<T>(startNode.Value, endNode.Value, cost);

                        // check, if it is in the original list
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

                    j++;
                }

                // when a cluster was build, add to the clusterlist
                if (cluster.Count > 0)
                {
                    this.clusters.Add(cluster);
                }

                // clusters will be e.g.
                // Cluster 1: (Frankfurt, Wiesbaden, Mainz)
                // Cluster 2: (Rüdesheim, Geisenheim)

                i++;
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
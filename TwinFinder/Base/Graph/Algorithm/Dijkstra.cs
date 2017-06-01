using System;
using System.Collections;
using System.Text;

namespace TwinFinder.Base.Graph.Algorithm
{
    public class Dijkstra<T>
    {
        // ***********************Fields***********************

        private static Hashtable dist = new Hashtable();
        private static Hashtable route = new Hashtable();

        private Graph<T> graph;

        // ***********************Functions***********************

        /// <summary>
        /// Initializes the distance and route tables used for Dijkstra's Algorithm.
        /// </summary>
        /// <param name="start">The <b>Key</b> to the source Node.</param>
        private void InitDistRouteTables(GraphNode<T> start)
        {
            // set the initial distance and route for each city in the pathFinding.Graph
            foreach (GraphNode<T> n in this.graph.Nodes)
            {
                dist.Add(n.Value, Int32.MaxValue);
                route.Add(n.Value, null);
            }

            // set the initial distance of start to 0
            dist[start] = 0;
        }

        /// <summary>
        /// Relaxes the edge from the Node uCity to vCity.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="end">The end node.</param>
        /// <param name="cost">The distance between uCity and vCity.</param>
        private void Relax(GraphNode<T> start, GraphNode<T> end, int cost)
        {
            int distToStart = (int)dist[start.Value];
            int distToEnd = (int)dist[end.Value];

            if (distToEnd > distToStart + cost)
            {
                // update distance and route
                dist[end.Value] = distToStart + cost;
                route[end.Value] = start;
            }
        }

        /// <summary>
        /// Retrieves the Node from the passed-in NodeList that has the <i>smallest</i> value in the distance table.
        /// </summary>
        /// <remarks>This method of grabbing the smallest Node gives Dijkstra's Algorithm a running time of
        /// O(<i>n</i><sup>2</sup>), where <i>n</i> is the number of nodes in the pathFinding.Graph.  A better approach is to
        /// use a <i>priority queue</i> data structure to store the nodes, rather than an array.  Using a priority queue
        /// will improve Dijkstra's running time to O(E lg <i>n</i>), where E is the number of edges.  This approach is
        /// preferred for sparse pathFinding.Graphs.  For more information on this, consult the README included in the download.</remarks>
        private GraphNode<T> GetMin(NodeList<T> nodes)
        {
            // find the node in nodes with the smallest distance value
            int minDist = Int32.MaxValue;
            GraphNode<T> minNode = null;
            foreach (GraphNode<T> n in nodes)
            {
                if (((int)dist[n.Value]) <= minDist)
                {
                    minDist = (int)dist[n.Value];
                    minNode = n;
                }
            }

            return minNode;
        }

        /// <summary>
        /// Dijkstra's Algorithm to find the shortest path.
        /// </summary>
        public void Execute(Graph<T> graph, GraphNode<T> start, GraphNode<T> end)
        {
            this.graph = graph;

            if (start == end)
            {
                return;
            }

            // initialize the route & distance tables
            this.InitDistRouteTables(start);

            // nodes == Q
            NodeList<T> nodes = this.graph.Nodes;

            /**** START DIJKSTRA ****/
            while (nodes.Count > 0)
            {
                // get the minimum node
                GraphNode<T> minNode = this.GetMin(nodes);

                // remove it from the set Q
                nodes.Remove(minNode);

                // iterate through all of u's neighbors
                foreach (GraphNode<T> neighborNode in minNode.Neighbors)
                    this.Relax(minNode, neighborNode, minNode.CostToNeighbor(neighborNode));		// relax each edge
            }	// repeat until Q is empty
            /**** END DIJKSTRA ****/

            // Display results
            string results = "The shortest path from " + start.Value + " to " + end.Value + " is " + dist[end.Value].ToString() + " miles and goes as follows: ";

            Stack traceBackSteps = new Stack();

            GraphNode<T> current = new GraphNode<T>(end.Value, null);

            GraphNode<T> prev = null;

            do
            {
                prev = current;
                current = (GraphNode<T>)route[current.Value];

                string temp = current.Value + " to " + prev.Value + "\r\n";
                traceBackSteps.Push(temp);
            } while (!current.Value.Equals(start.Value));

            StringBuilder sb = new StringBuilder(30 * traceBackSteps.Count);
            while (traceBackSteps.Count > 0)
                sb.Append((string)traceBackSteps.Pop());

            Console.WriteLine(results + "\r\n\r\n" + sb.ToString());

            dist.Clear();
            route.Clear();
        }

        /// <summary>
        /// Prints the graph's edges
        /// </summary>
        public void ShowEdges()
        {
            foreach (GraphNode<T> node in this.graph.Nodes)
                foreach (GraphNode<T> edge in node.Neighbors)
                    Console.WriteLine("{0} <-> {1} - {2} miles", node.Value, edge.Value, edge.CostToNeighbor(edge));
        }
    }
}
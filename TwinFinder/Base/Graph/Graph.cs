using System;
using System.Collections;
using System.Collections.Generic;

namespace TwinFinder.Base.Graph
{
    /// <summary>
    /// Represents a graph.  A graph is an arbitrary collection of GraphNode instances.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the graph's nodes.</typeparam>
    public class Graph<T> : IEnumerable<T>
    {
        // ***********************Fields***********************

        private NodeList<T> nodeSet;        // the set of nodes in the graph

        // ***********************Constructors***********************

        public Graph()
            : this(null)
        {
        }

        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        // ***********************Properties***********************

        public List<Edge<T>> Edges
        {
            get
            {
                List<Edge<T>> edges = new List<Edge<T>>();

                // enumerate through each node in the nodeSet, looking for edges
                foreach (GraphNode<T> node in this.nodeSet)
                {
                    // check each neigbor from the node
                    foreach (GraphNode<T> neighborNode in node.Neighbors)
                    {
                        int costToNeighbor = node.CostToNeighbor(neighborNode);
                        Edge<T> edge = new Edge<T>(new GraphNode<T>(node.Value), new GraphNode<T>(neighborNode.Value), costToNeighbor, Direction.Undirected);

                        if (!edges.Contains(edge))
                        {
                            edges.Add(edge);
                        }
                    }
                }

                return edges;
            }
        }

        /// <summary>
        /// Gets the infinity value for cost.
        /// This means, there is no wayfrom node A to node B because the cost are infinity.
        /// </summary>
        /// <value>
        /// The infinity value.
        /// </value>
        public int Infinity
        {
            get { return Int32.MaxValue / 3; }
        }

        /// <summary>
        /// Returns the number of nodes (vertices) in the graph.
        /// </summary>
        public int NodeCount
        {
            get { return this.nodeSet.Count; }
        }

        /// <summary>
        /// Returns the set of nodes in the graph.
        /// </summary>
        public NodeList<T> Nodes
        {
            get
            {
                return this.nodeSet;
            }
        }

        // ***********************Functions***********************

        /// <summary>
        /// Adds a directed edge from a GraphNode with one value (from) to a GraphNode with another value (to).
        /// </summary>
        /// <param name="from">The value of the GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The value of the GraphNode to which the edge leads.</param>
        public void AddDirectedEdge(T from, T to)
        {
            this.AddDirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds a directed edge from one GraphNode (from) to another (to).
        /// </summary>
        /// <param name="from">The GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The GraphNode to which the edge leads.</param>
        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            this.AddDirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds a directed edge from one GraphNode (from) to another (to) with an associated cost.
        /// </summary>
        /// <param name="from">The GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The GraphNode to which the edge leads.</param>
        /// <param name="cost">The cost of the edge from "from" to "to".</param>
        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            this.AddDirectedEdge(from.Value, to.Value, cost);
        }

        /// <summary>
        /// Adds an directed, weighted edge
        /// </summary>
        /// <param name="edge">the edge</param>
        public virtual void AddDirectedEdge(Edge<T> edge)
        {
            this.AddDirectedEdge(edge.From, edge.To, edge.Cost);
        }

        /// <summary>
        /// Adds a directed edge from a GraphNode with one value (from) to a GraphNode with another value (to)
        /// with an associated cost.
        /// </summary>
        /// <param name="from">The value of the GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The value of the GraphNode to which the edge leads.</param>
        /// <param name="cost">The cost of the edge from "from" to "to".</param>
        public void AddDirectedEdge(T from, T to, int cost)
        {
            if (this.nodeSet.FindByValue(from) == null)
            {
                this.AddNode(new GraphNode<T>(from));
            }

            if (this.nodeSet.FindByValue(to) == null)
            {
                this.AddNode(new GraphNode<T>(to));
            }

            ((GraphNode<T>)this.nodeSet.FindByValue(from)).Neighbors.Add(this.nodeSet.FindByValue(to));
            ((GraphNode<T>)this.nodeSet.FindByValue(from)).Costs.Add(cost);
            //GraphNode<T> toNode = (GraphNode<T>)nodeSet.FindByValue(to);
            //GraphNode<T> fromNode = (GraphNode<T>)nodeSet.FindByValue(from);
            //fromNode.Neighbors.Add(toNode);
            //fromNode.Costs.Add(cost);
        }

        /// <summary>
        /// Adds a new GraphNode instance to the Graph
        /// </summary>
        /// <param name="node">The GraphNode instance to add.</param>
        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            this.nodeSet.Add(node);
        }

        /// <summary>
        /// Adds a new value to the graph.
        /// </summary>
        /// <param name="value">The value to add to the graph</param>
        public void AddNode(T value)
        {
            this.nodeSet.Add(new GraphNode<T>(value));
        }

        /// <summary>
        /// Adds an undirected edge from a GraphNode with one value (from) to a GraphNode with another value (to).
        /// </summary>
        /// <param name="from">The value of one of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">The value of one of the GraphNodes that is joined by the edge.</param>
        public void AddUndirectedEdge(T from, T to)
        {
            this.AddUndirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds an undirected edge from one GraphNode to another.
        /// </summary>
        /// <param name="from">One of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">One of the GraphNodes that is joined by the edge.</param>
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            this.AddUndirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds an undirected edge from one GraphNode to another with an associated cost.
        /// </summary>
        /// <param name="from">One of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">One of the GraphNodes that is joined by the edge.</param>
        /// <param name="cost">The cost of the undirected edge.</param>
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            if (!this.nodeSet.Contains(from))
            {
                this.AddNode(from);
            }

            if (!this.nodeSet.Contains(to))
            {
                this.AddNode(to);
            }

            Node<T> fromNode = this.nodeSet.FindByValue(from.Value);
            if (fromNode != null && !fromNode.Neighbors.Contains(to))
            {
                fromNode.Neighbors.Add(to);
                fromNode.Costs.Add(cost);
            }

            Node<T> toNode = this.nodeSet.FindByValue(to.Value);
            if (toNode != null && !toNode.Neighbors.Contains(from))
            {
                toNode.Neighbors.Add(from);
                toNode.Costs.Add(cost);
            }
        }

        /// <summary>
        /// Adds an undirected, weighted edge
        /// </summary>
        /// <param name="edge">the edge</param>
        public virtual void AddUndirectedEdge(Edge<T> edge)
        {
            this.AddUndirectedEdge(edge.From, edge.To, edge.Cost);
        }

        /// <summary>
        /// Adds an undirected edge from a GraphNode with one value (from) to a GraphNode with another value (to)
        /// with an associated cost.
        /// </summary>
        /// <param name="from">The value of one of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">The value of one of the GraphNodes that is joined by the edge.</param>
        /// <param name="cost">The cost of the undirected edge.</param>
        public void AddUndirectedEdge(T from, T to, int cost)
        {
            if (this.nodeSet.FindByValue(from) == null)
            {
                this.AddNode(from);
            }

            if (this.nodeSet.FindByValue(to) == null)
            {
                this.AddNode(to);
            }

            ((GraphNode<T>)this.nodeSet.FindByValue(from)).Neighbors.Add(this.nodeSet.FindByValue(to));
            ((GraphNode<T>)this.nodeSet.FindByValue(from)).Costs.Add(cost);

            ((GraphNode<T>)this.nodeSet.FindByValue(to)).Neighbors.Add(this.nodeSet.FindByValue(from));
            ((GraphNode<T>)this.nodeSet.FindByValue(to)).Costs.Add(cost);
        }

        /// <summary>
        /// Clears out the contents of the Graph.
        /// </summary>
        public void Clear()
        {
            this.nodeSet.Clear();
        }

        /// <summary>
        /// Returns a Boolean, indicating if a particular value exists within the graph.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if the value exist in the graph; false otherwise.</returns>
        public bool Contains(T value)
        {
            return this.nodeSet.FindByValue(value) != null;
        }

        /// <summary>
        /// Finds the edge with the maximum cost
        /// </summary>
        /// <returns>the edge with the maximum cost</returns>
        public Edge<T> FindMaxEdge()
        {
            Edge<T> maxEdge = null;

            // enumerate through each node in the nodeSet, looking for edge with the max cost
            foreach (GraphNode<T> node in this.nodeSet)
            {
                // check each neigbor from the node
                foreach (GraphNode<T> neighborNode in node.Neighbors)
                {
                    int costToNeighbor = node.CostToNeighbor(neighborNode);
                    if (maxEdge == null || costToNeighbor > maxEdge.Cost)
                    {
                        maxEdge = new Edge<T>(node, neighborNode, costToNeighbor, Direction.Undirected);
                    }
                }
            }

            return maxEdge;
        }

        /// <summary>
        /// Finds the edge with the minumum cost
        /// </summary>
        /// <returns>the edge with the minimum cost</returns>
        public Edge<T> FindMinEdge()
        {
            Edge<T> minEdge = null;

            // enumerate through each node in the nodeSet, looking for edge with the min cost
            foreach (GraphNode<T> node in this.nodeSet)
            {
                // check each neigbor from the node
                foreach (GraphNode<T> neighborNode in node.Neighbors)
                {
                    int costToNeighbor = node.CostToNeighbor(neighborNode);
                    if (minEdge == null || costToNeighbor < minEdge.Cost)
                    {
                        minEdge = new Edge<T>(node, neighborNode, costToNeighbor, Direction.Undirected);
                    }
                }
            }

            return minEdge;
        }

        public int[,] GetCostMatrix()
        {
            int[,] costMatrix = new int[this.NodeCount, this.NodeCount];

            for (int i = 0; i < this.NodeCount; i++)
            {
                for (int j = 0; j < this.NodeCount; j++)
                {
                    if (i == j)
                        costMatrix[i, j] = 0;
                    else
                        costMatrix[i, j] = this.Infinity;
                }
            }

            // Input data into the matrix, where matrix[i][j] is the cost/distance from i to j.
            for (int i = 0; i < this.NodeCount; i++)
            {
                for (int j = 0; j < this.NodeCount; j++)
                {
                    var from = (GraphNode<T>)this.Nodes[i];
                    var to = (GraphNode<T>)this.Nodes[j];

                    if (from.IsNeighbor(to))
                    {
                        int cost = from.CostToNeighbor(to);
                        costMatrix[i, j] = (cost == 0 ? 1 : cost);
                    }
                }
            }

            return costMatrix;
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (GraphNode<T> gnode in this.nodeSet)
                yield return gnode.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Attempts to remove a node from a graph.
        /// </summary>
        /// <param name="value">The value that is to be removed from the graph.</param>
        /// <returns>True if the corresponding node was found, and removed; false if the value was not
        /// present in the graph.</returns>
        /// <remarks>This method removes the GraphNode instance, and all edges leading to or from the
        /// GraphNode.</remarks>
        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)this.nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            return this.Remove(nodeToRemove);
        }

        /// <summary>
        /// Removes a node from the graph
        /// </summary>
        /// <param name="nodeToRemove">the node to remove</param>
        /// <returns></returns>
        public bool Remove(GraphNode<T> nodeToRemove)
        {
            if (!this.nodeSet.Contains(nodeToRemove))
            {
                return false;
            }

            // otherwise, the node was found
            this.nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> node in this.nodeSet)
            {
                int index = node.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    node.Neighbors.RemoveAt(index);
                    node.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public bool Remove(NodeList<T> nodes)
        {
            bool result = true;
            foreach (var node in nodes)
            {
                if (!this.Remove((GraphNode<T>)node))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Removes a edge from the graph
        /// </summary>
        /// <param name="edge">the edge to remove</param>
        /// <returns></returns>
        public bool RemoveEdge(Edge<T> edge)
        {
            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> node in this.nodeSet)
            {
                if (node.Equals(edge.From))
                {
                    int index = node.Neighbors.IndexOf(edge.To);
                    if (index != -1)
                    {
                        // remove the reference to the node and associated cost
                        node.Neighbors.RemoveAt(index);
                        node.Costs.RemoveAt(index);
                    }
                }

                if (node.Equals(edge.To))
                {
                    int index = node.Neighbors.IndexOf(edge.From);
                    if (index != -1)
                    {
                        // remove the reference to the node and associated cost
                        node.Neighbors.RemoveAt(index);
                        node.Costs.RemoveAt(index);
                    }
                }
            }

            return true;
        }
    }
}
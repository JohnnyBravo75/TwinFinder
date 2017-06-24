using System.Collections.Generic;

namespace TwinFinder.Base.Graph
{
    /// <summary>
    /// The Node&lt;T&gt; class represents the base concept of a Node for a tree or graph.  It contains
    /// a data item of type T, and a list of neighbors.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the Node.</typeparam>
    /// <remarks>None of the classes in the SkmDataStructures2 namespace use the Node class directly;
    /// they all derive from this class, adding necessary functionality specific to each data structure.</remarks>
    public class Node<T>
    {
        // ***********************Fields***********************

        private List<int> costs;
        private T data;
        private NodeList<T> neighbors = null;

        // ***********************Constructors***********************

        public Node()
        {
        }

        public Node(T value)
            : this(value, null)
        {
        }

        public Node(T value, NodeList<T> neighbors)
        {
            this.data = value;
            this.neighbors = neighbors;
        }

        // ***********************Properties***********************

        /// <summary>
        /// Returns the set of costs for the edges eminating from this graph node.
        /// The k<sup>th</sup> cost (Cost[k]) represents the cost from the graph node to the node
        /// represented by its k<sup>th</sup> neighbor (Neighbors[k]).
        /// </summary>
        /// <value></value>
        public List<int> Costs
        {
            get
            {
                if (this.costs == null)
                {
                    this.costs = new List<int>();
                }

                return this.costs;
            }
        }

        public bool HasNeighbors
        {
            get
            {
                if (this.Neighbors == null)
                {
                    return false;
                }

                if (this.Neighbors.Count == 0)
                {
                    return false;
                }

                return true;
            }
        }

        public NodeList<T> Neighbors
        {
            get
            {
                if (this.neighbors == null)
                {
                    this.neighbors = new NodeList<T>();
                }

                return this.neighbors;
            }

            set
            {
                this.neighbors = value;
            }
        }

        public T Value
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        // ***********************Functions***********************

        public int CostToNeighbor(GraphNode<T> node)
        {
            int index = this.Neighbors.IndexOf(node);

            if (index >= 0)
            {
                return this.Costs[index];
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = (obj as Node<T>);

            if (other == null)
            {
                return false;
            }

            if (this.Value.Equals(other.Value))
            {
                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public bool IsNeighbor(Node<T> node)
        {
            if (this.Neighbors.IndexOf(node) == -1)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (this.Value == null)
            {
                return string.Empty;
            }

            return this.Value.ToString();
        }
    }
}
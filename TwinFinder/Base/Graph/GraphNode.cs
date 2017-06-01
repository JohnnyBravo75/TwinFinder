namespace TwinFinder.Base.Graph
{
    /// <summary>
    /// Represents a node in a graph.  A graph node contains some piece of data, along with a set of
    /// neighbors.  There can be an optional cost between a graph node and each of its neighbors.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the graph node.</typeparam>
    public class GraphNode<T> : Node<T>
    {
        //private List<int> costs;        // the cost associated with each edge

        // ***********************Constructors***********************

        public GraphNode()
            : base()
        {
        }

        public GraphNode(T value)
            : base(value)
        {
        }

        public GraphNode(T value, NodeList<T> neighbors)
            : base(value, neighbors)
        {
        }

        // ***********************Properties***********************

        ///// <summary>
        ///// Returns the set of neighbors for this graph node.
        ///// </summary>
        //new public NodeList<T> Neighbors
        //{
        //    get
        //    {
        //        if (base.Neighbors == null)
        //            base.Neighbors = new NodeList<T>();

        //        return base.Neighbors;
        //    }
        //}

        ///// <summary>
        ///// Returns the set of costs for the edges eminating from this graph node.
        ///// The k<sup>th</sup> cost (Cost[k]) represents the cost from the graph node to the node
        ///// represented by its k<sup>th</sup> neighbor (Neighbors[k]).
        ///// </summary>
        ///// <value></value>
        //public List<int> Costs
        //{
        //    get
        //    {
        //        if (costs == null)
        //            costs = new List<int>();

        //        return costs;
        //    }
        //}

        //public int CostToNeighbor(GraphNode<T> node)
        //{
        //    int index = base.Neighbors.IndexOf(node);

        //    if (index >= 0)
        //    {
        //        return Costs[index];
        //    }

        //    return 0;
        //}

        public override string ToString()
        {
            if (this.Value == null)
            {
                return base.ToString();
            }

            return this.Value.ToString();
        }
    }
}
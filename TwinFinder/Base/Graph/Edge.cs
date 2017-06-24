namespace TwinFinder.Base.Graph
{
    public enum Direction { Directed, Undirected };

    public class Edge<T>
    {
        // ***********************Constructors***********************

        public Edge(GraphNode<T> from, GraphNode<T> to, int cost, Direction direction)
        {
            this.Add(from, to, cost, direction);
        }

        public Edge(T from, T to, int cost, Direction direction)
        {
            this.Add(new GraphNode<T>(from), new GraphNode<T>(to), cost, direction);
        }

        protected Edge()
        {
        }

        // ***********************Properties***********************

        public int Cost { get; set; }

        public GraphNode<T> From { get; set; }

        public GraphNode<T> To { get; set; }

        // ***********************Functions***********************

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = (obj as Edge<T>);

            if (other == null)
            {
                return false;
            }

            if ((this.From.Equals(other.From))
             && (this.To.Equals(other.To)))
            {
                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.From.GetHashCode() + this.To.GetHashCode();
        }

        public override string ToString()
        {
            return (this.From != null ? this.From.ToString() : "") + " -> " + (this.To != null ? this.To.ToString() : "");
        }

        protected void Add(GraphNode<T> from, GraphNode<T> to, int cost, Direction direction)
        {
            this.From = from;
            this.To = to;
            this.Cost = cost;

            if (!this.From.Neighbors.Contains(to))
            {
                this.From.Neighbors.Add(to);
                this.From.Costs.Add(cost);
            }

            if (direction == Direction.Undirected)
            {
                if (!this.To.Neighbors.Contains(from))
                {
                    this.To.Neighbors.Add(from);
                    this.To.Costs.Add(cost);
                }
            }
        }
    }
}
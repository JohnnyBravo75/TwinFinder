using System.Collections.ObjectModel;

namespace TwinFinder.Base.Graph
{
    public class NodeList<T> : Collection<Node<T>>
    {
        // ***********************Constructors***********************

        public NodeList()
            : base()
        {
        }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        // ***********************Functions***********************

        public void Add(GraphNode<T> item)
        {
            this.Items.Add(item);
        }

        public NodeList<T> Clone()
        {
            NodeList<T> newList = new NodeList<T>();
            foreach (var node in this.Items)
            {
                newList.Add((GraphNode<T>)node);
            }

            return newList;
        }

        /// <summary>
        /// Searches the NodeList for a Node containing a particular value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The Node in the NodeList, if it exists; null otherwise.</returns>
        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (GraphNode<T> node in this.Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }
}
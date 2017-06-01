using System.Collections.ObjectModel;

namespace TwinFinder.Base.Graph
{
    public class EdgeList<T> : Collection<Edge<T>>
    {
        // ***********************Constructors***********************

        public EdgeList()
            : base()
        {
        }

        public EdgeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Edge<T>));
        }

        // ***********************Functions***********************

        public EdgeList<T> Clone()
        {
            EdgeList<T> newList = new EdgeList<T>();
            foreach (var edge in this.Items)
            {
                newList.Add(edge);
            }

            return newList;
        }
    }
}
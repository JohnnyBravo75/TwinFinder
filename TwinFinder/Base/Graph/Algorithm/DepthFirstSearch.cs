using System;
using System.Collections.Generic;

namespace TwinFinder.Base.Graph.Algorithm
{
    /// <summary>
    /// Tiefensuche (depth-first search, DFS)
    /// The DFS searches a graph from a start node by visiting every other reachble node (and stops at a given node)
    ///
    /// Zuerst wird ein Startknoten ausgewählt.
    /// Von diesem Knoten aus wird nun die erste Kante  betrachtet und getestet, ob der gegenüberliegende Knoten
    /// schon entdeckt wurde bzw. das gesuchte Element ist. Ist dies noch nicht der Fall, so wird rekursiv für
    /// diesen Knoten die Tiefensuche aufgerufen, wodurch wieder der erste Nachfolger dieses Knotens expandiert wird.
    /// Diese Art der Suche wird solange fortgesetzt, bis das gesuchte Element entweder gefunden wurde oder die Suche
    /// bei einer Senke im Graphen angekommen ist und somit keine weiteren Nachfolgeknoten mehr untersuchen kann.
    /// An dieser Stelle kehrt der Algorithmus nun zum zuletzt expandierten Knoten  zurück und untersucht den nächsten
    /// Nachfolger des Knotens. Sollte es hier keine weiteren Nachfolger mehr geben, geht der Algorithmus wieder Schritt
    /// für Schritt zum jeweiligen Vorgänger zurück und versucht es dort erneut.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DepthFirstSearch<T>
    {
        // ***********************Fields***********************

        private Dictionary<GraphNode<T>, bool> visited = new Dictionary<GraphNode<T>, bool>();
        private Dictionary<GraphNode<T>, GraphNode<T>> predecessors = new Dictionary<GraphNode<T>, GraphNode<T>>();

        private NodeList<T> allNodes = new NodeList<T>();

        private EdgeList<T> allEdges = new EdgeList<T>();

        // ***********************Properties***********************

        public NodeList<T> AllNodes
        {
            get { return this.allNodes; }
        }

        public EdgeList<T> AllEdges
        {
            get { return this.allEdges; }
        }

        // ***********************Functions***********************

        public NodeList<T> Execute(GraphNode<T> start, GraphNode<T> end, Action<GraphNode<T>> action)
        {
            this.allNodes.Clear();
            this.allEdges.Clear();
            this.visited.Clear();
            this.predecessors.Clear();

            this._Execute(start, end, action);

            return this.allNodes;
        }

        private void _Execute(GraphNode<T> start, GraphNode<T> end, Action<GraphNode<T>> action)
        {
            Stack<GraphNode<T>> worklist = new Stack<GraphNode<T>>();

            this.visited.Add(start, false);

            worklist.Push(start);

            while (worklist.Count != 0)
            {
                GraphNode<T> node = worklist.Pop();
                this.allNodes.Add(node);

                if (action != null)
                {
                    action(node);
                }

                // Check if node is the end
                if (end != null && node == end)
                {
                    break;
                }
                else
                {
                    foreach (GraphNode<T> neighbor in node.Neighbors)
                    {
                        // When neighbor wasn´t visited, visit him
                        if (!this.visited.ContainsKey(neighbor))
                        {
                            // mark as visited
                            this.visited.Add(neighbor, false);

                            // add as predecessor (for getting the way and a path back)
                            this.predecessors.Add(neighbor, node);
                            this.allEdges.Add(new Edge<T>(node, neighbor, node.CostToNeighbor(neighbor), Direction.Undirected));

                            // remember in the worklist
                            worklist.Push(neighbor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prints the walked path in the grpah
        /// </summary>
        /// <param name="node">The node.</param>
        public void PrintPath(GraphNode<T> node)
        {
            while (this.predecessors[node] != null)
            {
                Console.WriteLine(node);
                node = this.predecessors[node];
            }
        }
    }
}
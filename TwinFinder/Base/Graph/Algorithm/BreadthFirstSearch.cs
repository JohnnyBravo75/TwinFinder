using System;
using System.Collections.Generic;

namespace TwinFinder.Base.Graph.Algorithm
{
    /// <summary>
    /// Breitensuche (breadth-first search, BFS)
    /// The BFS searches a graph from a start node by visiting every other reachble node (and stops at a given node)
    ///
    /// The BFS begins at a root node and inspect all the neighboring nodes.
    /// Then for each of those neighbor nodes in turn, it inspects their neighbor nodes which were unvisited, and so on...
    ///
    /// Zuerst wird ein Startknoten  ausgewählt. Von diesem Knoten aus wird nun jede Kante betrachtet und getestet,
    /// ob der gegenüberliegende Knoten schon entdeckt wurde bzw. das gesuchte Element ist.
    /// Ist dies noch nicht der Fall, so wird der entsprechende Knoten in einer Warteschlange FIFO-Queue(i.e., First In, First Out)
    /// gespeichert und im nächsten Schritt bearbeitet. Hierbei ist zu beachten,
    /// dass Breitensuche immer zuerst alle direkt nachfolgenden Knoten bearbeitet,
    /// und nicht wie die Tiefensuche (DFS) einem Pfad in die Tiefe folgt.
    /// Nachdem alle Kanten des Ausgangsknotens betrachtet wurden,
    /// wird der erste Knoten der Warteschlange entnommen und das Verfahren wiederholt.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BreadthFirstSearch<T>
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

        public NodeList<T> Execute(GraphNode<T> start, GraphNode<T> end, Action<GraphNode<T>> action)
        {
            this.allNodes.Clear();
            this.allEdges.Clear();
            this.visited.Clear();
            this.predecessors.Clear();

            this._Execute(start, end, action);

            return this.allNodes;
        }

        // ***********************Functions***********************

        private void _Execute(GraphNode<T> start, GraphNode<T> end, Action<GraphNode<T>> action)
        {
            Queue<GraphNode<T>> worklist = new Queue<GraphNode<T>>();

            this.visited.Add(start, false);

            worklist.Enqueue(start);

            while (worklist.Count != 0)
            {
                GraphNode<T> node = worklist.Dequeue();
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
                            worklist.Enqueue(neighbor);
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
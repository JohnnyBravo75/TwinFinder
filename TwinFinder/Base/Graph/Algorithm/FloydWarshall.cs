using System;

namespace TwinFinder.Base.Graph.Algorithm
{
    /// <summary>
    /// the FloyWarshall finds all-pairs shortest path
    /// it computes the transitive closure of a graph ,this
    //  means you can determine all possible ways in a graph (so you can check, if nodes have a way to each other in the graph)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FloydWarshall<T>
    {
        private int[,] costMatrix;
        private Graph<T> graph;

        public Graph<T> Graph
        {
            get { return this.graph; }
        }

        /// <summary>
        /// The adjencency / cost / distance matrix of the graph.
        /// In the matrix there is the distance from startnode[i] to endnode[j] (which means how much it costs to drive from i to j)
        /// </summary>
        public int[,] CostMatrix
        {
            get { return this.costMatrix; }
        }

        public void Execute(Graph<T> graph)
        {
            this.graph = graph;

            this.costMatrix = graph.GetCostMatrix();

            // Calculate the Transitive closure matrix of the Graph
            for (int k = 0; k < this.costMatrix.GetLength(0); k++)
            {
                for (int i = 0; i < this.costMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < this.costMatrix.GetLength(0); j++)
                    {
                        // Warshall (transitive closure)
                        // costMatrix[i, j] = costMatrix[i, j] | costMatrix[i, k] & costMatrix[k, j];

                        // Floyd (all shortest paths)
                        this.costMatrix[i, j] = Math.Min(this.costMatrix[i, j], this.costMatrix[i, k] + this.costMatrix[k, j]);
                    }
                }
            }

            // Now the shortest distance between i and j will be in costMatrix[i][j].
        }

        /// <summary>
        /// Prints the paths.
        /// </summary>
        public void PrintPaths()
        {
            // Now the shortest distance between i and j will be in costMatrix[i][j].
            for (int i = 0; i < this.costMatrix.GetLength(0); i++)
            {
                Console.WriteLine("From " + this.graph.Nodes[i] + ":");

                for (int j = 0; j < this.costMatrix.GetLength(0); j++)
                {
                    string cost = (this.costMatrix[i, j] == this.graph.Infinity) ? "Infinity" : this.costMatrix[i, j].ToString();
                    Console.WriteLine("Shortest Path Cost to " + this.graph.Nodes[j] + " is: " + cost);
                }
            }
        }
    }
}
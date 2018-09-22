using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Graph
{
    /// <summary>
    /// A Graph is a set of vertices and a collection of edges that each connect a pair of vertices. An directed graph is a graph where all edges are unidirectional.
    /// </summary>
    public class EdgeWeightedDirectedGraph
    {
        /// <summary>
        /// the graph's set of edges.
        /// </summary>
        private WeightedGraphEdge[] edges;
        /// <summary>
        /// the graph's set of vertices.
        /// </summary>
        private GraphNode[] vertices;
        /// <summary>
        /// Creates a new instance of <see cref="EdgeWeightedDirectedGraph"/> from an existing source.
        /// </summary>
        /// <param name="g">the graph being cloned</param>
        public EdgeWeightedDirectedGraph(EdgeWeightedDirectedGraph g)
        {
            vertices = new GraphNode[g.vertices.Length];
            Array.Copy(g.vertices, vertices, g.vertices.Length);
            edges = new WeightedGraphEdge[g.edges.Length];
            Array.Copy(g.edges, edges, g.edges.Length);
        }
        /// <summary>
        /// Creates a new instance of <see cref="EdgeWeightedDirectedGraph"/> with a specific number of vertices and no edges.
        /// </summary>
        /// <param name="numVertices">the number of vertices</param>
        public EdgeWeightedDirectedGraph(int numVertices)
        {
            vertices = new GraphNode[numVertices];
            for (int i = numVertices - 1; i >= 0; i--)
            {
                vertices[i] = new GraphNode(i);
            }
            edges = new WeightedGraphEdge[0];
        }
        public EdgeWeightedDirectedGraph(EdgeWeightedUndirectedGraph g)
        {
            vertices = new GraphNode[g.NumberOfVertices];
            for (int i = 0, len = g.NumberOfVertices; i < len; i++)
            {
                vertices[i] = new GraphNode(i);
            }
            edges = new WeightedGraphEdge[0];
        }
        /// <summary>
        /// Adds edge v-w to this graph with default cost of 1.
        /// </summary>
        /// <param name="v">vertex v's id</param>
        /// <param name="w">vertex w's id</param>
        public void AddEdge(int v, int w)
        {
            AddEdge(v, w, 1);
        }
        /// <summary>
        /// Adds edge v-w to this graph.
        /// </summary>
        /// <param name="v">vertex v's id</param>
        /// <param name="w">vertex w's id</param>
        /// <param name="c">the edge's cost</param>
        public void AddEdge(int v, int w, float c)
        {
            if (!HasEdge(v, w))
            {
                if (!HasVertex(v))
                {
                    AddVertex(v);
                }
                if (!HasVertex(w))
                {
                    AddVertex(w);
                }
                edges = ArrayUtilities.Instance.ExtendArray(new WeightedGraphEdge(v, w, c), edges);
            }
        }
        /// <summary>
        /// Adds edge v-w to this graph.
        /// </summary>
        /// <param name="e"><see cref="WeightedGraphEdge"/> v-w</param>
        public void AddEdge(WeightedGraphEdge e)
        {
            if (!HasEdge(e))
            {
                if (!HasVertex(e.From))
                {
                    AddVertex(e.From);
                }
                if (!HasVertex(e.To))
                {
                    AddVertex(e.To);
                }
                edges = ArrayUtilities.Instance.ExtendArray(
                        new WeightedGraphEdge(e), edges);
            }
        }
        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="v">the vertex instance</param>
        public void AddVertex(GraphNode v)
        {
            int i = EmptyVertexSlot;
            if (i == -1)
            {
                i = vertices.Length;
                ExtendVerticesArray();
            }
            vertices[i] = v;
        }
        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="v">the vertex id</param>
        public void AddVertex(int v)
        {
            int i = EmptyVertexSlot;
            if (i == -1)
            {
                i = vertices.Length;
                ExtendVerticesArray();
            }
            vertices[i] = new GraphNode(v);
        }
        /// <summary>
        /// Extends the length of the set of vertices by 1.
        /// </summary>
        private void ExtendVerticesArray()
        {
            this.ExtendVerticesArray(1);
        }
        /// <summary>
        /// Extends the length of the set of vertices by a specific length.
        /// </summary>
        /// <param name="length">the length by which the set of vertices is being extended</param>
        private void ExtendVerticesArray(int length)
        {
            GraphNode[] dest = new GraphNode[vertices.Length + length];
            Array.Copy(vertices, dest, vertices.Length);
            vertices = dest;
            dest = null;
        }
        /// <summary>
        /// Gets the set of all vertices adjacent to vertex v.
        /// </summary>
        /// <param name="v">vertex v</param>
        /// <returns><see cref="int"/>[]</returns>
        public int[] GetAdjacencies(int v)
        {
            int[] adjacencies = new int[0];
            for (int i = edges.Length - 1; i >= 0; i--)
            {
                if (edges[i].From == v)
                {
                    adjacencies = ArrayUtilities.Instance.ExtendArray(
                            edges[i].To, adjacencies);
                }
            }
            return adjacencies;
        }
        /// <summary>
        /// Gets the edges.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WeightedGraphEdge GetEdge(int index)
        {
            return edges[index];
        }
        /// <summary>
        /// The location of an empty index in the set of vertices.
        /// </summary>
        private int EmptyVertexSlot
        {
            get
            {
                int index = -1;
                for (int i = 0, len = vertices.Length; i < len; i++)
                {
                    if (vertices[i] == null)
                    {
                        index = i;
                        break;
                    }
                    else if (vertices[i].Index == -1)
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
        }
        /// <summary>
        /// the graph's number of edges.
        /// </summary>
        public int NumberOfEdges
        {
            get
            {
                int numEdges = 0;
                for (int i = edges.Length - 1; i >= 0; i--)
                {
                    numEdges++;
                }
                return numEdges;
            }
        }
        /// <summary>
        /// the graph's number of vertices.
        /// </summary>
        public int NumberOfVertices
        {
            get
            {
                int numVertices = 0;
                for (int i = vertices.Length - 1; i >= 0; i--)
                {
                    if (vertices[i].Index > -1)
                    {
                        numVertices++;
                    }
                }
                return numVertices;
            }
        }
        /// <summary>
        /// Gets a vertex by its id.
        /// </summary>
        /// <param name="id">the vertex' id</param>
        /// <returns><see cref="GraphNode"/></returns>
        public GraphNode GetVertex(int id)
        {
            GraphNode v = null;
            for (int i = vertices.Length - 1; i >= 0; i--)
            {
                if (vertices[i].Index == id)
                {
                    v = vertices[i];
                    break;
                }
            }
            return v;
        }
        /// <summary>
        /// Gets the directed edges incident from vertex <tt>v</tt>.
        /// </summary>
        /// <param name="v">vertex v</param>
        /// <returns><see cref="WeightedGraphEdge"/>[]</returns>
        public WeightedGraphEdge[] GetVertexAdjacencies(int v)
        {
            WeightedGraphEdge[] adj = new WeightedGraphEdge[0];
            for (int i = edges.Length - 1; i >= 0; i--)
            {
                if (edges[i].From == v)
                {
                    adj = ArrayUtilities.Instance.ExtendArray(edges[i], adj);
                }
            }
            return adj;
        }
        /// <summary>
        /// Determines if edge v-w exists on the graph.
        /// </summary>
        /// <param name="v">vertex v's id</param>
        /// <param name="w">vertex w's id</param>
        /// <returns>true if edge v-w exists; false otherwise</returns>
        private bool HasEdge(int v, int w)
        {
            bool exists = false;
            for (int i = edges.Length - 1; i >= 0; i--)
            {
                if (edges[i].EqualsDirected(v, w))
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
        /// <summary>
        /// Determines if edge v-w exists on the graph.
        /// </summary>
        /// <param name="e">the edge instance</param>
        /// <returns>true if edge v-w exists; false otherwise</returns>
        private bool HasEdge(WeightedGraphEdge e)
        {
            bool exists = false;
            for (int i = edges.Length - 1; i >= 0; i--)
            {
                if (edges[i].EqualsDirected(e))
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
        /// <summary>
        /// Determines if vertex v exists in the graph.
        /// </summary>
        /// <param name="vertexId">the id of vertex v</param>
        /// <returns>true if vertex v exists; false otherwise</returns>
        private bool HasVertex(int vertexId)
        {
            bool exists = false;
            for (int i = vertices.Length - 1; i >= 0; i--)
            {
                if (vertices[i].Index == vertexId)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
        /// <summary>
        /// Removes edge v-w from the set of edges.
        /// </summary>
        /// <param name="v">vertex v's id</param>
        /// <param name="w">vertex w's id</param>
        /// <returns>true if the edge was removed; false otherwise</returns>
        public bool RemoveEdge(int v, int w)
        {
            bool removed = false;
            if (HasEdge(v, w))
            {
                int i;
                for (i = edges.Length - 1; i >= 0; i--)
                {
                    if (edges[i].EqualsDirected(v, w))
                    {
                        break;
                    }
                }
                removed = true;
                edges = ArrayUtilities.Instance.RemoveIndex(i, edges);
            }
            return removed;
        }
    }
}

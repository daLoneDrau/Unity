﻿using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RPGBase.Graph
{
    public sealed class DijkstraUndirectedSearch
    {
        /// <summary>
        /// distTo[v] = distance of shortest s->v path
        /// </summary>
        private double[] distTo;
        /// <summary>
        /// edgeTo[v] = last edge on shortest s->v path
        /// </summary>
        private WeightedGraphEdge[] edgeTo;
        /// <summary>
        /// priority queue of vertices
        /// </summary>
        private IndexMinPQ<Double> pq;
        /// <summary>
        /// Computes a shortest paths tree from <tt>s</tt> to every other vertex in the edge-weighted digraph<tt>G</tt>.
        /// </summary>
        /// <param name="graph">the edge-weighted digraph</param>
        /// <param name="source">the source vertex</param>
        public DijkstraUndirectedSearch(EdgeWeightedUndirectedGraph graph, int source)
        {
            for (int i = graph.NumberOfEdges - 1; i >= 0; i--)
            {
                if (graph.GetEdge(i).Cost < 0)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "edge " + graph.GetEdge(i) + " has negative weight");
                }
            }
            distTo = new double[graph.NumberOfVertices];
            edgeTo = new WeightedGraphEdge[graph.NumberOfVertices];
            for (int v = 0; v < graph.NumberOfVertices; v++)
            {
                distTo[v] = Double.PositiveInfinity;
            }
            distTo[source] = 0.0;

            // relax vertices in order of distance from s
            pq = new IndexMinPQ<Double>(graph.NumberOfVertices);
            pq.Insert(source, distTo[source]);
            while (!pq.IsEmpty)
            {
                int v = pq.DelMin();
                WeightedGraphEdge[] adj = graph.GetVertexAdjacencies(v);
                for (int i = adj.Length - 1; i >= 0; i--)
                {
                    Relax(adj[i], v);
                }
            }

            // check optimality conditions
            Debug.Assert(Check(graph, source));
        }
        // check optimality conditions:
        // (i) for all edges e: distTo[e.to()] <= distTo[e.from()] + e.weight()
        // (ii) for all edge e on the SPT: distTo[e.to()] == distTo[e.from()] +
        // e.weight()
        private bool Check(EdgeWeightedUndirectedGraph graph, int s)
        {
            // check that edge weights are nonnegative
            for (int i = graph.NumberOfEdges - 1; i >= 0; i--)
            {
                WeightedGraphEdge e = graph.GetEdge(i);
                if (e.Cost < 0)
                {
                    Console.WriteLine("negative edge weight detected");
                    return false;
                }
            }
            // check that distTo[v] and edgeTo[v] are consistent
            if (distTo[s] != 0.0 || edgeTo[s] != null)
            {
                Console.WriteLine("distTo[s] and edgeTo[s] inconsistent");
                return false;
            }
            for (int v = graph.NumberOfVertices - 1; v >= 0; v--)
            {
                if (v == s)
                {
                    continue;
                }
                if (edgeTo[v] == null && distTo[v] != Double.PositiveInfinity)
                {
                    Console.WriteLine("distTo[] and edgeTo[] inconsistent");
                    return false;
                }
            }

            // check that all edges e = v->w satisfy distTo[w] <= distTo[v] +
            // e.weight()
            for (int v = graph.NumberOfVertices - 1; v >= 0; v--)
            {
                WeightedGraphEdge[] adj = graph.GetVertexAdjacencies(v);
                for (int i = adj.Length - 1; i >= 0; i--)
                {
                    WeightedGraphEdge e = adj[i];
                    int w;
                    if (v == e.To)
                    {
                        w = e.From;
                    }
                    else
                    {
                        w = e.To;
                    }
                    if (distTo[v] + e.Cost < distTo[w])
                    {
                        Console.WriteLine("edge " + e + " not relaxed");
                        return false;
                    }
                }
            }

            // check that all edges e = v->w on SPT satisfy distTo[w] == distTo[v] +
            // e.weight()
            for (int w = 0; w < graph.NumberOfVertices; w++)
            {
                if (edgeTo[w] == null)
                {
                    continue;
                }
                WeightedGraphEdge e = edgeTo[w];
                int v = e.From;
                if (w != e.To)
                {
                    return false;
                }
                if (distTo[v] + e.Cost != distTo[w])
                {
                    Console.WriteLine("edge " + e + " on shortest path not tight");
                    return false;
                }
            }
            return true;
        }
        /**
         * Returns the Length of a shortest path from the source vertex <tt>s</tt>
         * to vertex <tt>v</tt>.
         * @param v the destination vertex
         * @return the Length of a shortest path from the source vertex <tt>s</tt>
         *         to vertex <tt>v</tt>; <tt>Double.POSITIVE_INFINITY</tt> if no
         *         such path
         */
        public double DistanceTo(int v)
        {
            return distTo[v];
        }
        /**
         * Is there a path from the source vertex <tt>s</tt> to vertex <tt>v</tt>?
         * @param v the destination vertex
         * @return <tt>true</tt> if there is a path from the source vertex
         *         <tt>s</tt> to vertex <tt>v</tt>, and <tt>false</tt> otherwise
         */
        public bool HasPathTo(int v)
        {
            return distTo[v] < Double.PositiveInfinity;
        }
        /**
         * Returns a shortest path from the source vertex <tt>s</tt> to vertex
         * <tt>v</tt>.
         * @param v the destination vertex
         * @return a shortest path from the source vertex <tt>s</tt> to vertex
         *         <tt>v</tt> as an iterable of edges, and <tt>null</tt> if no such
         *         path
         */
        public WeightedGraphEdge[] pathTo(int v)
        {
            WeightedGraphEdge[] path = null;
            if (HasPathTo(v))
            {
                path = new WeightedGraphEdge[0];
                WeightedGraphEdge e = edgeTo[v];
                int lastVertex = v;
                for (; e != null;)
                {
                    path = ArrayUtilities.Instance.ExtendArray(e, path);
                    if (e.From == lastVertex)
                    {
                        lastVertex = e.To;
                        e = edgeTo[e.To];
                    }
                    else
                    {
                        lastVertex = e.From;
                        e = edgeTo[e.From];
                    }
                }
            }
            return path;
        }
        /**
         * Relax the edge and update the priority queue if needed.
         * @param edge the edge
         * @param source the source vertex where the edge leads from
         */
        private void Relax(WeightedGraphEdge edge, int source)
        {
            int v, w;
            if (source == edge.From)
            {
                v = edge.From;
                w = edge.To;
            }
            else
            {
                v = edge.To;
                w = edge.From;
            }
            if (distTo[w] > distTo[v] + edge.Cost)
            {
                distTo[w] = distTo[v] + edge.Cost;
                edgeTo[w] = edge;
                if (pq.Contains(w))
                {
                    pq.DecreaseKey(w, distTo[w]);
                }
                else
                {
                    pq.Insert(w, distTo[w]);
                }
            }
        }
    }
}

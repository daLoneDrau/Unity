using Assets.Scripts.RPGBase.Graph;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    public class RiverDijkstraSearch
    {
        private double[] distTo;          // distTo[v] = distance of
                                          // shortest s->v path
        private WeightedGraphEdge[] edgeTo;    // edgeTo[v] = last edge on
                                               // shortest s->v path
        private IndexMinPQ<double> pq;    // priority queue of vertices
                                          /**
                                           * Computes a shortest paths tree from <tt>s</tt> to every other vertex in
                                           * the edge-weighted digraph <tt>G</tt>.
                                           * @param graph the edge-weighted digraph
                                           * @param source the source vertex
                                           * @throws IllegalArgumentException if an edge weight is negative
                                           * @throws IllegalArgumentException unless 0 &le; <tt>s</tt> &le; 
                                           * <tt>V</tt> - 1
                                           */
        public RiverDijkstraSearch(EdgeWeightedDirectedGraph graph, int source)
        {
            if (!HexMap.Instance.HasRiverNode(source))
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "not a valid river node");
            }
            if (HexMap.Instance.GetRiverNodeById(source).Type != RiverGraphNode.RIVER_BANK)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "cannot start river path from river itself; must be a river bank");
            }
            for (int i = graph.NumberOfEdges - 1; i >= 0; i--)
            {
                WeightedGraphEdge e = graph.GetEdge(i);
                if (e.Cost < 0)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "edge " + e + " has negative weight");
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
        private bool Check(EdgeWeightedDirectedGraph graph,                 int s)
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
         * Returns the length of a shortest path from the source vertex <tt>s</tt>
         * to vertex <tt>v</tt>.
         * @param v the destination vertex
         * @return the length of a shortest path from the source vertex <tt>s</tt>
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
                for (WeightedGraphEdge e = edgeTo[v]; e != null;
                        e = edgeTo[e.From])
                {
                    path = ArrayUtilities.Instance.ExtendArray(e, path);
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

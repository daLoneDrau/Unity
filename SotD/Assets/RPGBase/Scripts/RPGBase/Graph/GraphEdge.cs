using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RPGBase.Graph
{
    /// <summary>
    /// An edge is a pair of vertices, ordered or unordered.
    /// </summary>
    public class GraphEdge
    {
        /// <summary>
        /// the index of the 1st of the <see cref="GraphNode"/>s this edge connects.
        /// </summary>
        public int From { get; private set; }
        /// <summary>
        /// the index of the 2nd of the <see cref="GraphNode"/>s this edge connects.
        /// </summary>
        public int To { get; private set; }
        /// <summary>
        /// Creates a new instance of <see cref="GraphEdge"/> from an existing source.
        /// </summary>
        /// <param name="edge">the edge being cloned</param>
        public GraphEdge(GraphEdge edge)
        {
            From = edge.From;
            To = edge.To;
        }
        /// <summary>
        /// reates a new instance of <see cref="GraphEdge"/>.
        /// </summary>
        /// <param name="f">the index of the 1st <see cref="GraphNode"/></param>
        /// <param name="t">the index of the 1st <see cref="GraphNode"/></param>
        public GraphEdge(int f, int t)
        {
            From = f;
            To = t;
        }
        /// <summary>
        /// Determines if this <see cref="GraphEdge"/> connects two nodes exactly in the direction provided.
        /// </summary>
        /// <param name="e">the second <see cref="GraphEdge"/></param>
        /// <returns>true if the <see cref="GraphEdge"/> connects two nodes in either direction; false otherwise</returns>
        public bool EqualsDirected(GraphEdge e)
        {
            return From == e.From && To == e.To;
        }
        /// <summary>
        /// Determines if this <see cref="GraphEdge"/> connects two nodes exactly in the direction provided.
        /// </summary>
        /// <param name="f">the node the edge is coming from</param>
        /// <param name="t">the node the edge is going to</param>
        /// <returns>true if the <see cref="GraphEdge"/> connects two nodes exactly in the direction provided; false otherwise</returns>
        public bool EqualsDirected(int f, int t)
        {
            return From == f && To == t;
        }
        /// <summary>
        /// Determines if this <see cref="GraphEdge"/> is the same as a second <see cref="GraphEdge"/>.
        /// </summary>
        /// <param name="e">the second <see cref="GraphEdge"/></param>
        /// <returns>true if the <see cref="GraphEdge"/> connects two nodes in either direction; false otherwise</returns>
        public bool EqualsUndirected(GraphEdge e)
        {
            return (From == e.From && To == e.To) || (From == e.To && To == e.From);
        }
        /// <summary>
        /// Determines if this <see cref="GraphEdge"/> connects two nodes in either direction.
        /// </summary>
        /// <param name="e0">the first node</param>
        /// <param name="e1">the second node</param>
        /// <returns>true if the <see cref="GraphEdge"/> connects two nodes in either direction; false otherwise</returns>
        public bool EqualsUndirected(int e0, int e1)
        {
            return (From == e0 && To == e1) || (From == e1 && To == e0);
        }
        public override string ToString()
        {
            return "[from=" + From + ",to=" + To + "]";
        }
    }
}

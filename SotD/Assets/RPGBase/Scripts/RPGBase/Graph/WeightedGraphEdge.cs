using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RPGBase.Graph
{
    public class WeightedGraphEdge : GraphEdge
    {
        /// <summary>
        /// the cost of traversing the <see cref="GraphEdge"/>.
        /// </summary>
        public double Cost { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="WeightedGraphEdge"/>.
        /// </summary>
        /// <param name="f">the index of the 1st <see cref="GraphNode"/></param>
        /// <param name="t">the index of the 2nd <see cref="GraphNode"/></param>
        /// <param name="c">the cost of traversing the <see cref="WeightedGraphEdge"/></param>
        public WeightedGraphEdge(int f, int t, double c) : base(f, t)
        {
            Cost = c;
        }
        /// <summary>
        /// Creates a new instance of <see cref="WeightedGraphEdge"/>.
        /// </summary>
        /// <param name="edge">the edge being cloned</param>
        public WeightedGraphEdge(WeightedGraphEdge edge) : base(edge)
        {
            Cost = edge.Cost;
        }
    }
}

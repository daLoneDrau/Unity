using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Graph
{
    /// <summary>
    /// A vertex (plural vertices) or node is the fundamental unit of which graphs are formed.
    /// </summary>
    public class GraphNode
    {
        /// <summary>
        /// the <see cref="GraphNode"/>'s Index. must be >= 0.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// the <see cref="GraphNode"/>'s Name. can be null.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="GraphNode"/>.
        /// </summary>
        public GraphNode()
        {
            Index = -1;
        }
        /// <summary>
        /// Creates a new instance of <see cref="GraphNode"/>.
        /// </summary>
        /// <param name="newName">the <see cref="GraphNode"/>'s Name</param>
        /// <param name="i">the <see cref="GraphNode"/>'s Index. must be >= 0</param>
        public GraphNode(string newName, int i)
        {
            if (i < 0)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Index must be greater than or equal to 0");
            }
            Name = newName;
            Index = i;
        }
        /// <summary>
        /// Creates a new instance of <see cref="GraphNode"/> from an existing source.
        /// </summary>
        /// <param name="vertex">the source vertex</param>
        public GraphNode(GraphNode vertex)
        {
            Index = vertex.Index;
            Name = vertex.Name;
        }
        /// <summary>
        /// Creates a new instance of <see cref="GraphNode"/>.
        /// </summary>
        /// <param name="ind">the Index to set</param>
        public GraphNode(int ind)
        {
            if (ind < 0)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Index must be greater than or equal to 0");
            }
            Index = ind;
        }
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (this == obj)
            {
                equals = true;
            }
            else if (obj != null)
            {
                if (obj is GraphNode)
                {
                    if (((GraphNode)obj).Index == Index)
                    {
                        equals = true;
                    }
                }
            }
            return equals;
        }
    }
}

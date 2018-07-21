using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.RPGBase.Graph
{
    public class PhysicalGraphNode : GraphNode
    {
        public Vector2 Location { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="PhysicalGraphNode"/>.
        /// </summary>
        /// <param name="ind">the cell's index</param>
        /// <param name="x">the cell's x-coordinates</param>
        /// <param name="y">the cell's y-coordinates</param>
        public PhysicalGraphNode(int ind, int x, int y) : base(ind)
        {
            Location = new Vector2(x, y);
        }
        /// <summary>
        /// Creates a new instance of <see cref="PhysicalGraphNode"/>.
        /// </summary>
        /// <param name="name">the <see cref="PhysicalGraphNode"/>'s name</param>
        /// <param name="ind">the cell's index</param>
        /// <param name="x">the cell's x-coordinates</param>
        /// <param name="y">the cell's y-coordinates</param>
        public PhysicalGraphNode(string name, int ind, int x, int y) : base(name, ind)
        {
            Location = new Vector2(x, y);
        }
        /// <summary>
        /// Creates a new instance of <see cref="PhysicalGraphNode"/>.
        /// </summary>
        /// <param name="ind">the cell's index</param>
        /// <param name="v">the cell's coordinates</param>
        public PhysicalGraphNode(int ind, Vector2 v) : base(ind)
        {
            Location = v;
        }
        /// <summary>
        /// Creates a new instance of <see cref="PhysicalGraphNode"/>.
        /// </summary>
        /// <param name="name">the <see cref="PhysicalGraphNode"/>'s name</param>
        /// <param name="ind">the cell's index</param>
        /// <param name="v">the cell's coordinates</param>
        public PhysicalGraphNode(String name, int ind, Vector2 v) : base(name, ind)
        {
            Location = v;
        }
        /// <summary>
        /// Determines if this <see cref="PhysicalGraphNode"/> equals a specific set of coordinates.
        /// </summary>
        /// <param name="x">the x-coordinates</param>
        /// <param name="y">the y-coordinates</param>
        /// <returns><tt>true</tt> if the <see cref="PhysicalGraphNode"/> equals the coordinates; <tt>false</tt> otherwise</returns>
        public bool Equals(float x, float y)
        {
            return Location.x == x && Location.y == y;
        }
        /// <summary>
        /// Determines if this <see cref="PhysicalGraphNode"/> equals a specific set of coordinates.
        /// </summary>
        /// <param name="v">the coordinates</param>
        /// <returns><tt>true</tt> if the <see cref="PhysicalGraphNode"/> equals the coordinates; <tt>false</tt> otherwise</returns>
        public bool Equals(Vector2 v)
        {
            return Location.Equals(v);
        }
    }
}

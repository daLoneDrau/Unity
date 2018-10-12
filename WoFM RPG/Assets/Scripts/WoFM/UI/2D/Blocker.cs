using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoFM.UI._2D
{
    public class Blocker : MonoBehaviour
    {
        /// <summary>
        /// does not block
        /// </summary>
        public const int VOID = 0;
        /// <summary>
        /// blocks as a wall
        /// </summary>
        public const int WALL = 1;
        /// <summary>
        /// blocks as a door
        /// </summary>
        public const int DOOR = 2;
        public int Type { get; set; }
    }
}

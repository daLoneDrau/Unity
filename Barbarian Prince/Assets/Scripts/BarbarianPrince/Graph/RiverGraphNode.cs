using Assets.Scripts.RPGBase.Graph;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    public class RiverGraphNode : GraphNode
    {
        /// <summary>
        /// node representing a hex location.
        /// </summary>
        public const int RIVER_BANK = 0;
        /// <summary>
        /// node representing a river side.
        /// </summary>
        public const int RIVER_RAFT = 1;
        /// <summary>
        /// an integer representing one of the hex nodes to which the river connects.
        /// </summary>
        public int Bank0 { get; set; }
        /// <summary>
        /// an integer representing one of the hex nodes to which the river connects.
        /// </summary>
        public int Bank1 { get; set; }
        /// <summary>
        /// the id of the hex node this node represents.
        /// </summary>
        private int hexId;
        /// <summary>
        /// the type of node.
        /// </summary>
        public int Type { get; set; }
        public int HexId { get { return hexId; } }
        public void SetHexId(int val)
        {
            hexId = val;
        }
        public void SetHexId(Vector2 val)
        {
            hexId = HexMap.Instance.GetHex(val).Index;
        }
        public override string ToString()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("[index = ");
            sb.Append(Index);
            switch (Type)
            {
                case RIVER_BANK:
                    sb.Append(", type = RIVER_BANK, name = \"");
                    sb.Append(Name);
                    sb.Append("\", hex = ");
                    sb.Append(hexId);
                    break;
                default:
                    sb.Append(", type = RIVER_RAFT, name = \"");
                    sb.Append(Name);
                    sb.Append("\", bank0 = ");
                    sb.Append(Bank0);
                    sb.Append(", bank1 = ");
                    sb.Append(Bank1);
                    break;
            }
            sb.Append("]");
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
    }
}

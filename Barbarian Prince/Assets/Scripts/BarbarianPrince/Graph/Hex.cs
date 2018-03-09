using Assets.Scripts.BarbarianPrince.Constants;
using Assets.Scripts.RPGBase.Graph;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    public class Hex : PhysicalGraphNode
    {
        /// <summary>
        /// the <see cref="Hex"/>'s features.
        /// </summary>
        private long features;
        /// <summary>
        /// the internal <see cref="Hexagon"/> instance representing this location.
        /// </summary>
        public Hexagon Hexagon { get; set; }
        /// <summary>
        /// <see cref="Hex"/>'s type.
        /// </summary>
        public HexType Type { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="Hex"/>.
        /// </summary>
        public Hex() : base("", 0, 0, 0) { }
        /// <summary>
        /// Adds a feature.
        /// </summary>
        /// <param name="feature">the feature</param>
        public void AddFeature(HexFeature feature)
        {
            features |= feature.Flag;
        }
        /// <summary>
        /// Determines if a specific action is allowed.
        /// </summary>
        /// <param name="action">the action id</param>
        /// <returns><tt>true</tt> if the action is allowed; <tt>false</tt> otherwise</returns>
        public bool CanPerformAction(int action)
        {
            long actions = PossibleActions;
            return (actions & action) == action;
        }
        /// <summary>
        /// Clears all features that were set.
        /// </summary>
        public void ClearFeatures()
        {
            features = 0;
        }
        /// <summary>
        /// the location's name.
        /// </summary>
        public string LocationName
        {
            get
            {
                string name = Type.Title;
                if (Name != null
                        && Name.Length > 0)
                {
                    name = Name;
                }
                return name;
            }
        }

        /// <summary>
        /// all possible actions allowed in this hex.
        /// </summary>
        public long PossibleActions
        {
            get
            {
                long actions = 0;
                actions |= BPGlobals.ACTION_REST;
                actions |= BPGlobals.ACTION_LAND_TRAVEL;
                if (HexMap.Instance.HasRiverNode(Index))
                {
                    actions |= BPGlobals.ACTION_RIVER_TRAVEL;
                }
                // TODO Auto-generated method stub
                // must check for airborne

                // TODO Auto-generated method stub
                // must check for caches

                if (HasFeature(HexFeature.TOWN)
                        || HasFeature(HexFeature.CASTLE)
                        || HasFeature(HexFeature.TEMPLE))
                {
                    actions |= BPGlobals.ACTION_NEWS;
                }

                if (HasFeature(HexFeature.TOWN)
                        || HasFeature(HexFeature.CASTLE))
                {
                    actions |= BPGlobals.ACTION_HIRE;
                }

                if (HasFeature(HexFeature.TOWN)
                        || HasFeature(HexFeature.CASTLE)
                        || HasFeature(HexFeature.TEMPLE))
                {
                    actions |= BPGlobals.ACTION_AUDIENCE;
                }

                if (HasFeature(HexFeature.TEMPLE))
                {
                    actions |= BPGlobals.ACTION_OFFERING;
                }

                if (HasFeature(HexFeature.RUINS))
                {
                    actions |= BPGlobals.ACTION_SEARCH_RUINS;
                }
                return actions;
            }
        }
        /// <summary>
        /// Determines if the <see cref="Hex"/> has a specific feature.
        /// </summary>
        /// <param name="feature">the feature</param>
        /// <returns>true if the <see cref="Hex"/> has the feature; false otherwise</returns>
        public bool HasFeature(HexFeature feature)
        {
            return (features & feature.Flag) == feature.Flag;
        }
        /// <summary>
        /// Determines if the <see cref="Hex"/> has any features.
        /// </summary>
        /// <returns>true if the <see cref="Hex"/> has any feature; false otherwise</returns>
        public bool HasFeatures()
        {
            return features != 0;
        }
        /// <summary>
        /// Removes a feature.
        /// </summary>
        /// <param name="feature">the feature</param>
        public void RemoveFeature(HexFeature feature)
        {
            features &= ~feature.Flag;
        }
        /// <summary>
        /// Gets a formatted string for this hex.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public String ToMsgString()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(LocationName);
            sb.Append(" (");
            sb.Append((int)Location.x);
            sb.Append(", ");
            sb.Append((int)Location.y);
            sb.Append(')');
            string s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
        public override string ToString()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("[index = ");
            sb.Append(Index);
            sb.Append(", location = ");
            sb.Append(Location);
            sb.Append(", name = \"");
            sb.Append(Name);
            sb.Append("\", type = ");
            sb.Append(Type);
            sb.Append(", features = ");
            sb.Append(features);
            sb.Append("]");
            string s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
    }
}

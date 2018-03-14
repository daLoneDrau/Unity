using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    /// <summary>
    /// The types of features present in a <see cref="Hex"/>.
    /// </summary>
    public class HexFeature
    {
        public static HexFeature ValueOf(string name)
        {
            HexFeature t = null;
            if (string.Equals(name, "TEMPLE", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.TEMPLE;
            }
            else if (string.Equals(name, "RUINS", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.RUINS;
            }
            else if (string.Equals(name, "CASTLE", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.CASTLE;
            }
            else if (string.Equals(name, "TOWN", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.TOWN;
            }
            else if (string.Equals(name, "RIVER", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.RIVER;
            }
            else if (string.Equals(name, "OASIS", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.OASIS;
            }
            else if (string.Equals(name, "CACHE", StringComparison.OrdinalIgnoreCase))
            {
                t = HexFeature.CACHE;
            }
            return t;
        }
        /// <summary>
        /// Temple.
        /// </summary>
        public static readonly HexFeature TEMPLE = new HexFeature(1, "Temple");
        /// <summary>
        /// Ruins.
        /// </summary>
        public static readonly HexFeature RUINS = new HexFeature(2, "Ruins");
        /// <summary>
        /// Castle.
        /// </summary>
        public static readonly HexFeature CASTLE = new HexFeature(4, "Castle");
        /// <summary>
        /// Town.
        /// </summary>
        public static readonly HexFeature TOWN = new HexFeature(8, "Town");
        /// <summary>
        /// River.
        /// </summary>
        public static readonly HexFeature RIVER = new HexFeature(16, "River");
        /// <summary>
        /// Oasis.
        /// </summary>
        public static readonly HexFeature OASIS = new HexFeature(32, "Oasis");
        /// <summary>
        /// Cache.
        /// </summary>
        public static readonly HexFeature CACHE = new HexFeature(64, "Cache");
        /// <summary>
        /// the type.
        /// </summary>
        public int Flag { get; private set; }
        /// <summary>
        /// the title.
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <param name="title">the title</param>
        private HexFeature(int flag, string title)
        {
            Flag = flag;
            Title = title;
        }
    }
}

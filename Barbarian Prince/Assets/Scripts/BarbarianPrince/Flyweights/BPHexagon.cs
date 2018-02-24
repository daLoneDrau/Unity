using Assets.Scripts.RPGBase.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Flyweights
{
    public class BPHexagon : Hexagon
    {
        /// <summary>
        /// the next available reference id.
        /// </summary>
        private static int nextId = 0;
        /// <summary>
        /// The types of terrain the hexagon represents.
        /// </summary>
        public enum HexType { Countryside, Desert, Mountain };
        /// <summary>
        /// The types of features present in the hex.
        /// </summary>
        public enum FeatureType { None, Town, Mountain, Forest };
        /// <summary>
        /// The Hex's feature.
        /// </summary>
        public FeatureType Feature { get; set; }
        public HexType Type { get; private set; }
        /// <summary>
        /// the hex' name.
        /// </summary>
        public string Name { get; set; }
        public BPHexagon(HexType type) : base(true, nextId++)
        {
            Type = type;
            Feature = FeatureType.None;
        }
    }
}

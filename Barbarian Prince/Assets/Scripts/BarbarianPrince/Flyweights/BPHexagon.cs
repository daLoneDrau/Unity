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
        public enum HexType { Country };
        public HexType Type { get; private set; }
        public BPHexagon(HexType type) : base(true, nextId++)
        {
            Type = type;
        }
    }
}

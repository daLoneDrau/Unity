using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    /// <summary>
    /// The types of terrain a <see cref="Hex"/> represents.
    /// </summary>
    public class HexType
    {
        public static HexType ValueOf(string name)
        {
            HexType t = null;
            if (string.Equals(name, "COUNTRY", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.COUNTRY;
            }
            else if (string.Equals(name, "FARM", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.FARM;
            }
            else if (string.Equals(name, "FOREST", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.FOREST;
            }
            else if (string.Equals(name, "HILL", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.HILL;
            }
            else if (string.Equals(name, "MOUNTAIN", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.MOUNTAIN;
            }
            else if (string.Equals(name, "DESERT", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.DESERT;
            }
            else if (string.Equals(name, "SWAMP", StringComparison.OrdinalIgnoreCase))
            {
                t = HexType.SWAMP;
            }
            return t;
        }
        /// <summary>
        /// Countryside.
        /// </summary>
        public static readonly HexType COUNTRY = new HexType(0, "Countryside");
        public const int COUNTRY_VAL = 0;
        /// <summary>
        /// Farmland.
        /// </summary>
        public static readonly HexType FARM = new HexType(1, "Farmland");
        public const int FARM_VAL = 1;
        /// <summary>
        /// Forest.
        /// </summary>
        public static readonly HexType FOREST = new HexType(2, "Forest");
        public const int FOREST_VAL = 2;
        /// <summary>
        /// Badlands.
        /// </summary>
        public static readonly HexType HILL = new HexType(3, "Badlands");
        public const int HILL_VAL = 3;
        /// <summary>
        /// Mountains.
        /// </summary>
        public static readonly HexType MOUNTAIN = new HexType(4, "Mountains");
        public const int MOUNTAIN_VAL = 4;
        /// <summary>
        /// Desert.
        /// </summary>
        public static readonly HexType DESERT = new HexType(5, "Desert");
        public const int DESERT_VAL = 5;
        /// <summary>
        /// Swamp.
        /// </summary>
        public static readonly HexType SWAMP = new HexType(6, "Swamp");
        public const int SWAMP_VAL = 6;
        /// <summary>
        /// the type.
        /// </summary>
        public int Type { get; private set; }
        /// <summary>
        /// the title.
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="title">the title</param>
        private HexType(int type, string title)
        {
            Type = type;
            Title = title;
        }
    }
}

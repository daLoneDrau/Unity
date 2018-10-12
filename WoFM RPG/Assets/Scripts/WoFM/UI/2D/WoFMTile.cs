using RPGBase.Scripts.UI._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI._2D
{
    public class WoFMTile : Tile
    {
        /// <summary>
        /// each tile is 16 pixels wide.
        /// </summary>
        public const int TILE_SIZE = 16;
        /// <summary>
        /// The id of the room the tile is associated with.
        /// </summary>
        public int[] Rooms { get; set; }
        /// <summary>
        /// the level of shadow that covers the tile, based on lighting.
        /// </summary>
        public int ShadeLevel { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="WoFMTile"/>.
        /// </summary>
        /// <param name="w">the <see cref="TileWorld"/> the <see cref="Tile"/> inhabits</param>
        /// <param name="id">the id of the rooms the tile is in</param>
        public WoFMTile(TileWorld w, int[] rooms) : base(w)
        {
            Rooms = rooms;
        }
        /// <summary>
        /// Determines if a tile blocks light.
        /// </summary>
        /// <returns>true if the tile blocks light, false otherwise</returns>
        public bool IsLightBlocker()
        {
            bool blocker = false;
            switch (Type)
            {
                case TerrainType.wall_0:
                case TerrainType.wall_1:
                case TerrainType.Void:
                    blocker = true;
                    break;
                default: break;
            }
            return blocker;
        }
    }
}

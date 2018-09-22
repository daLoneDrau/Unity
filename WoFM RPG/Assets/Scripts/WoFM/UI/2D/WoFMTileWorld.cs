using RPGBase.Scripts.UI._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoFM.UI._2D
{
    /// <summary>
    /// Warlock of Firetop Mountain implementation of the <see cref="TileWorld"/>.  Uses the <see cref="WoFMTile"/> definition.
    /// </summary>
    public class WoFMTileWorld : TileWorld
    {
        /// <summary>
        /// Creates a new instance of <see cref="WoFMTileWorld"/>.
        /// </summary>
        /// <param name="w">the <see cref="WoFMTileWorld"/>'s width</param>
        /// <param name="h">the <see cref="WoFMTileWorld"/>'s height</param>
        public WoFMTileWorld(int w = 100, int h = 100)
        {
            // CODE BELOW USED SUCCESSFULLY FOR WOFM TILE WORLD
            // WOFM HAS BUILT-IN BUFFER OF EMPTY TILES TO COMPENSATE FOR UI BOUNDARY
            Width = w;
            Height = h;
            // generate a new list of empty tiles to fill the world.
            tiles = new WoFMTile[Width, Height];
            for (int x = Width - 1; x >= 0; x--)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    tiles[x, y] = new WoFMTile(this, 0);
                }
            }
        }
        /// <summary>
        /// Resets the shadow level of each tile, readying them for lighting.
        /// </summary>
        public void ResetShadows()
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    ((WoFMTile)tiles[x, y]).ShadeLevel = ShadowcastFOV.UNSCANNED;
                }
            }
        }
    }
}

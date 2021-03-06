﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGBase.Scripts.UI._2D
{
    public class TileWorld
    {
        /// <summary>
        /// the list of <see cref="Tile"/>s in the world.
        /// </summary>
        protected Tile[,] tiles;
        /// <summary>
        /// The <see cref="TileWorld"/>'s width.
        /// </summary>
        public int Width { get; protected set; }
        /// <summary>
        /// The <see cref="TileWorld"/>'s height.
        /// </summary>
        public int Height { get; protected set; }
        /// <summary>
        /// Creates a new instance of <see cref="TileWorld"/>.
        /// </summary>
        /// <param name="w">the <see cref="TileWorld"/>'s width</param>
        /// <param name="h">the <see cref="TileWorld"/>'s height</param>
        public TileWorld(int w = 100, int h = 100)
        {
            // CODE BELOW USED SUCCESSFULLY FOR WOFM TILE WORLD
            // WOFM HAS BUILT-IN BUFFER OF EMPTY TILES TO COMPENSATE FOR UI BOUNDARY
            Width = w;
            Height = h;
            /* USED THIS CODE FOR HEX WORLD - NOT SURE WHY HAVE BUFFER OF 2 AND 3
            Width = w + 1;
            Width++;
            Height = h + 1;
            Height += 2;
            */
            // generate a new list of empty tiles to fill the world.
            tiles = new Tile[Width, Height];
            for (int x = Width - 1; x >= 0; x--)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    tiles[x, y] = new Tile(this);
                }
            }
        }
        public Tile GetTileAtWorldCoordinates(Vector3 pos)
        {
            Tile t = null;
            for (int x = Width - 1; x >= 0; x--)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    Rect r = new Rect((float)x - 0.5f, (float)y - 0.5f, 1, 1);
                    if (r.Contains(pos))
                    {
                        t = tiles[x, y];
                        break;
                    }
                }
            }
            return t;
        }
        public Vector3 GetTileCoordinatesForWorldCoordinates(Vector3 pos)
        {
            Vector3 v = new Vector3(0, 0, 0);
            for (int x = Width - 1; x >= 0; x--)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    Rect r = new Rect((float)x - 0.5f, (float)y - 0.5f, 1, 1);
                    if (r.Contains(pos))
                    {
                        v.Set(x, y, 0);
                        break;
                    }
                }
            }
            return v;
        }
        /// <summary>
        /// Gets the tile at a specific location.  If no tile exists, null is returned.
        /// </summary>
        /// <param name="x">the x-coordinate</param>
        /// <param name="y">the y-coordinate</param>
        /// <returns><see cref="Tile"/></returns>
        public Tile GetTileAt(int x, int y)
        {
            Tile t = null;
            if (x >= 0
                && x < Width
                && y >= 0
                && y < Height)
            {
                if (tiles[x, y] == null)
                {
                    tiles[x, y] = new Tile(this);
                }
                t = tiles[x, y];
            }
            return t;
        }
        public Tile GetTileAt(Vector2 v)
        {
            return GetTileAt((int)v.x, (int)v.y);
        }
    }
}

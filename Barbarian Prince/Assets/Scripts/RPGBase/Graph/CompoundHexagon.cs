using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.RPGBase.Graph
{
    public class CompoundHexagon : Hexagon
    {
        /** the list of tiles that make up the hex. */
        private Hexagon[] hexes = new Hexagon[0];
        /** the number of rotations applied to the {@link Hexagon}. */
        private int rotations;
        /**
         * Creates a new instance of {@link CompoundHexagon}.
         */
        private CompoundHexagon(bool isFlat, int refId) : base(isFlat, refId) { }
        /**
         * Creates a new instance of {@link CompoundHexagon}.
         * @param refId the hexagon's reference id
         */
        protected CompoundHexagon(int refId) : base(false, refId) { }
        /**
         * Adds a hexagon.
         * @param hexagon the hexagon
         */
        public void AddHex(Hexagon hexagon)
        {
            if (hexagon != null)
            {
                hexes = ArrayUtilities.Instance.ExtendArray(hexagon, hexes);
            }
        }
        public void CopyOf(CompoundHexagon hex)
        {
            ((Hexagon)this).CopyOf(hex);
            rotations = hex.rotations;
            hexes = new Hexagon[hex.hexes.Length];
            for (int i = 0, len = hexes.Length; i < len; i++)
            {
                Hexagon h = new Hexagon(
                        hex.hexes[i].IsFlat(), hex.hexes[i].Id,
                        hex.hexes[i].GetSize());
                h.CopyOf(hex.hexes[i]);
                hexes[i] = h;
                h = null;
            }
        }
        /**
         * Gets the center hex for this ascii hex.
         * @return {@link Hexagon}
         */
        public Hexagon GetCenterHexagon()
        {
            int maxx = 0, minx = 0, maxy = 0, miny = 0, maxz = 0, minz = 0;
            for (int i = hexes.Length - 1; i >= 0; i--)
            {
                maxx = Math.Max(maxx, hexes[i].X);
                minx = Math.Min(minx, hexes[i].X);
                maxy = Math.Max(maxy, hexes[i].Y);
                miny = Math.Min(miny, hexes[i].Y);
                maxz = Math.Max(maxz, hexes[i].Z);
                minz = Math.Min(minz, hexes[i].Z);
            }
            return GetHexagon((maxx + minx) / 2, (maxy + miny) / 2,
                    (maxz + minz) / 2);
        }
        /**
         * Gets the {@link Hexagon} found at a specific index.
         * @param index the index
         * @return {@link Hexagon}
         */
        public Hexagon GetHexagon(int index)
        {
            return hexes[index];
        }
        /**
         * Gets a hexagon at a specific set of coordinates.
         * @param x the x-coordinate
         * @param y the y-coordinate
         * @param z the z-coordinate
         * @return {@link Hexagon}
         */
        public Hexagon GetHexagon(int x, int y, int z)
        {
            Hexagon hex = null;
            for (int i = hexes.Length - 1; i >= 0; i--)
            {
                if (hexes[i].Equals(x, y, z))
                {
                    hex = hexes[i];
                }
            }
            return hex;
        }
        /**
         * Gets a hexagon at a specific set of coordinates.
         * @param v3 the set of coordinates
         * @return {@link Hexagon}
         */
        public Hexagon GetHexagon(Vector3 v3)
        {
            return GetHexagon((int)v3.x, (int)v3.y, (int)v3.z);
        }
        /**
         * gets the number of hexes that make up this ascii hex.
         * @return <code>int</code>
         */
        public int GetNumberOfHexes()
        {
            return hexes.Length;
        }
        /**
         * Gets the number of rotations applied to the hex tile.
         * @return <code>int</code>
         */
        public int GetRotations()
        {
            return rotations;
        }
        /**
         * Rotates the hex tile.
         * @throws RPGException if an error occurs
         */
        public override void Rotate()
        {
            for (int i = hexes.Length - 1; i >= 0; i--)
            {
                hexes[i].Rotate();
            }
            rotations++;
            if (rotations > 5)
            {
                rotations = 0;
            }
        }
    }
}

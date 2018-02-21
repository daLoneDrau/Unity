using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.RPGBase.Graph
{
    public class Hexagon
    {
        /** hexagons have 6 corners; each corner is shared by two other hexagons. */
        private int[][] corners;
        /** hexagons have 6 edges; each edge is shared by another hexagon. */
        private int[] edges;
        /** the hexagon's orientation; flat or pointed on top. */
        private bool flat;
        /** the hexagon's height. */
        private float height;
        /** the horizontal distance between adjacent hexes. */
        private float horizontalDistance;
        /** each hexagon has a unique id. */
        private int id;
        /** constants. */
        private int sixty = 60, oneEighty = 180;
        /** the distance between a hexagon's center point and a corner. */
        private float size;
        /** constants. */
        private int three = 3, four = 4, six = 6, thirty = 30;
        /** the vertical distance between adjacent hexes. */
        private float verticalDistance;
        /** the hexagon's width. */
        private float width;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        /** cube coordinates. */
        //private int X, Y, Z;
        /**
         * Creates a new instance of {@link Hexagon}.
         * @param isFlat flag indicating whether the hexagon is flat on top or 
         * pointy
         * @param refId the hexagon's reference id
         */
        public Hexagon(bool isFlat, int refId) : this(isFlat, refId, 0) { }
        /**
         * Creates a new instance of {@link Hexagon}.
         * @param isFlat flag indicating whether the hexagon is flat on top or
         * pointy
         * @param refId the hexagon's reference id
         * @param newSize the hexagon's new size
         */
        public Hexagon(bool isFlat, int refId, float newSize)
        {
            id = refId;
            edges = new int[] { -1, -1, -1, -1, -1, -1 };
            corners =
                    new int[][] { new int[]{ -1, -1 }, new int[]{ -1, -1 }, new int[]{ -1, -1 }, new int[]{ -1, -1 },
                        new int[]{ -1, -1 }, new int[]{ -1, -1 } };
            flat = isFlat;
            if (newSize > 0)
            {
                SetSize(newSize);
            }
        }
        /**
         * Creates a new instance of {@link Hexagon}.
         * @param refId the hexagon's reference id
         */
        public Hexagon(int refId) : this(true, refId, 0) { }
        /**
         * Creates a new instance of {@link Hexagon}.
         * @param refId the hexagon's reference id
         * @param newSize the hexagon's new size
         */
        public Hexagon(int refId, float newSize) : this(true, refId, newSize) { }
        /**
         * Makes this {@link Hexagon} a copy of a specific {@link Hexagon}.
         * @param hex the {@link Hexagon} being copied
         */
        public void CopyOf(Hexagon hex)
        {
            size = hex.size;
            X = hex.X;
            Y = hex.Y;
            Z = hex.Z;
        }
        /**
         * Determines if this {@link Hexagon} is equal to the supplied coordinates.
         * @param x1 the X-coordinate
         * @param y1 the Y-coordinate
         * @param z1 the Z-coordinate
         * @return <tt>true</tt> if the coordinates match this instance;
         *         <tt>false</tt> otherwise
         */
        public bool Equals(int x1, int y1, int z1)
        {
            bool equals = false;
            if (x1 == X && y1 == Y && z1 == Z)
            {
                equals = true;
            }
            return equals;
        }
        /*
         * (non-Javadoc)
         * @see java.lang.Object#equals(java.lang.Object)
         */
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is Hexagon
                    && ((Hexagon)obj).X == X
                    && ((Hexagon)obj).Y == Y
                    && ((Hexagon)obj).Z == Z)
            {
                equals = true;
            }
            else if (obj is Vector3
              && (int)((Vector3)obj).x == X
              && (int)((Vector3)obj).y == Y
              && (int)((Vector3)obj).z == Z)
            {
                equals = true;
            }
            return equals;
        }
        public String GetCubeCoordinatesArt()
        {
            String s = null;
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            // sb.Append(" _ _ ");
            sb.Append("/");
            sb.Append(Y);
            int len = Y.ToString().Length;
            for (int i = "     ".Length - len; i > 0; i--)
            {
                sb.Append(' ');
            }
            sb.Append("\\");
            sb.Append('\n');
            sb.Append("/");
            len = X.ToString().Length;
            for (int i = "       ".Length - len; i > 0; i--)
            {
                sb.Append(' ');
            }
            sb.Append(X);
            sb.Append("\\");
            sb.Append('\n');
            sb.Append("\\");
            sb.Append(Z);
            len = Z.ToString().Length;
            for (int i = "       ".Length - len; i > 0; i--)
            {
                sb.Append(' ');
            }
            sb.Append("/");
            sb.Append('\n');
            sb.Append("\\ _ _ /");
            s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
        public int GetCubeCoordinatesArtHeight()
        {
            return 4;
        }
        public int GetCubeCoordinatesArtWidth()
        {
            return 8;
        }
        /**
         * Gets the hexagon's height.
         * @return <tt>float</tt>
         */
        public float GetHeight()
        {
            return height;
        }
        /**
         * Gets the position (vertex) of a specific corner of a hexagon.
         * @param center the hexagon's center point.
         * @param cornerId the corner id
         * @param newSize the hexagon's new size
         * @return {@link Vector2}
         * @ if the size was never declared
         */
        public Vector2 GetHexCornerVertex(Vector2 center,
                 float newSize, int cornerId)
        {
            SetSize(newSize);
            return GetHexCornerVertex(center, cornerId);
        }
        /**
         * Gets the position (vertex) of a specific corner of a hexagon.
         * @param center the hexagon's center point.
         * @param cornerId the corner id
         * @return {@link Vector2}
         * @ if the size was never declared
         */
        public Vector2 GetHexCornerVertex(Vector2 center, int cornerId)
        {
            if (size <= 0)
            {
                throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Size was never set!");
            }
            int offset = 0;
            if (!flat)
            {
                offset = thirty;
            }
            float angleDeg = sixty * cornerId + offset;
            float angleRad = (float)(Math.PI / oneEighty * angleDeg);
            return new Vector2(center.x + size * Mathf.Cos(angleRad),
                    center.y + size * Mathf.Sin(angleRad));
        }
        /**
         * Gets the horizontal distance between adjacent hexes.
         * @return <tt>float</tt>
         */
        public float GetHorizontalDistance()
        {
            return horizontalDistance;
        }
        public int GetId()
        {
            return id;
        }
        public String GetOffsetCoordinatesArt(HexCoordinateSystem grid)
        {
            Vector2 pt = grid.GetOffsetCoordinates(this);
            int x1 = (int)pt.x;
            int y1 = (int)pt.y;
            PooledStringBuilder sb =
                    StringBuilderPool.Instance.GetStringBuilder();
            // sb.Append(" _ _ ");
            sb.Append("/     \\");
            sb.Append('\n');
            sb.Append("/");
            int len = x1.ToString().Length;
            len++;
            len += y1.ToString().Length;
            int off = "       ".Length - len;
            int lef = off / 2;
            for (int i = lef; i > 0; i--)
            {
                sb.Append(' ');
            }
            sb.Append(x1);
            sb.Append(",");
            sb.Append(y1);
            for (int i = off - lef; i > 0; i--)
            {
                sb.Append(' ');
            }
            sb.Append("\\");
            sb.Append('\n');
            sb.Append("\\       /");
            sb.Append('\n');
            sb.Append("\\ _ _ /");
            String s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
        /**
         * Gets the distance between a hexagon's center point and a corner.
         * @return <tt>float</tt>
         */
        public float GetSize()
        {
            return size;
        }
        /**
         * Gets the {@link Hexagon}'s position.
         * @return {@link Vector3}
         */
        public Vector3 GetVector()
        {
            return new Vector3(X, Y, Z);
        }
        /**
         * Gets the vertical distance between adjacent hexes.
         * @return <tt>float</tt>
         */
        public float GetVerticalDistance()
        {
            return verticalDistance;
        }
        /**
         * Gets the hexagon's width.
         * @return <tt>float</tt>
         */
        public float GetWidth()
        {
            return width;
        }
        /**
         * Gets the value of the flat.
         * @return {@link bool}
         */
        public bool IsFlat()
        {
            return flat;
        }
        /**
         * Rotates the hexagon 60 degrees to the right.
         * @ if an error occurs
         */
        public virtual void Rotate()
        {
            int oldx = X, oldy = Y, oldz = Z;
            X = -oldz;
            Y = -oldx;
            Z = -oldy;
        }
        /**
         * Sets the {@link Hexagon}'s cube coordinates.
         * @param newX the X-coordinate
         * @param newY the Y-coordinate
         * @param newZ the Z-coordinate
         */
        public void setCoordinates(int newX, int newY,
                 int newZ)
        {
            X = newX;
            Y = newY;
            Z = newZ;
        }
        /**
         * Sets the {@link Hexagon}'s cube coordinates.
         * @param v3 the new coordinates
         */
        public void setCoordinates(Vector3 v3)
        {
            X = (int)v3.x;
            Y = (int)v3.y;
            Z = (int)v3.z;
        }
        /**
         * Sets the distance between a hexagon's center point and a corner.
         * @param newSize the hexagon's new size
         */
        public void SetSize(float newSize)
        {
            size = newSize;
            if (flat)
            {
                width = size * 2;
                horizontalDistance = width * three / four;
                height = (float)(Math.Sqrt(three) / 2 * width);
                verticalDistance = height;
            }
            else
            {
                height = size * 2;
                verticalDistance = height * three / four;
                width = (float)(Math.Sqrt(three) / 2 * height);
                horizontalDistance = width;
            }
        }
        public String ToCubeCoordinateString()
        {
            return new Vector3(X, Y, Z).ToString();
        }
    }
}

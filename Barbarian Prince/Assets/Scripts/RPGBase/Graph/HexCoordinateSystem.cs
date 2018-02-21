using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.RPGBase.Graph
{
    public class HexCoordinateSystem
    {
        /// <summary>
        /// pre-calculated changes to find a hex's neighbor coordinates.
        /// </summary>
        private static Vector3[] COMPOUND_NEIGHBORS = new Vector3[] {new Vector3(
                                                                                        4,
                                                                                        5,
                                                                                        -9),			// side
																										// 0
																				new Vector3(
                                                                                        9,
                                                                                        -4,
                                                                                        -5),			// side
																										// 1
																				new Vector3(
                                                                                        5,
                                                                                        -9,
                                                                                        4),				// side
																										// 2
																				new Vector3(
                                                                                        -4,
                                                                                        -5,
                                                                                        9),				// side
																										// 3
																				new Vector3(
                                                                                        -9,
                                                                                        4,
                                                                                        5),				// side
																										// 4
																				new Vector3(
                                                                                        -5,
                                                                                        9,
                                                                                        -4),			// side
																										// 5
																		};
        /// <summary>
        /// direction N.
        /// </summary>
        public const int DIRECTION_N = 0;
        /** direction NNE. */
        public const int DIRECTION_NNE = 1;
        /** direction NNW. */
        public const int DIRECTION_NNW = 5;
        /** direction S. */
        public const int DIRECTION_S = 3;
        /** direction SSE. */
        public const int DIRECTION_SSE = 2;
        /** direction SSW. */
        public const int DIRECTION_SSW = 4;
        /**
         * <p>
         * layout for flat-topped hexagons where hex columns are aligned with even-numbered columns sticking
         * out at the bottom.
         * </p>
         * &nbsp;&nbsp;&nbsp;1,<b>0</b>&nbsp;&nbsp;&nbsp;3,<b>0</b><br>
         * 0,<b>0</b>&nbsp;&nbsp;&nbsp;2,<b>0</b>&nbsp;&nbsp;&nbsp;4,<b>0</b><br>
         * &nbsp;&nbsp;&nbsp;1,<b>1</b>&nbsp;&nbsp;&nbsp;3,<b>1</b><br>
         * 0,<b>1</b>&nbsp;&nbsp;&nbsp;2,<b>1</b>&nbsp;&nbsp;&nbsp;4,<b>1</b><br>
         */
        public const int EVEN_Q = 3;
        /**
         * <p>
         * layout for pointy-topped hexagons where hex rows are aligned with
         * even-numbered rows sticking out to the right.
         * </p>
         * &nbsp;&nbsp;&nbsp;0,<b>0</b>&nbsp;&nbsp;&nbsp;1,<b>0</b>&nbsp;&nbsp;
         * &nbsp;2,<b>0</b> - row 0<br>
         * 0,<b>1</b>&nbsp;&nbsp;&nbsp;1,<b>1</b>&nbsp;&nbsp;&nbsp;2,<b>1</b>&nbsp;
         * &nbsp;&nbsp; - row 1<br>
         * &nbsp;&nbsp;&nbsp;0,<b>2</b>&nbsp;&nbsp;&nbsp;1,<b>2</b>&nbsp;&nbsp;
         * &nbsp;2,<b>2</b> - row 2<br>
         * 0,<b>3</b>&nbsp;&nbsp;&nbsp;1,<b>3</b>&nbsp;&nbsp;&nbsp;2,<b>3</b>&nbsp;
         * &nbsp;&nbsp; - row 3<br>
         */
        public const int EVEN_R = 1;
        /** pre-calculated changes to find a hex's neighbor coordinates. */
        private static Vector3[][] NEIGHBORS = new Vector3[][] {new Vector3[]
        { /** ODD-R. */
				new Vector3(1, 0, -1), /** North. */
				new Vector3(1, -1, 0), /** NorthNorthEast. */
				new Vector3(0, -1, 1), /** SouthSouthEast. */
				new Vector3(-1, 0, 1), /** South. */
				new Vector3(-1, 1, 0), /** SouthSouthWest. */
				new Vector3(0, 1, -1), /** NorthNorthWest. */
			}, new Vector3[]{ /** EVEN_R. */
				new Vector3(1, 0, -1), /** North. */
				new Vector3(1, -1, 0), /** NorthNorthEast. */
				new Vector3(0, -1, 1), /** SouthSouthEast. */
				new Vector3(-1, 0, 1), /** South. */
				new Vector3(-1, 1, 0), /** SouthSouthWest. */
				new Vector3(0, 1, -1), /** NorthNorthWest. */
			}, new Vector3[]{ /** ODD_Q. */
				new Vector3(0, 1, -1), /** North. */
				new Vector3(1, 0, -1), /** NorthNorthEast. */
				new Vector3(1, -1, 0), /** SouthSouthEast. */
				new Vector3(0, -1, 1), /** South. */
				new Vector3(-1, 0, 1), /** SouthSouthWest. */
				new Vector3(-1, 1, 0), /** NorthNorthWest. */
			}, new Vector3[]{ /** EVEN_Q. */
				new Vector3(0, 1, -1), /** North. */
				new Vector3(1, 0, -1), /** NorthNorthEast. */
				new Vector3(1, -1, 0), /** SouthSouthEast. */
				new Vector3(0, -1, 1), /** South. */
				new Vector3(-1, 0, 1), /** SouthSouthWest. */
				new Vector3(-1, 1, 0) /** NorthNorthWest. */
			}
    };
        /**
         * <p>
         * layout for flat-topped hexagons where hex columns are aligned with
         * odd-numbered columns sticking out at the bottom.
         * </p>
         * 0,<b>0</b>&nbsp;&nbsp;&nbsp;2,<b>0</b>&nbsp;&nbsp;&nbsp;4,<b>0</b><br>
         * &nbsp;&nbsp;&nbsp;1,<b>0</b>&nbsp;&nbsp;&nbsp;3,<b>0</b><br>
         * 0,<b>1</b>&nbsp;&nbsp;&nbsp;2,<b>1</b>&nbsp;&nbsp;&nbsp;4,<b>1</b><br>
         * &nbsp;&nbsp;&nbsp;1,<b>1</b>&nbsp;&nbsp;&nbsp;3,<b>1</b><br>
         */
        public const int ODD_Q = 2;
        /**
         * <p>
         * layout for pointy-topped hexagons where hex rows are aligned with
         * odd-numbered rows sticking out to the right.
         * </p>
         * 0,<b>0</b>&nbsp;&nbsp;&nbsp;1,<b>0</b>&nbsp;&nbsp;&nbsp;2,<b>0</b>&nbsp;
         * &nbsp;&nbsp; - row 0<br>
         * &nbsp;&nbsp;&nbsp;0,<b>1</b>&nbsp;&nbsp;&nbsp;1,<b>1</b>&nbsp;&nbsp;
         * &nbsp;2,<b>1</b> - row 1<br>
         * 0,<b>2</b>&nbsp;&nbsp;&nbsp;1,<b>2</b>&nbsp;&nbsp;&nbsp;2,<b>2</b>&nbsp;
         * &nbsp;&nbsp; - row 2<br>
         * &nbsp;&nbsp;&nbsp;0,<b>3</b>&nbsp;&nbsp;&nbsp;1,<b>3</b>&nbsp;&nbsp;
         * &nbsp;2,<b>3</b> - row 3<br>
         */
        public const int ODD_R = 0;
        /** the list of {@link Hexagon}s in the coordinate system. */
        private Hexagon[] hexes;
        /** the next available reference id. */
        private int nextId;
        /** the system's offset configuration. */
        private int offsetConfiguration;
        /**
         * Creates a new instance of {@link HexCoordinateSystem}.
         * @param config the system's offset configuration
         */
        public HexCoordinateSystem(int config)
        {
            hexes = new Hexagon[0];
            offsetConfiguration = config;
        }
        /**
         * Adds a {@link Hexagon} to the coordinate system.
         * @param hex the {@link Hexagon} being added
         */
        public void AddHexagon(Hexagon hex)
        {
            switch (offsetConfiguration)
            {
                case ODD_R:
                case EVEN_R:
                    if (hex.IsFlat())
                    {
                        throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "Cannot have-flat topped hexagons with horizontal layout");
                    }
                    break;
                case ODD_Q:
                case EVEN_Q:
                    if (!hex.IsFlat())
                    {
                        throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "Cannot have pointy topped hexagons with vertical layout");
                    }
                    break;
                default:
                    throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "Invalid offset coordinates " + offsetConfiguration);
            }
            int index = hexes.Length - 1;
            for (; index >= 0; index--)
            {
                if (hexes[index] != null && hexes[index].Equals(hex))
                {
                    break;
                }
            }
            if (index >= 0)
            {
                hexes[index] = hex;
            }
            else
            {
                hexes = ArrayUtilities.Instance.ExtendArray(hex, hexes);
            }
            if (nextId <= hex.GetId())
            {
                nextId = hex.GetId() + 1;
            }
        }
        /**
         * Adds a {@link Hexagon} to the coordinate system.
         * @param x the {@link Hexagon}'s x-coordinate
         * @param z the {@link Hexagon}'s z-coordinate
         * @return {@link Hexagon}
         * @ 
         */
        public Hexagon AddHexagon(int x, int z)


        {
            Hexagon hex = GetHexagon(x, z);
            if (hex == null)
            {
                hex = new Hexagon(nextId++);
                hex.setCoordinates(GetCubeCoordinates(x, z));
                hexes = ArrayUtilities.Instance.ExtendArray(hex, hexes);
            }
            return hex;
        }
        /**
         * Gets the distance between two hexes.
         * @param hex0 the first hex
         * @param hex1 the second hex
         * @return <code>int</code>
         */
        public int Distance(Hexagon hex0, Hexagon hex1)
        {
            return (Math.Abs(hex0.X - hex1.X) + Math.Abs(hex0.Y - hex1.Y) + Math.Abs(hex0.Z - hex1.Z)) / 2;
        }
        /**
         * Gets the distance between two hexes in a cube coordinate system.
         * @param v0 the first hex
         * @param v1 the second hex
         * @return <code>int</code>
         */
        public int CubeDistance(Vector3 v0, Vector3 v1)
        {
            return (int)((Math.Abs(v0.x - v1.x) + Math.Abs(v0.y - v1.y) + Math.Abs(v0.z - v1.z)) / 2);
        }
        /**
         * Gets the distance between two hexes in an axial coordinate system.
         * @param p0 the first hex
         * @param p1 the second hex
         * @return <code>int</code>
         * @ if an error occurs
         */
        public int AxialDistance(Vector2 p0, Vector2 p1)
        {
            Vector3 ac = GetCubeCoordinates((int)p0.x, (int)p0.y);
            Vector3 bc = GetCubeCoordinates((int)p1.x, (int)p1.y);
            return CubeDistance(ac, bc);
        }
        /**
         * Gets the distance between two hexes.
         * @param v0 the first hex
         * @param v1 the second hex
         * @return <code>int</code>
         * @ if an error occurs
         */
        public int Distance(Vector3 v0, Vector3 v1)
        {
            Vector2 p0 = getAxialCoordinates(v0);
            Vector2 p1 = getAxialCoordinates(v1);
            return AxialDistance(p0, p1);
        }
        /**
         * Gets the {@link Hexagon}'s axial coordinates.
         * @param hexagon the {@link Hexagon}
         * @return {@link Vector2}
         * @ if the configuration is invalid
         */
        public Vector2 GetAxialCoordinates(Hexagon hexagon)
        {
            return getAxialCoordinates(hexagon.GetVector());
        }
        /**
         * Gets the {@link Vector3}'s axial coordinates.
         * @param v3 the {@link Vector3}
         * @return {@link Vector2}
         * @ if the configuration is invalid
         */
        public Vector2 getAxialCoordinates(Vector3 v3)
        {
            int q, r;
            switch (offsetConfiguration)
            {
                case EVEN_Q:
                    q = (int)v3.x;
                    r = (int)v3.z + ((int)v3.x + ((int)v3.x & 1)) / 2;
                    break;
                case ODD_Q:
                    q = (int)v3.x;
                    r = (int)v3.z + ((int)v3.x - ((int)v3.x & 1)) / 2;
                    break;
                case EVEN_R:
                    q = (int)v3.x + ((int)v3.z + ((int)v3.z & 1)) / 2;
                    r = (int)v3.z;
                    break;
                case ODD_R:
                    q = (int)v3.x + ((int)v3.z - ((int)v3.z & 1)) / 2;
                    r = (int)v3.z;
                    break;
                default:
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Invalid offset configuration " + offsetConfiguration);
            }
            return new Vector2(q, r);
        }
        /**
         * Gets the {@link Hexagon}'s cube coordinates.
         * @param hexagon the {@link Hexagon}
         * @return {@link Vector3}
         */
        public Vector3 GetCubeCoordinates(Hexagon hexagon)
        {
            return new Vector3(hexagon.X, hexagon.Y, hexagon.Z);
        }
        /**
         * Gets the cube coordinates for a specific column and row.
         * @param q the column
         * @param r the row
         * @return {@link Vector3}
         * @ if the system's offset configuration is invalid
         */
        public Vector3 GetCubeCoordinates(int q, int r)
        {
            Vector3 v3 = new Vector3();
            int x1, y1, z1;
            switch (offsetConfiguration)
            {
                case EVEN_Q:
                    x1 = q;
                    z1 = r - (q + (q & 1)) / 2;
                    y1 = -x1 - z1;
                    break;
                case ODD_Q:
                    x1 = q;
                    z1 = r - (q - (q & 1)) / 2;
                    y1 = -x1 - z1;
                    break;
                case EVEN_R:
                    x1 = q - (r + (r & 1)) / 2;
                    z1 = r;
                    y1 = -x1 - z1;
                    break;
                case ODD_R:
                    x1 = q - (r - (r & 1)) / 2;
                    z1 = r;
                    y1 = -x1 - z1;
                    break;
                default:
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Invalid offset configuration " + offsetConfiguration);
            }
            v3.Set(x1, y1, z1);
            return v3;
        }
        /**
         * Gets a hexagon at a specific index.
         * @param index the index
         * @return {@link Hexagon}
         */
        protected Hexagon GetHexagon(int index)
        {
            return hexes[index];
        }
        /**
         * Gets a hexagon at a specific set of coordinates.
         * @param x the x-coordinate
         * @param z the z-coordinate
         * @return {@link Hexagon}
         * @ if an error occurs
         */
        public Hexagon GetHexagon(int x, int z)
        {
            Hexagon hex = null;
            Vector3 v = GetCubeCoordinates(x, z);
            for (int i = hexes.Length - 1; i >= 0; i--)
            {
                if (hexes[i].Equals(v))
                {
                    hex = hexes[i];
                    break;
                }
            }
            return hex;
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
                    break;
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
         * Gets the coordinates for a neighboring {@link Hexagon}.
         * @param hexagon the original {@link Hexagon}
         * @param direction the direction in which the neighbor lies
         * @return {@link Vector3}
         * @ if the direction was invalid
         */
        public Vector3 GetNeighborCoordinates(Hexagon hexagon, int direction)
        {
            Vector3 neighbor = new Vector3(hexagon.X, hexagon.Y, hexagon.Z);
            return GetNeighborCoordinates(neighbor, direction);
        }
        /**
         * Gets the coordinates for a neighboring {@link Hexagon}.
         * @param coords the original {@link Hexagon}'s coordinates
         * @param direction the direction of the neighboring {@link Hexagon}
         * @return {@link Vector3}
         * @ if the direction was invalid
         */
        public Vector3 GetNeighborCoordinates(Vector3 coords, int direction)
        {
            // copy current coordinates
            Vector3 neighbor = new Vector3(coords.x, coords.y, coords.z);
            Vector3 v;
            // convert coords to axial
            Vector2 pt = this.getAxialCoordinates(coords);
            switch (offsetConfiguration)
            {
                case ODD_R:
                    if (((int)pt.y & 1) == 0)
                    {
                        switch (direction)
                        {
                            case DIRECTION_N:
                                v = new Vector3(1, 0, -1);
                                break;
                            case DIRECTION_NNE:
                                v = new Vector3(1, -1, 0);
                                break;
                            case DIRECTION_SSE:
                                v = new Vector3(0, -1, 1);
                                break;
                            case DIRECTION_S:
                                v = new Vector3(-1, 0, 1);
                                break;
                            case DIRECTION_SSW:
                                v = new Vector3(-1, 1, 0);
                                break;
                            case DIRECTION_NNW:
                                v = new Vector3(0, 1, -1);
                                break;
                        }
                    }
                    else
                    {
                        switch (direction)
                        {
                            case DIRECTION_N:
                                v = new Vector3(1, 0, -1);
                                break;
                        }
                    }
                    break;
            }
            // check for row even or odd
            if (((int)pt.y & 1) == 0)
            {

            }
            neighbor += HexCoordinateSystem.NEIGHBORS[offsetConfiguration][direction];
            return neighbor;
        }
        /**
         * Gets the next available reference id.
         * @return <code>int</code>
         */
        public int GetNextId()
        {
            return nextId++;
        }
        /**
         * Gets the value of the offsetConfiguration.
         * @return {@link int}
         */
        public int GetOffsetConfiguration()
        {
            return offsetConfiguration;
        }
        /**
         * Gets a {@link Hexagon}'s offset coordinates.
         * @param hexagon the {@link Hexagon}
         * @return {@link Vector2}
         * @ if the offset configuration was set to an invalid
         * value
         */
        public Vector2 GetOffsetCoordinates(Hexagon hexagon)
        {
            int col, row;
            switch (offsetConfiguration)
            {
                case EVEN_Q:
                    col = hexagon.X;
                    row = hexagon.Z + (hexagon.X + (hexagon.X & 1)) / 2;
                    break;
                case ODD_Q:
                    col = hexagon.X;
                    row = hexagon.Z + (hexagon.X - (hexagon.X & 1)) / 2;
                    break;
                case EVEN_R:
                    col = hexagon.X + (hexagon.Z + (hexagon.Z & 1)) / 2;
                    row = hexagon.Z;
                    break;
                case ODD_R:
                    col = hexagon.X + (hexagon.Z - (hexagon.Z & 1)) / 2;
                    row = hexagon.Z;
                    break;
                default:
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Invalid offset configuration " + offsetConfiguration);
            }
            return new Vector2(col, row);
        }
        public int GetSharedEdge(Hexagon hex0, Hexagon hex1)
        {
            return GetSharedEdge(hex0.GetVector(), hex1.GetVector());
        }
        public int GetSharedEdge(Vector3 hex0, Vector3 hex1)
        {
            int i = HexCoordinateSystem.NEIGHBORS[offsetConfiguration].Length - 1;
            if (Distance(hex0, hex1) == 1)
            {
                for (; i >= 0; i--)
                {
                    Vector3 v0 = new Vector3(hex1.x, hex1.y, hex1.z);
                    v0 -= hex0;
                    Vector3 neighbor = HexCoordinateSystem.NEIGHBORS[offsetConfiguration][i];
                    if (v0.Equals(neighbor))
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "The hexes are not neighbors - " + hex0 + ", " + hex1);
            }
            return i;
        }
        /**
         * Gets the number of hexes in the system.
         * @return <code>int</code>
         */
        protected int Length()
        {
            return hexes.Length;
        }
        public void MoveCompoundHexagonToSide(CompoundHexagon compoundHexagon,
                 Vector3 v3, int side)
        {
            // find current location
            CompoundHexagon current = (CompoundHexagon)this.GetHexagon(v3);
            Console.WriteLine("found hex " + current.ToString() + " at " + v3);
            for (int i = current.GetNumberOfHexes() - 1; i >= 0; i--)
            {
                Vector3 v = new Vector3(current.GetHexagon(i).GetVector().x, current.GetHexagon(i).GetVector().y, current.GetHexagon(i).GetVector().z);
                v += HexCoordinateSystem.COMPOUND_NEIGHBORS[side];
                compoundHexagon.GetHexagon(i).setCoordinates(v);
            }
        }
        public String PrintGrid(Hexagon center)
        {
            // get Northern coordinates
            Vector3 nv = GetNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_N);
            nv = GetNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
            nv = GetNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
            nv = GetNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
            // get Southern coordinates
            Vector3 sv =
                    GetNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_S);
            sv = GetNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
            sv = GetNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
            sv = GetNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
            // all hexes saved in coordinates
            // need to print 7 * height lines
            // print hexes
            int sD = Distance(nv, sv) + 1;
            // need to print
            int numLines = sD * center.GetCubeCoordinatesArtHeight();
            PooledStringBuilder line = StringBuilderPool.Instance.GetStringBuilder();
            // switch to offset coords
            int q = (int)(GetOffsetCoordinates(center).x - 4);
            int maxQ = (int)(GetOffsetCoordinates(center).x + 4);
            int r = (int)(GetOffsetCoordinates(center).y - 4);
            for (int i = 0; i < numLines; i++)
            {
                if (i % 4 == 0 && i > 0)
                {
                    r++;
                }
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                bool centerisOdd = GetOffsetCoordinates(center).x % 2 == 1;
                int col = q;
                while (col <= maxQ)
                {
                    bool columnIsEven = col % 2 == 0;
                    int row = r;
                    if (centerisOdd)
                    {
                        if (columnIsEven)
                        {
                            if (i % 4 < 2)
                            {
                                row--;
                            }
                        }
                    }
                    else
                    {
                        if (!columnIsEven)
                        {
                            if (i % 4 > 1)
                            {
                                row++;
                            }
                        }
                    }
                    Vector3 v3 = GetCubeCoordinates(col, row);
                    Hexagon hex = GetHexagon(v3);
                    if (hex != null)
                    {
                        String[] split = hex.GetCubeCoordinatesArt().Split('\n');
                        // how do i know if i am printing top or bottom?
                        if (centerisOdd)
                        {
                            if (columnIsEven)
                            {
                                if (i % 4 < 2)
                                {
                                    // printing bottom
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 + 2]);
                                }
                                else
                                {
                                    // printing top
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 - 2]);
                                }
                            }
                            else
                            {
                                if ((i % 4 == 0 || i % 4 == 3)
                                        && sb.Length() == 0)
                                {
                                    sb.Append(' ');
                                }
                                sb.Append(split[i % 4]);
                            }
                        }
                        else
                        {
                            // center is not odd
                            if (columnIsEven)
                            {
                                if ((i % 4 == 0 || i % 4 == 3)
                                        && sb.Length() == 0)
                                {
                                    sb.Append(' ');
                                }
                                sb.Append(split[i % 4]);
                            }
                            else
                            {
                                if (i % 4 < 2)
                                {
                                    // printing bottom
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 + 2]);
                                }
                                else
                                {
                                    // printing top
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 - 2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (centerisOdd)
                        {
                            if (columnIsEven)
                            {
                                if (i % 4 == 1 || i % 4 == 2)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                            else
                            {
                                if (i % 4 == 0 || i % 4 == 3)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                        }
                        else
                        {
                            if (columnIsEven)
                            {
                                if (i % 4 == 0 || i % 4 == 3)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                            else
                            {
                                if (i % 4 == 1 || i % 4 == 2)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                        }
                    }
                    col++;
                }
                line.Append(sb.ToString());
                line.Append('\n');
                sb.ReturnToPool();
                sb = null;
            }
            String s = line.ToString();
            line.ReturnToPool();
            line = null;
            return s;
        }
        /**
         * Prints the hex map with all hexes as cube coordinates, with the supplied
         * hex as the center point.
         * @param center the center point
         * @return {@link String}
         * @throws PooledException should not happen
         * @ if there is an error getting any cell's coordinates
         */
        public String PrintCubeCoordinatesView(Hexagon center)
        {
            // get Northern coordinates
            Vector3 nv =
                    GetNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_N);
            nv = GetNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
            nv = GetNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
            nv = GetNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
            // get Southern coordinates
            Vector3 sv =
                    GetNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_S);
            sv = GetNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
            sv = GetNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
            sv = GetNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
            // all hexes saved in coordinates
            // need to print 7 * height lines
            // print hexes
            int sD = Distance(nv, sv) + 1;
            // need to print
            int numLines = sD * center.GetCubeCoordinatesArtHeight();
            PooledStringBuilder line = StringBuilderPool.Instance.GetStringBuilder();
            // switch to offset coords
            int q = (int)(GetOffsetCoordinates(center).x - 4);
            int maxQ = (int)(GetOffsetCoordinates(center).x + 4);
            int r = (int)(GetOffsetCoordinates(center).y - 4);
            for (int i = 0; i < numLines; i++)
            {
                if (i % 4 == 0 && i > 0)
                {
                    r++;
                }
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                bool centerisOdd = GetOffsetCoordinates(center).x % 2 == 1;
                int col = q;
                while (col <= maxQ)
                {
                    bool columnIsEven = col % 2 == 0;
                    int row = r;
                    if (centerisOdd)
                    {
                        if (columnIsEven)
                        {
                            if (i % 4 < 2)
                            {
                                row--;
                            }
                        }
                    }
                    else
                    {
                        if (!columnIsEven)
                        {
                            if (i % 4 > 1)
                            {
                                row++;
                            }
                        }
                    }
                    Vector3 v3 = GetCubeCoordinates(col, row);
                    Hexagon hex = GetHexagon(v3);
                    if (hex != null)
                    {
                        String[] split = hex.GetCubeCoordinatesArt().Split('\n');
                        // how do i know if i am printing top or bottom?
                        if (centerisOdd)
                        {
                            if (columnIsEven)
                            {
                                if (i % 4 < 2)
                                {
                                    // printing bottom
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 + 2]);
                                }
                                else
                                {
                                    // printing top
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 - 2]);
                                }
                            }
                            else
                            {
                                if ((i % 4 == 0 || i % 4 == 3)
                                        && sb.Length() == 0)
                                {
                                    sb.Append(' ');
                                }
                                sb.Append(split[i % 4]);
                            }
                        }
                        else
                        {
                            // center is not odd
                            if (columnIsEven)
                            {
                                if ((i % 4 == 0 || i % 4 == 3)
                                        && sb.Length() == 0)
                                {
                                    sb.Append(' ');
                                }
                                sb.Append(split[i % 4]);
                            }
                            else
                            {
                                if (i % 4 < 2)
                                {
                                    // printing bottom
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 + 2]);
                                }
                                else
                                {
                                    // printing top
                                    if ((i % 4 == 1 || i % 4 == 2)
                                            && sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append(split[i % 4 - 2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (centerisOdd)
                        {
                            if (columnIsEven)
                            {
                                if (i % 4 == 1 || i % 4 == 2)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                            else
                            {
                                if (i % 4 == 0 || i % 4 == 3)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                        }
                        else
                        {
                            if (columnIsEven)
                            {
                                if (i % 4 == 0 || i % 4 == 3)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                            else
                            {
                                if (i % 4 == 1 || i % 4 == 2)
                                {
                                    if (sb.Length() == 0)
                                    {
                                        sb.Append(' ');
                                    }
                                    sb.Append("|*****|");
                                }
                                else
                                {
                                    sb.Append("|*******|");
                                }
                            }
                        }
                    }
                    col++;
                }
                line.Append(sb.ToString());
                line.Append('\n');
                sb.ReturnToPool();
                sb = null;
            }
            String s = line.ToString();
            line.ReturnToPool();
            line = null;
            return s;
        }
    }
}

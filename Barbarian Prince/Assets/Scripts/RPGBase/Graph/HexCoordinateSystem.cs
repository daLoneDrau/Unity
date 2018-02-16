using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.RPGBase.Graph
{
    public class HexCoordinateSystem
    {/** pre-calculated changes to find a hex's neighbor coordinates. */
        private static Vector3[] COMPOUND_NEIGHBORS =
                                                                        new Vector3[] {
                                                                                new Vector3(
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
        /** direction N. */
        public static int DIRECTION_N = 0;
        /** direction NNE. */
        public static int DIRECTION_NNE = 1;
        /** direction NNW. */
        public static int DIRECTION_NNW = 5;
        /** direction S. */
        public static int DIRECTION_S = 3;
        /** direction SSE. */
        public static int DIRECTION_SSE = 2;
        /** direction SSW. */
        public static int DIRECTION_SSW = 4;
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
            Hexagon hex = getHexagon(x, z);
            if (hex == null)
            {
                hex = new Hexagon(nextId++);
                hex.setCoordinates(this.GetCubeCoordinates(x, z));
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
            return (Math.Abs(hex0.getX() - hex1.getX())
                    + Math.Abs(hex0.getY() - hex1.getY()) + Math.abs(hex0.getZ()
                            - hex1.getZ()))
                    / 2;
        }
        /**
         * Gets the distance between two hexes in a cube coordinate system.
         * @param v0 the first hex
         * @param v1 the second hex
         * @return <code>int</code>
         */
        public int CubeDistance(Vector3 v0,
                 Vector3 v1)
        {
            return (int)((Math.abs(v0.getX() - v1.getX())
                    + Math.abs(v0.getY() - v1.getY()) + Math.abs(v0.getZ()
                            - v1.getZ()))
                    / 2);
        }
        /**
         * Gets the distance between two hexes in an axial coordinate system.
         * @param p0 the first hex
         * @param p1 the second hex
         * @return <code>int</code>
         * @ if an error occurs
         */
        public int axialDistance(SimplePoint p0,
                 SimplePoint p1)
        {
            Vector3 ac = getCubeCoordinates((int)p0.getX(), (int)p0.getY());
            Vector3 bc = getCubeCoordinates((int)p1.getX(), (int)p1.getY());
            return cubeDistance(ac, bc);
        }
        /**
         * Gets the distance between two hexes.
         * @param v0 the first hex
         * @param v1 the second hex
         * @return <code>int</code>
         * @ if an error occurs
         */
        public int distance(Vector3 v0, Vector3 v1)


        {
            SimplePoint p0 = getAxialCoordinates(v0);
            SimplePoint p1 = getAxialCoordinates(v1);
            return axialDistance(p0, p1);
        }
        /**
         * Gets the {@link Hexagon}'s axial coordinates.
         * @param hexagon the {@link Hexagon}
         * @return {@link SimplePoint}
         * @ if the configuration is invalid
         */
        public SimplePoint getAxialCoordinates(Hexagon hexagon)


        {
            return getAxialCoordinates(hexagon.getVector());
        }
        /**
         * Gets the {@link Vector3}'s axial coordinates.
         * @param v3 the {@link Vector3}
         * @return {@link SimplePoint}
         * @ if the configuration is invalid
         */
        public SimplePoint getAxialCoordinates(Vector3 v3)


        {
            int q, r;
            switch (offsetConfiguration)
            {
                case EVEN_Q:
                    q = (int)v3.getX();
                    r = (int)v3.getZ() + ((int)v3.getX() + ((int)v3.getX() & 1)) / 2;
                    break;
                case ODD_Q:
                    q = (int)v3.getX();
                    r = (int)v3.getZ() + ((int)v3.getX() - ((int)v3.getX() & 1)) / 2;
                    break;
                case EVEN_R:
                    q = (int)v3.getX() + ((int)v3.getZ() + ((int)v3.getZ() & 1)) / 2;
                    r = (int)v3.getZ();
                    break;
                case ODD_R:
                    q = (int)v3.getX() + ((int)v3.getZ() - ((int)v3.getZ() & 1)) / 2;
                    r = (int)v3.getZ();
                    break;
                default:
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                            "Invalid offset configuration "
                            + offsetConfiguration);
            }
            return new SimplePoint(q, r);
        }
        /**
         * Gets the {@link Hexagon}'s cube coordinates.
         * @param hexagon the {@link Hexagon}
         * @return {@link Vector3}
         */
        public Vector3 getCubeCoordinates(Hexagon hexagon)
        {
            return new Vector3(
                    hexagon.getX(), hexagon.getY(), hexagon.getZ());
        }
        /**
         * Gets the cube coordinates for a specific column and row.
         * @param q the column
         * @param r the row
         * @return {@link Vector3}
         * @ if the system's offset configuration is invalid
         */
        public Vector3 getCubeCoordinates(int q, int r)


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
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                            "Invalid offset configuration "
                            + offsetConfiguration);
            }
            v3.set(x1, y1, z1);
            return v3;
        }
        /**
         * Gets a hexagon at a specific index.
         * @param index the index
         * @return {@link Hexagon}
         */
        protected Hexagon getHexagon(int index)
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
        public Hexagon getHexagon(int x, int z)


        {
            Hexagon hex = null;
            Vector3 v = getCubeCoordinates(x, z);
            for (int i = hexes.length - 1; i >= 0; i--)
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
        public Hexagon getHexagon(int x, int y, int z)
        {
            Hexagon hex = null;
            for (int i = hexes.length - 1; i >= 0; i--)
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
        public Hexagon getHexagon(Vector3 v3)
        {
            return getHexagon((int)v3.getX(), (int)v3.getY(), (int)v3.getZ());
        }
        /**
         * Gets the coordinates for a neighboring {@link Hexagon}.
         * @param hexagon the original {@link Hexagon}
         * @param direction the direction in which the neighbor lies
         * @return {@link Vector3}
         * @ if the direction was invalid
         */
        public Vector3 getNeighborCoordinates(Hexagon hexagon,
                 int direction)
        {
            Vector3 neighbor =

                        new Vector3(hexagon.getX(), hexagon.getY(), hexagon
                                .getZ());
            return getNeighborCoordinates(neighbor, direction);
        }
        /**
         * Gets the coordinates for a neighboring {@link Hexagon}.
         * @param coords the original {@link Hexagon}'s coordinates
         * @param direction the direction of the neighboring {@link Hexagon}
         * @return {@link Vector3}
         * @ if the direction was invalid
         */
        public Vector3 getNeighborCoordinates(
                 Vector3 coords, int direction)


        {
            // copy current coordinates
            Vector3 neighbor = new Vector3(coords);
            Vector3 v = null;
            // convert coords to axial
            SimplePoint pt = this.getAxialCoordinates(coords);
            switch (offsetConfiguration)
            {
                case ODD_R:
                    if (((int)pt.getY() & 1) == 0)
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
            }
            // check for row even or odd
            if (((int)pt.getY() & 1) == 0)
            {

            }
            neighbor.increment(HexCoordinateSystem
                    .NEIGHBORS[offsetConfiguration][direction]);
            return neighbor;
        }
        /**
         * Gets the next available reference id.
         * @return <code>int</code>
         */
        public int getNextId()
        {
            return nextId++;
        }
        /**
         * Gets the value of the offsetConfiguration.
         * @return {@link int}
         */
        public int getOffsetConfiguration()
        {
            return offsetConfiguration;
        }
        /**
         * Gets a {@link Hexagon}'s offset coordinates.
         * @param hexagon the {@link Hexagon}
         * @return {@link SimplePoint}
         * @ if the offset configuration was set to an invalid
         * value
         */
        public SimplePoint getOffsetCoordinates(Hexagon hexagon)


        {
            int col, row;
            switch (offsetConfiguration)
            {
                case EVEN_Q:
                    col = hexagon.getX();
                    row =
                            hexagon.getZ()
                                    + (hexagon.getX() + (hexagon.getX() & 1)) / 2;
                    break;
                case ODD_Q:
                    col = hexagon.getX();
                    row =
                            hexagon.getZ()
                                    + (hexagon.getX() - (hexagon.getX() & 1)) / 2;
                    break;
                case EVEN_R:
                    col =
                            hexagon.getX()
                                    + (hexagon.getZ() + (hexagon.getZ() & 1)) / 2;
                    row = hexagon.getZ();
                    break;
                case ODD_R:
                    col =
                            hexagon.getX()
                                    + (hexagon.getZ() - (hexagon.getZ() & 1)) / 2;
                    row = hexagon.getZ();
                    break;
                default:
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                            "Invalid offset configuration " + offsetConfiguration);
            }
            return new SimplePoint(col, row);
        }
        public int getSharedEdge(Hexagon hex0, Hexagon hex1)


        {
            return getSharedEdge(hex0.getVector(), hex1.getVector());
        }
        public int getSharedEdge(Vector3 hex0,
                 Vector3 hex1)
        {
            int i = HexCoordinateSystem.NEIGHBORS[offsetConfiguration].length - 1;
            if (distance(hex0, hex1) == 1)
            {
                for (; i >= 0; i--)
                {
                    Vector3 v0 = new Vector3(hex1);
                    v0.decrement(hex0);
                    Vector3 neighbor =
                            HexCoordinateSystem.NEIGHBORS[offsetConfiguration][i];
                    if (v0.Equals(neighbor))
                    {
                        neighbor = null;
                        break;
                    }
                    neighbor = null;
                }
            }
            else
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                        "The hexes are not neighbors - " + hex0 + ", "
                        + hex1);
            }
            return i;
        }
        /**
         * Gets the number of hexes in the system.
         * @return <code>int</code>
         */
        protected int length()
        {
            return hexes.length;
        }
        public void moveCompoundHexagonToSide(CompoundHexagon compoundHexagon,
                 Vector3 v3, int side)
        {
            // find current location
            CompoundHexagon current = (CompoundHexagon)this.getHexagon(v3);
            System.out.println("found hex " + current.toString() + " at " + v3);
            for (int i = current.getNumberOfHexes() - 1; i >= 0; i--)
            {
                Vector3 v =
                        new Vector3(current.getHexagon(i).getVector());
                v.increment(HexCoordinateSystem.COMPOUND_NEIGHBORS[side]);
                compoundHexagon.getHexagon(i).setCoordinates(v);
            }
        }
        public String printGrid(Hexagon center) , PooledException {
		// get Northern coordinates
		Vector3 nv =
                getNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_N);
        nv = getNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
        nv = getNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
        nv = getNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
        // get Southern coordinates
        Vector3 sv =
                getNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_S);
        sv = getNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
        sv = getNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
        sv = getNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
        // all hexes saved in coordinates
        // need to print 7 * height lines
        // print hexes
        int sD = distance(nv, sv) + 1;
        // need to print
        int numLines = sD * center.getCubeCoordinatesArtHeight();
        PooledStringBuilder line =
                StringBuilderPool.Instance.getStringBuilder();
        // switch to offset coords
        int q = (int)(getOffsetCoordinates(center).getX() - 4);
        int maxQ = (int)(getOffsetCoordinates(center).getX() + 4);
        int r = (int)(getOffsetCoordinates(center).getY() - 4);
		for (int i = 0; i<numLines; i++) {
			if (i % 4 == 0 && i > 0) {
				r++;
			}
    PooledStringBuilder sb =
            StringBuilderPool.Instance.getStringBuilder();
    boolean centerisOdd = getOffsetCoordinates(center).getX() % 2 == 1;
    int col = q;
			while (col <= maxQ) {
				boolean columnIsEven = col % 2 == 0;
    int row = r;
				if (centerisOdd) {
					if (columnIsEven) {
						if (i % 4 < 2) {
							row--;
						}
					}
				} else {
					if (!columnIsEven) {
						if (i % 4 > 1) {
							row++;
						}
					}
				}
				Vector3 v3 = getCubeCoordinates(col, row);
Hexagon hex = getHexagon(v3);
				if (hex != null) {
					String[] split = hex.getCubeCoordinatesArt().split("\n");
					// how do i know if i am printing top or bottom?
					if (centerisOdd) {
						if (columnIsEven) {
							if (i % 4 < 2) {
								// printing bottom
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 + 2]);
							} else {
								// printing top
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 - 2]);
							}
						} else {
							if ((i % 4 == 0 || i % 4 == 3)
									&& sb.length() == 0) {
								sb.append(' ');
							}
							sb.append(split[i % 4]);
						}
					} else {
						// center is not odd
						if (columnIsEven) {
							if ((i % 4 == 0 || i % 4 == 3)
									&& sb.length() == 0) {
								sb.append(' ');
							}
							sb.append(split[i % 4]);
						} else {
							if (i % 4 < 2) {
								// printing bottom
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 + 2]);
							} else {
								// printing top
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 - 2]);
							}
						}
					}
				} else {
					if (centerisOdd) {
						if (columnIsEven) {
							if (i % 4 == 1 || i % 4 == 2) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						} else {
							if (i % 4 == 0 || i % 4 == 3) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						}
					} else {
						if (columnIsEven) {
							if (i % 4 == 0 || i % 4 == 3) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						} else {
							if (i % 4 == 1 || i % 4 == 2) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						}
					}
				}
				col++;
			}
			line.append(sb.toString());
			line.append('\n');
			sb.returnToPool();
			sb = null;
		}
		String s = line.toString();
line.returnToPool();
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
	public String printCubeCoordinatesView(Hexagon center)

            throws PooledException, RPGException {
		// get Northern coordinates
		Vector3 nv =
                getNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_N);
nv = getNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
nv = getNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
nv = getNeighborCoordinates(nv, HexCoordinateSystem.DIRECTION_N);
// get Southern coordinates
Vector3 sv =
        getNeighborCoordinates(center, HexCoordinateSystem.DIRECTION_S);
sv = getNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
sv = getNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
sv = getNeighborCoordinates(sv, HexCoordinateSystem.DIRECTION_S);
// all hexes saved in coordinates
// need to print 7 * height lines
// print hexes
int sD = distance(nv, sv) + 1;
// need to print
int numLines = sD * center.getCubeCoordinatesArtHeight();
PooledStringBuilder line =
        StringBuilderPool.Instance.getStringBuilder();
// switch to offset coords
int q = (int)(getOffsetCoordinates(center).getX() - 4);
int maxQ = (int)(getOffsetCoordinates(center).getX() + 4);
int r = (int)(getOffsetCoordinates(center).getY() - 4);
		for (int i = 0; i<numLines; i++) {
			if (i % 4 == 0 && i > 0) {
				r++;
			}
			PooledStringBuilder sb =
                    StringBuilderPool.Instance.getStringBuilder();
boolean centerisOdd = getOffsetCoordinates(center).getX() % 2 == 1;
int col = q;
			while (col <= maxQ) {
				boolean columnIsEven = col % 2 == 0;
int row = r;
				if (centerisOdd) {
					if (columnIsEven) {
						if (i % 4 < 2) {
							row--;
						}
					}
				} else {
					if (!columnIsEven) {
						if (i % 4 > 1) {
							row++;
						}
					}
				}
				Vector3 v3 = getCubeCoordinates(col, row);
Hexagon hex = getHexagon(v3);
				if (hex != null) {
					String[] split = hex.getCubeCoordinatesArt().split("\n");
					// how do i know if i am printing top or bottom?
					if (centerisOdd) {
						if (columnIsEven) {
							if (i % 4 < 2) {
								// printing bottom
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 + 2]);
							} else {
								// printing top
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 - 2]);
							}
						} else {
							if ((i % 4 == 0 || i % 4 == 3)
									&& sb.length() == 0) {
								sb.append(' ');
							}
							sb.append(split[i % 4]);
						}
					} else {
						// center is not odd
						if (columnIsEven) {
							if ((i % 4 == 0 || i % 4 == 3)
									&& sb.length() == 0) {
								sb.append(' ');
							}
							sb.append(split[i % 4]);
						} else {
							if (i % 4 < 2) {
								// printing bottom
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 + 2]);
							} else {
								// printing top
								if ((i % 4 == 1 || i % 4 == 2)
										&& sb.length() == 0) {
									sb.append(' ');
								}
								sb.append(split[i % 4 - 2]);
							}
						}
					}
				} else {
					if (centerisOdd) {
						if (columnIsEven) {
							if (i % 4 == 1 || i % 4 == 2) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						} else {
							if (i % 4 == 0 || i % 4 == 3) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						}
					} else {
						if (columnIsEven) {
							if (i % 4 == 0 || i % 4 == 3) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						} else {
							if (i % 4 == 1 || i % 4 == 2) {
								if (sb.length() == 0) {
									sb.append(' ');
								}
								sb.append("|*****|");
							} else {
								sb.append("|*******|");
							}
						}
					}
				}
				col++;
			}
			line.append(sb.toString());
			line.append('\n');
			sb.returnToPool();
			sb = null;
		}
		String s = line.toString();
line.returnToPool();
		line = null;
		return s;
	}
    }
}

using Assets.Scripts.BarbarianPrince.Singletons;
using Assets.Scripts.RPGBase.Graph;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    public class HexMap
    {
        /// <summary>
        /// the distance cost for travelling downriver.
        /// </summary>
        private const float DOWN_RIVER = .33f;
        /// <summary>
        /// the distance cost for travelling upriver.
        /// </summary>
        private const float UP_RIVER = .5f;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static HexMap instance;
        /// <summary>
        /// the instance property
        /// </summary>
        public static HexMap Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HexMap();
                }
                return instance;
            }
        }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private HexMap() { }
        /// <summary>
        /// the hex coordinate system; needed for determining neighbors.
        /// </summary>
        private HexCoordinateSystem coordinateSystem;
        /// <summary>
        /// the list of map hexes.
        /// </summary>
        public Hex[] Hexes { get; set; }
        /// <summary>
        /// the list of <see cref="RiverCrossing"/>s.
        /// </summary>
        public RiverCrossing[] RiverCrossings { get; set; }
        public int[][] Roads { get; internal set; }
        /// <summary>
        /// the map graph.
        /// </summary>
        private EdgeWeightedUndirectedGraph hexGraph;
        /// <summary>
        /// the graph of all river crossing edges.
        /// </summary>
        private EdgeWeightedUndirectedGraph riverCrossingsGraph;
        /// <summary>
        /// the graph of all river edges.
        /// </summary>
        private EdgeWeightedDirectedGraph riverGraph;
        /// <summary>
        /// the list of river nodes.
        /// </summary>
        private RiverGraphNode[] riverNodes;
        /// <summary>
        /// the graph of all road edges.
        /// </summary>
        private EdgeWeightedUndirectedGraph roadGraph;
        /// <summary>
        /// Gets the path from the source hex to a destination.
        /// </summary>
        /// <param name="source">the source location</param>
        /// <param name="v">the destination</param>
        /// <returns><see cref="WeightedGraphEdge"/>[]</returns>
        public WeightedGraphEdge[] GetAirPath(int source, int v)
        {
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(hexGraph, source);
            WeightedGraphEdge[] path = dijkstra.pathTo(v);
            dijkstra = null;
            return path;
        }
        private int NextRiverNodeId
        {
            get
            {
                int i = 0;
                while (true)
                {
                    bool found = false;
                    for (int j = 0, len = riverNodes.Length; j < len; j++)
                    {
                        if (i == riverNodes[j].Index)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        break;
                    }
                    i++;
                }
                return i;
            }
        }
        /// <summary>
        /// Gets a <see cref="Hex"/> by its coordinates.
        /// </summary>
        /// <param name="pt">the coordinates</param>
        /// <returns><see cref="Hex"/></returns>
        public Hex GetHex(Vector2 pt)
        {
            Debug.Log("GetHex(" + pt);
            Hex hex = null;
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                Debug.Log("check " + Hexes[i]);
                if (Hexes[i].Location.Equals(pt))
                {
                    hex = Hexes[i];
                    break;
                }
            }
            return hex;
        }
        /// <summary>
        /// Gets a <see cref="Hex"/> by its coordinates.
        /// </summary>
        /// <param name="v">the coordinates</param>
        /// <returns><see cref="Hex"/></returns>
        public Hex GetHex(Vector3 v)
        {
            Hex hex = null;
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                if (Hexes[i].Hexagon.Equals(v))
                {
                    hex = Hexes[i];
                    break;
                }
            }
            return hex;
        }
        /// <summary>
        /// Gets a <see cref="Hex"/> by its coordinates.
        /// </summary>
        /// <param name="x">the x-coordinates</param>
        /// <param name="y">the y-coordinates</param>
        /// <returns><see cref="Hex"/></returns>
        public Hex GetHex(int x, int y)
        {
            Hex hex = null;
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                if (Hexes[i].Location == new Vector2(x, y))
                {
                    hex = Hexes[i];
                    break;
                }
            }
            return hex;
        }
        /// <summary>
        /// Gets a <see cref="Hex"/> by its id.
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns><see cref="Hex"/></returns>
        public Hex GetHexById(int id)
        {
            Hex hex = null;
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                if (Hexes[i].Index == id)
                {
                    hex = Hexes[i];
                    break;
                }
            }
            return hex;
        }
        /// <summary>
        /// Gets the path from the source hex to a destination.
        /// </summary>
        /// <param name="source">source the source location</param>
        /// <param name="v">the destination</param>
        /// <returns><see cref="WeightedGraphEdge"/>[]</returns>
        public WeightedGraphEdge[] GetLandPath(int source, int v)
        {
            // is destination 
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(roadGraph, source);
            WeightedGraphEdge[] path = dijkstra.pathTo(v);
            if (path == null)
            {
                dijkstra = new DijkstraUndirectedSearch(hexGraph, source);
                path = dijkstra.pathTo(v);
            }
            dijkstra = null;
            return path;
        }
        /// <summary>
        /// Gets a list of path options that are reachable within a certain distance of the location.
        /// </summary>
        /// <param name="pt">the location</param>
        /// <param name="distance">the distance</param>
        /// <returns><see cref="Hex"/>[]</returns>
        public Hex[] GetLandTravelOptions(Vector2 pt, float distance)
        {
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(hexGraph, GetHex(pt).Index);
            Hex[] list = new Hex[0];
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                if (Hexes[i].Index == GetHex(pt).Index)
                {
                    continue;
                }
                if (dijkstra.HasPathTo(Hexes[i].Index)
                        && dijkstra.DistanceTo(Hexes[i].Index) <= distance)
                {
                    list = ArrayUtilities.Instance.ExtendArray(Hexes[i], list);
                }
            }
            return list;
        }
        /// <summary>
        /// Gets the range of hex coordinates the map covers, from the top-left to the bottom-right.
        /// </summary>
        /// <returns><see cref="Vector2"/>[]</returns>
        public Vector2[] GetMapRange()
        {
            int minx = 9999, maxx = -1, miny = 999, maxy = -1;
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                Vector2 coords = Hexes[i].Location;
                minx = (int)Mathf.Min(minx, coords.x);
                maxx = (int)Mathf.Max(maxx, coords.x);
                miny = (int)Mathf.Min(miny, coords.y);
                maxy = (int)Mathf.Max(maxy, coords.y);
            }
            return new Vector2[] { new Vector2(minx, miny), new Vector2(maxx, maxy) };
        }
        /// <summary>
        /// Gets the largest hex id.
        /// </summary>
        public int MaxHexId
        {
            get
            {
                int max = -1;
                for (int i = Hexes.Length - 1; i >= 0; i--)
                {
                    max = Mathf.Max(max, Hexes[i].Index);
                }
                return max;
            }
        }
               /// <summary>
        /// Gets the reference id of a hex's neighbor.
        /// </summary>
        /// <param name="hexId"> the hex's id</param>
        /// <param name="direction">the direction</param>
        /// <returns><see cref="int"/></returns>
        public int GetNeighbor(int hexId, int direction)
        {
            int neighbor = -1;
            Hex hex = GetHexById(hexId);
            if (hex != null)
            {
                hex = GetHex(coordinateSystem.GetNeighborCoordinates(hex.Hexagon, direction));
                if (hex != null
                        && hex.Location.y > 0)
                {
                    neighbor = hex.Index;
                }
            }
            return neighbor;
        }
        public Hex GetNeighborHex(int hexId, int direction)
        {
            Hex neighbor = null;
            Hex hex = GetHexById(hexId);
            if (hex != null)
            {
                hex = GetHex(coordinateSystem.GetNeighborCoordinates(hex.Hexagon, direction));
                if (hex != null
                        && hex.Location.y > 0)
                {
                    neighbor = GetHexById(hex.Index);
                }
            }
            return neighbor;
        }
        public RiverGraphNode[] GetRiverNodesForHex(Vector2 pt)
        {
            return GetRiverNodesForHex(GetHex(pt).Index);
        }
        public RiverGraphNode[] GetRiverNodesForHex(int hexId)
        {
            RiverGraphNode[] nodes = new RiverGraphNode[0];
            for (int i = riverNodes.Length - 1; i >= 0; i--)
            {
                if (riverNodes[i].Type == RiverGraphNode.RIVER_RAFT
                        && (riverNodes[i].Bank0 == hexId
                        || riverNodes[i].Bank1 == hexId))
                {
                    nodes = ArrayUtilities.Instance.ExtendArray(riverNodes[i], nodes);
                }
            }
            return nodes;
        }
        private Hex[] GetRiverNodeAdjacencies(int nodeId)
        {
            Hex[] list = new Hex[0];
            for (int i = riverNodes.Length - 1; i >= 0; i--)
            {
                RiverGraphNode node = riverNodes[i];
                if (riverNodes[i].Type == RiverGraphNode.RIVER_RAFT
                        && riverNodes[i].Index == nodeId)
                {
                    Hex hex = GetHexById(node.Bank0);
                    if (hex.Location.y > 0)
                    {
                        list = ArrayUtilities.Instance.ExtendArray(hex, list);
                    }
                    hex = GetHexById(node.Bank1);
                    if (hex.Location.y > 0)
                    {
                        list = ArrayUtilities.Instance.ExtendArray(hex, list);
                    }
                    hex = null;
                }
            }
            return list;
        }
        /// <summary>
        /// Gets a <see cref="RiverGraphNode"/> by its id.
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns><see cref="RiverGraphNode"/></returns>
        public RiverGraphNode GetRiverNodeById(int id)
        {
            RiverGraphNode node = null;
            for (int i = riverNodes.Length - 1; i >= 0; i--)
            {
                if (riverNodes[i].Index == id)
                {
                    node = riverNodes[i];
                    break;
                }
            }
            return node;
        }
        /// <summary>
        /// Gets the list of path options available when traveling by river from a specific location.
        /// </summary>
        /// <param name="locId">the location id</param>
        /// <returns><see cref="Hex"/>[]</returns>
        public Hex[] GetRiverTravelOptions(int locId)
        {
            return GetRiverTravelOptions(GetHexById(locId));
        }
        /// <summary>
        /// Gets the list of path options available when traveling by river from a specific location.
        /// </summary>
        /// <param name="hex">the location</param>
        /// <returns><see cref="Hex"/>[]</returns>
        public Hex[] GetRiverTravelOptions(Hex hex)
        {
            return GetRiverTravelOptions(hex.Location);
        }
        /// <summary>
        /// Gets the list of path options available when travelling by river from a specific location.
        /// </summary>
        /// <param name="pt">the location</param>
        /// <returns><see cref="Hex"/>[]</returns>
        public Hex[] GetRiverTravelOptions(Vector2 pt)
        {
            RiverGraphNode[] nodes = HexMap.Instance.GetRiverNodesForHex(pt);
            Hex[]
            paths = new Hex[0];
            Dictionary<Vector2, int> map = new Dictionary<Vector2, int>();
            for (int i = nodes.Length - 1; i >= 0; i--)
            {
                Hex[] list = HexMap.Instance.GetRiverPathOptions(nodes[i]);
                for (int j = list.Length - 1; j >= 0; j--)
                {
                    if (list[j].Location == pt)
                    {
                        // skip starting location
                        continue;
                    }
                    if (map.Keys.Contains(list[j].Location))
                    {
                        // do not add locations twice
                        continue;
                    }
                    map[list[j].Location] = 0;
                    paths = ArrayUtilities.Instance.ExtendArray(list[j], paths);
                }
                list = null;
            }
            nodes = null;
            map = null;
            return paths;
        }
        /// <summary>
        /// Gets the name of the river being crossed.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <returns><see cref="string"/></returns>
        public string GetRiverCrossingName(Hex from, Hex to)
        {
            // return WebServiceClient.Instance.getRiverCrossingName(from.Index, to.Index);
            return "";
        }
        /// <summary>
        /// Gets the name of the river being crossed.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <returns><see cref="string"/></returns>
        public String GetRiverCrossingName(int from, int to)
        {
            // return WebServiceClient.Instance.getRiverCrossingName(from, to);
            return "";
        }
        /// <summary>
        /// Gets the name of the river being crossed.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <returns><see cref="string"/></returns>
        public String GetRiverCrossingName(Vector2 from, Vector2 to)
        {
            // return WebServiceClient.Instance.getRiverCrossingName(GetHex(from).Index, GetHex(to).Index);
            return "";
        }
        /// <summary>
        /// Gets a list of path options that are reachable within a certain distance of the location.
        /// </summary>
        /// <param name="node">the location</param>
        /// <param name="v"></param>
        /// <returns></returns>
        public double GetRiverDistanceTo(RiverGraphNode node, int v)
        {
            DijkstraDirectedSearch dijkstra = new DijkstraDirectedSearch(riverGraph, node.Index);
            return dijkstra.DistanceTo(v);
        }
        /// <summary>
        /// Gets a specific river node by the hexes it separates.
        /// </summary>
        /// <param name="hex0">the id for the first hex</param>
        /// <param name="hex1">the id for the second hex</param>
        /// <returns><see cref="RiverGraphNode"/></returns>
        private RiverGraphNode GetRiverNodeByCrossing(int hex0, int hex1)
        {
            RiverGraphNode node = null;
            for (int i = riverNodes.Length - 1; i >= 0; i--)
            {
                if ((riverNodes[i].Bank0 == hex0
                        && riverNodes[i].Bank1 == hex1)
                        || (riverNodes[i].Bank1 == hex0
                                && riverNodes[i].Bank0 == hex1))
                {
                    node = riverNodes[i];
                    break;
                }
            }
            return node;
        }
        /// <summary>
        /// Gets the path from the source hex to a destination.
        /// </summary>
        /// <param name="source">the source location</param>
        /// <param name="v">the destination</param>
        /// <returns><see cref="WeightedGraphEdge"/>[]</returns>
        private WeightedGraphEdge[] GetRiverPath(int source, int v)
        {
            DijkstraDirectedSearch dijkstra = new DijkstraDirectedSearch(riverGraph, source);
            WeightedGraphEdge[] path = dijkstra.PathTo(v);
            dijkstra = null;
            return path;
        }
        public WeightedGraphEdge[] GetRiverPathList(Hex hex0, Hex hex1)
        {
            WeightedGraphEdge[] e = null;
            RiverGraphNode[]
            nodes0 = HexMap.Instance.GetRiverNodesForHex(hex0.Location);
            RiverGraphNode[]
            nodes1 = HexMap.Instance.GetRiverNodesForHex(hex1.Location);
            for (int i = nodes0.Length - 1; i >= 0; i--)
            {
                for (int j = nodes1.Length - 1; j >= 0; j--)
                {
                    if (e == null)
                    {
                        e = GetRiverPath(nodes0[i].Index, nodes1[j].Index);
                    }
                    else
                    {
                        WeightedGraphEdge[] t = GetRiverPath(nodes0[i].Index, nodes1[j].Index);
                        if (e.Length > t.Length)
                        {
                            e = t;
                        }
                        t = null;
                    }
                }
            }
            return e;
        }
        /// <summary>
        /// Gets a list of path options that are reachable within a certain distance of the location.
        /// </summary>
        /// <param name="node">the river node</param>
        /// <returns><see cref="Hex"/>[]</returns>
        private Hex[] GetRiverPathOptions(RiverGraphNode node)
        {
            DijkstraDirectedSearch dijkstra = new DijkstraDirectedSearch(riverGraph, node.Index);
            Hex[] list = new Hex[0];
            for (int i = riverNodes.Length - 1; i >= 0; i--)
            {
                if (dijkstra.HasPathTo(riverNodes[i].Index)
                        && dijkstra.DistanceTo(riverNodes[i].Index) <= 1f)
                {
                    Hex[] adj = GetRiverNodeAdjacencies(riverNodes[i].Index);
                    for (int j = adj.Length - 1; j >= 0; j--)
                    {
                        list = ArrayUtilities.Instance.ExtendArray(adj[j], list);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Determines if a hex has a valid path to another under a specific distance.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <param name="distance">the max distance</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool HasPathTo(Vector2 from, Vector2 to, float distance)
        {
            bool hasPath = false;
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(hexGraph, GetHex(from).Index);
            if (dijkstra.HasPathTo(GetHex(to).Index)
                    && dijkstra.DistanceTo(GetHex(to).Index) <= distance)
            {
                hasPath = true;
            }
            return hasPath;
        }
        /// <summary>
        /// Determines if a hex has a valid path to another under a specific distance.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool HasRiverCrossingTo(Vector2 from, Vector2 to)
        {
            bool hasPath = false;
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(
                    riverCrossingsGraph, GetHex(from).Index);
            if (dijkstra.HasPathTo(GetHex(to).Index)
                    && dijkstra.DistanceTo(GetHex(to).Index) <= 1)
            {
                hasPath = true;
            }
            return hasPath;
        }
        /// <summary>
        /// Determines if a hex has a valid path to another under a specific distance.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool HasRiverCrossingTo(Hex from, Hex to)
        {
            return riverCrossingsGraph.HasEdge(from.Index, to.Index);
            /*
            bool hasPath = false;
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(riverCrossingsGraph, from.Index);
            if (dijkstra.HasPathTo(to.Index)
                    && dijkstra.DistanceTo(to.Index) <= 1)
            {
                hasPath = true;
            }
            return hasPath;
            */
        }
        /// <summary>
        /// Determines if a hex location has river nodes on one or more of its sides.
        /// </summary>
        /// <param name="hexId">the hex id</param>
        /// <returns><tt>true</tt> if the hex has a river on one or more of its side; <tt>false</tt> otherwise</returns>
        public bool HasRiverNode(int hexId)
        {
            bool has = false;
            for (int i = riverNodes.Length - 1; i >= 0; i--)
            {
                if (riverNodes[i].Type == RiverGraphNode.RIVER_RAFT
                        && (riverNodes[i].Bank0 == hexId
                        || riverNodes[i].Bank1 == hexId))
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
        /// <summary>
        /// Determines if a hex location has river nodes on one or more of its sides.
        /// </summary>
        /// <param name="pt">the hex's location</param>
        /// <returns><tt>true</tt> if the hex has a river on one or more of its side; <tt>false</tt> otherwise</returns>
        public bool HasRiverNode(Vector2 pt)
        {
            return HasRiverNode(GetHex(pt).Index);
        }
        /// <summary>
        /// Determines if a hex has a valid path to another under a specific distance.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <param name="distance">the max distance</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool HasRoadTo(Vector2 from, Vector2 to, float distance)
        {
            bool hasPath = false;
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(roadGraph, GetHex(from).Index);
            if (dijkstra.HasPathTo(GetHex(to).Index)
                    && dijkstra.DistanceTo(GetHex(to).Index) <= distance)
            {
                hasPath = true;
            }
            return hasPath;
        }
        /// <summary>
        /// Determines if a hex has a valid path to another under a specific distance.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool IsOnRoad(Hex from)
        {
            return IsOnRoad(from.Index);
        }
        /// <summary>
        /// Determines if a vertex is on the road graph.
        /// </summary>
        /// <param name="v">the source vertex</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool IsOnRoad(int v)
        {
            bool isRoad = false;
            for (int i = roadGraph.NumberOfEdges - 1; i >= 0; i--)
            {
                if (roadGraph.GetEdge(i).From == v
                        || roadGraph.GetEdge(i).To == v)
                {
                    isRoad = true;
                    break;
                }
            }
            return isRoad;
        }
        /// <summary>
        /// Determines if a hex has a valid path to another under a specific distance.
        /// </summary>
        /// <param name="from">the source hex</param>
        /// <param name="to">the destination hex</param>
        /// <param name="distance">the max distance</param>
        /// <returns><tt>true</tt> if there is a valid path; <tt>false</tt> otherwise</returns>
        public bool HasRoadTo(Hex from, Hex to, float distance)
        {
            bool hasPath = false;
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(roadGraph, from.Index);
            if (dijkstra.HasPathTo(to.Index)
                    && dijkstra.DistanceTo(to.Index) <= distance)
            {
                hasPath = true;
            }
            return hasPath;
        }
        public bool HasRoadEdge(Hex from, Hex to)
        {
            return roadGraph.HasEdge(from.Index, to.Index);
        }
        /// <summary>
        /// Loads the hex map.
        /// </summary>
        public void Load()
        {
            // Hexes = BPServiceClient.Instance.LoadHexes();
            hexGraph = new EdgeWeightedUndirectedGraph(0);
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                hexGraph.AddVertex(Hexes[i]);
            }
            coordinateSystem = new HexCoordinateSystem(HexCoordinateSystem.EVEN_Q);
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                Hexagon hexagon = new Hexagon(true, Hexes[i].Index);
                hexagon.SetCoordinates(coordinateSystem.GetCubeCoordinates((int)Hexes[i].Location.x, (int)Hexes[i].Location.y));
                Hexes[i].Hexagon = hexagon;
                coordinateSystem.AddHexagon(Hexes[i].Hexagon);
            }

            // copy hex to road map
            roadGraph = new EdgeWeightedUndirectedGraph(hexGraph);
            // copy hex to river crossings map
            riverCrossingsGraph = new EdgeWeightedUndirectedGraph(hexGraph);
            // copy hex to river map
            //riverGraph = new EdgeWeightedDirectedGraph(hexGraph);

            // create edges between all hexes
            for (int i = Hexes.Length - 1; i >= 0; i--)
            {
                if (Hexes[i].Location.y == 0)
                {
                    continue;
                }
                int dir = HexCoordinateSystem.DIRECTION_NNW;
                for (; dir >= HexCoordinateSystem.DIRECTION_N; dir--)
                {
                    Hexagon h = coordinateSystem.GetHexagon(coordinateSystem.GetNeighborCoordinates(Hexes[i].Hexagon, dir));
                    if (h != null
                            && GetHexById(h.Id).Location.y != 0)
                    {
                        hexGraph.AddEdge(Hexes[i].Index, h.Id);
                    }
                }
            }
            // create edges for roads
            for (int i = Roads.Length - 1; i >= 0; i--)
            {
                roadGraph.AddEdge(Roads[i][0], Roads[i][1]);
            }
            // create edges for river crossings
            for (int i = RiverCrossings.Length - 1; i >= 0; i--)
            {
                riverCrossingsGraph.AddEdge(RiverCrossings[i].From, RiverCrossings[i].To);
            }
            /*
            // add all land nodes to the river graph
            riverGraph = new EdgeWeightedDirectedGraph(0);
            riverNodes = new RiverGraphNode[0];
            for (int i = 0, len = hexes.Length; i < len; i++)
            {
                if (getRiverNodeById(hexes[i].Index) == null)
                {
                    RiverGraphNode node = new RiverGraphNode();
                    node.setType(RiverGraphNode.RIVER_BANK);
                    node.setIndex(hexes[i].Index);
                    node.setHexId(hexes[i].Index);
                    node.setName("");
                    riverNodes = ArrayUtilities.Instance.ExtendArray(
                            node, riverNodes);
                    riverGraph.addVertex(node);
                    node = null;
                }
            }
            // add all water nodes to the river graph
            for (int i = riverCrossings.Length - 1; i >= 0; i--)
            {
                RiverGraphNode node = new RiverGraphNode();
                node.setType(RiverGraphNode.RIVER_RAFT);
                node.setIndex(getNextRiverNodeId());
                node.setBank0(riverCrossings[i].From);
                node.setBank1(riverCrossings[i].To);
                node.setName(riverCrossings[i].getRiverName());
                riverNodes = ArrayUtilities.Instance.ExtendArray(
                        node, riverNodes);
                riverGraph.addVertex(node);
                node = null;
            }
            // get all nodes for Tragoth
            loadTragothRiver();

            loadRiver("Nesser River", false);

            loadRiver("Greater Nesser River", true);

            loadRiver("Lower Nesser River", true);

            loadRiver("Dienstal Branch", false);

            loadRiver("Largos River", true);
            // connect all rivers
            // TRAGOTH->NESSER
            RiverGraphNode up = getRiverNodeByCrossing(
                    GetHex(new Vector2(14, 1)).Index,
                    GetHex(new Vector2(15, 1)).Index);
            RiverGraphNode down = getRiverNodeByCrossing(
                    GetHex(new Vector2(14, 1)).Index,
                    GetHex(new Vector2(15, 2)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            up = getRiverNodeByCrossing(
                    GetHex(new Vector2(15, 1)).Index,

                    GetHex(new Vector2(15, 2)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            // NESSER->GREATER NESSER
            up = getRiverNodeByCrossing(
                    GetHex(new Vector2(6, 9)).Index,

                    GetHex(new Vector2(7, 9)).Index);
            down = getRiverNodeByCrossing(
                    GetHex(new Vector2(7, 9)).Index,

                    GetHex(new Vector2(7, 10)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            up = getRiverNodeByCrossing(
                    GetHex(new Vector2(6, 9)).Index,

                    GetHex(new Vector2(7, 10)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            // DIENSTAL->GREATER NESSER
            up = getRiverNodeByCrossing(
                    GetHex(new Vector2(12, 18)).Index,

                    GetHex(new Vector2(12, 19)).Index);
            down = getRiverNodeByCrossing(
                    GetHex(new Vector2(12, 18)).Index,

                    GetHex(new Vector2(11, 19)).Index);
            riverGraph.addEdge(up.Index, down.Index, UP_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            down = getRiverNodeByCrossing(
                    GetHex(new Vector2(11, 19)).Index,

                    GetHex(new Vector2(12, 19)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            // LOWER NESSER->GREATER NESSER
            up = getRiverNodeByCrossing(
                    GetHex(new Vector2(14, 22)).Index,

                    GetHex(new Vector2(15, 23)).Index);
            down = getRiverNodeByCrossing(
                    GetHex(new Vector2(15, 22)).Index,

                    GetHex(new Vector2(15, 23)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, DOWN_RIVER);
            up = getRiverNodeByCrossing(
                    GetHex(new Vector2(14, 22)).Index,

                    GetHex(new Vector2(15, 22)).Index);
            down = getRiverNodeByCrossing(
                    GetHex(new Vector2(14, 22)).Index,

                    GetHex(new Vector2(15, 23)).Index);
            riverGraph.addEdge(up.Index, down.Index, DOWN_RIVER);
            riverGraph.addEdge(down.Index, up.Index, UP_RIVER);
            */
        }
        /// <summary>
        /// Loads all nodes for a specific river.
        /// </summary>
        /// <param name="riverName">the river's name</param>
        /// <param name="upDown">flag indicating the river's flow</param>
        private void LoadRiver(String riverName, bool upDown)
        {
            RiverGraphNode[] river = new RiverGraphNode[0];
            for (int i = 0, len = riverNodes.Length; i < len; i++)
            {
                RiverGraphNode node = riverNodes[i];
                if (node.Type == RiverGraphNode.RIVER_BANK)
                {
                    node = null;
                    continue;
                }
                if (string.Equals(riverName, node.Name, StringComparison.OrdinalIgnoreCase))
                {
                    river = ArrayUtilities.Instance.ExtendArray(node, river);
                }
                node = null;
            }
            if (upDown)
            {
                for (int i = 0, len = river.Length; i < len; i++)
                {
                    if (i + 1 < len)
                    {
                        riverGraph.AddEdge(river[i].Index, river[i + 1].Index, UP_RIVER);
                    }
                }
                for (int i = river.Length - 1; i >= 0; i--)
                {
                    if (i - 1 >= 0)
                    {
                        riverGraph.AddEdge(river[i].Index, river[i - 1].Index, DOWN_RIVER);
                    }
                }
            }
            else
            {
                for (int i = 0, len = river.Length; i < len; i++)
                {
                    if (i + 1 < len)
                    {
                        riverGraph.AddEdge(river[i].Index, river[i + 1].Index, DOWN_RIVER);
                    }
                }
                for (int i = river.Length - 1; i >= 0; i--)
                {
                    if (i - 1 >= 0)
                    {
                        riverGraph.AddEdge(river[i].Index, river[i - 1].Index, UP_RIVER);
                    }
                }
            }
            river = null;
        }
        /// <summary>
        /// Loads all nodes for the Tragoth River.
        /// </summary>
        private void LoadTragothRiver()
        {
            RiverGraphNode[] river = new RiverGraphNode[0];
            for (int i = 0, len = riverNodes.Length; i < len; i++)
            {
                RiverGraphNode node = riverNodes[i];
                if (node.Type == RiverGraphNode.RIVER_BANK)
                {
                    node = null;
                    continue;
                }
                if (string.Equals("Tragoth River", node.Name, StringComparison.OrdinalIgnoreCase))
                {
                    river = ArrayUtilities.Instance.ExtendArray(node, river);
                }
                node = null;
            }
            Array.Sort(river, new TragothRiverComparator(this));
            for (int i = 0, len = river.Length; i < len; i++)
            {
                if (i + 1 < len)
                {
                    riverGraph.AddEdge(river[i].Index, river[i + 1].Index, UP_RIVER);
                }
            }
            for (int i = river.Length - 1; i >= 0; i--)
            {
                if (i - 1 >= 0)
                {
                    riverGraph.AddEdge(river[i].Index, river[i - 1].Index, DOWN_RIVER);
                }
            }
            river = null;
        }
        /// <summary>
        /// Converts a <see cref="Hex"/> to a string to be displayed in the UI Location table.
        /// </summary>
        /// <param name="id">the <see cref="Hex"/>'s reference id</param>
        /// <returns><see cref="string"/></returns>
        public String ToUITableString(int id)
        {
            Hex hex = GetHexById(id);
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            try
            {
                sb.Append(hex.LocationName);
                sb.Append(" (");
                sb.Append((int)hex.Location.x);
                sb.Append(", ");
                sb.Append((int)hex.Location.y);
                sb.Append(")");
                sb.Append('\n');
                if (!string.Equals(hex.LocationName, hex.Type.Title, StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append(hex.Type.Title);
                }
                else
                {
                    sb.Append(' ');
                }
            }
            catch (PooledException e)
            {
                throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            hex = null;
            return s;
        }
    }
    class TragothRiverComparator : IComparer<RiverGraphNode>
    {
        /// <summary>
        /// the hex map instance used for getting hex locations.
        /// </summary>
        private HexMap m;
        /**
         * Creates a new instance of {@link TragothRiverComparator}.
         * @param hm the hex map instance 
         */
        public TragothRiverComparator(HexMap hm)
        {
            m = hm;
        }
        public int Compare(RiverGraphNode arg0, RiverGraphNode arg1)
        {
            int compare = 0;
            if (arg0.Type == RiverGraphNode.RIVER_RAFT
                    && arg1.Type == RiverGraphNode.RIVER_RAFT)
            {
                Hex bank00 = m.GetHexById(arg0.Bank0);
                Hex bank01 = m.GetHexById(arg0.Bank1);
                Hex bank10 = m.GetHexById(arg1.Bank0);
                Hex bank11 = m.GetHexById(arg1.Bank1);
                if (bank00.Location.x
                        < bank10.Location.x)
                {
                    compare = -1;
                }
                else if (bank00.Location.x
                      == bank10.Location.x)
                {
                    if (bank00.Location.y
                            < bank10.Location.y)
                    {
                        compare = -1;
                    }
                    else if (bank00.Location.y
                          == bank10.Location.y)
                    {
                        if (bank01.Location.x
                                < bank11.Location.x)
                        {
                            compare = -1;
                        }
                        else if (bank01.Location.x
                              == bank11.Location.x)
                        {
                            if (bank01.Location.y
                                    < bank11.Location.y)
                            {
                                compare = -1;
                            }
                            else if (bank01.Location.y
                                  == bank11.Location.y)
                            {
                                compare = 0;
                            }
                        }
                        else
                        {
                            compare = 1;
                        }
                    }
                    else
                    {
                        compare = 1;
                    }
                }
                else
                {
                    compare = 1;
                }
            }
            return compare;
        }
    }
}

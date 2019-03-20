using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Graph;
using RPGBase.Pooled;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WoFM.Constants;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class WorldController : Singleton<WorldController>
    {
        /// <summary>
        /// the map graph.
        /// </summary>
        private EdgeWeightedUndirectedGraph dungeonGraph;
        /// <summary>
        /// map data that gets loaded before the game plays.
        /// </summary>
        private List<MapData> mapData;
        /// <summary>
        /// Transform to hold all our game objects, so they don't clog up the hierarchy.
        /// </summary>
        private Transform tileHolder;
        /// <summary>
        /// The map of tile objects used to display the game board
        /// </summary>
        private Dictionary<string, GameObject> tileObjects;
        /// <summary>
        /// the dimensions for the number of tiles that can fit in the viewport.
        /// </summary>
        private Vector2 viewportDimensions;
        /// <summary>
        /// The game world.
        /// </summary>
        private WoFMTileWorld world;
        #region Map METHODS
        /// <summary>
        /// the field-of-view instance
        /// </summary>
        private ShadowcastFOV shadowcaster;
        private void RunLightingChecks()
        {
            // reset the shadows
            world.ResetShadows();
            // check all quadrants on player's position
            shadowcaster.CheckQuadrant1(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant2(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant3(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant4(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant5(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant6(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant7(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
            shadowcaster.CheckQuadrant8(((WoFMInteractive)Interactive.Instance).GetPlayerIO().LastPositionHeld, 5);
        }
        private void DisplayDoors(int minx, int maxx, int miny, int maxy, float now)
        {
            foreach (Transform child in GameController.Instance.doorHolder)
            {
                //child is your child transform
                WoFMInteractiveObject io = child.gameObject.GetComponent<WoFMInteractiveObject>();
                float iox = io.Script.GetLocalFloatVariableValue("x"), ioy = io.Script.GetLocalFloatVariableValue("y");
                int room = io.Script.GetLocalIntVariableValue("room");
                if (iox >= minx
                    && iox < maxx
                    && ioy >= miny
                    && ioy < maxy
                    && GameSceneController.Instance.WasRoomVisited(io.Script.GetLocalIntVariableValue("room")))
                {
                    // Door is in viewport
                    child.position = TileViewportController.Instance.GetScreenCoordinatesForWorldPosition(new Vector2(iox, ioy));
                    if (((WoFMTile)world.GetTileAt((int)iox, (int)ioy)).ShadeLevel == ShadowcastFOV.UNSCANNED)
                    {
                        io.GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else
                    {
                        io.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                else
                {
                    // Door is off-screen. move outside view
                    child.position = new Vector3(-1, 0, 0);
                }
            }
        }
        /// <summary>
        /// Displays the game board.
        /// </summary>
        public void DisplayMap()
        {
            //print("DisplayMap");
            RunLightingChecks();
            // get current time for animations
            float now = (Time.time - RPGTime.Instance.TimePaused) * 1000f;
            // get the viewport's range.
            Vector2 v = TileViewportController.Instance.ViewportPosition;
            int minx = Mathf.FloorToInt(v.x);
            int miny = Mathf.FloorToInt(v.y);
            int maxx = minx + (int)viewportDimensions.x;
            int maxy = miny + (int)viewportDimensions.y;
            //print("viewport at " + v.x + "," + v.y + "\nrange from " + minx + "," + miny + " to " + maxx+ "," + maxy);
            // iterate through tiles in viewport's range to set the onscreen sprites
            DisplayTiles(minx, maxx, miny, maxy, now);
            DisplayDoors(minx, maxx, miny, maxy, now);
            DisplayMobs(minx, maxx, miny, maxy, now);
        }
        /// <summary>
        /// Displays all mobs that are in the viewport.
        /// </summary>
        /// <param name="minx">the viewport's minimum x-coordinate</param>
        /// <param name="maxx">the viewport's maximum x-coordinate</param>
        /// <param name="miny">the viewport's minimum y-coordinate</param>
        /// <param name="maxy">the viewport's maximum y-coordinate</param>
        /// <param name="now">the current frame time</param>
        private void DisplayMobs(int minx, int maxx, int miny, int maxy, float now)
        {
            foreach (Transform child in GameController.Instance.mobHolder)
            {
                // child is your child transform
                WoFMInteractiveObject io = child.gameObject.GetComponent<WoFMInteractiveObject>();
                // check position of child's IO instance
                if (io.Position.x >= minx
                    && io.Position.x < maxx
                    && io.Position.y >= miny
                    && io.Position.y < maxy)
                {
                    // Mob is in viewport. move child transform to correct screen position
                    child.position = TileViewportController.Instance.GetScreenCoordinatesForWorldPosition(io.Position);
                    if (((WoFMTile)world.GetTileAt(io.Position)).ShadeLevel == ShadowcastFOV.UNSCANNED)
                    {
                        Script.Instance.SendIOScriptEvent(io, WoFMGlobals.SM_304_OUT_OF_VIEW, null, null);
                    }
                    else
                    {
                        io.GetComponent<SpriteRenderer>().color = Color.white;
                        Script.Instance.SendIOScriptEvent(io, WoFMGlobals.SM_305_IN_VIEW, null, null);
                    }
                }
                else
                {
                    // Mob is off-screen. move child transform outside view
                    child.position = new Vector3(-1, 0, 0);
                    Script.Instance.SendIOScriptEvent(io, WoFMGlobals.SM_304_OUT_OF_VIEW, null, null);
                }
            }
        }
        private void DisplayTiles(int minx, int maxx, int miny, int maxy, float now)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            for (int x = minx, lx = maxx; x < lx; x++)
            {
                for (int y = miny, ly = maxy; y < ly; y++)
                {
                    int tileXIndex = x - minx, tileYIndex = y - miny;
                    // tile object being used is offset for the viewport area
                    // so if the viewport is at 2,3, then tile object 0_0 would represent world tile 2,3, tile object 1_0 would be world tile 3,3, etc...
                    sb.Append("Tile_");
                    sb.Append(tileXIndex);
                    sb.Append("_");
                    sb.Append(tileYIndex);
                    // world tile position is represented by x,y
                    // tile object id is x-minx, y-miny (adjusting id for viewport)
                    try
                    {
                        GameObject tileObject = tileObjects[sb.ToString()];
                        // adjust tile position based on viewport
                        tileObject.transform.position = TileViewportController.Instance.GetScreenCoordinatesForWorldPosition(new Vector2(x, y));
                        WoFMTile tile = (WoFMTile)world.GetTileAt(x, y);
                        if (tile != null)
                        {
                            if (tile.ShadeLevel == ShadowcastFOV.UNSCANNED)
                            {
                                tileObject.GetComponent<SpriteRenderer>().color = Color.gray;
                            }
                            else
                            {
                                tileObject.GetComponent<SpriteRenderer>().color = Color.white;
                            }
                            bool animatedTile = false;
                            // TODO - check for tile needing animation
                            // set the tile sprite based on underlying data
                            if (animatedTile)
                            {
                                // animate tiles based on type. example below is for rivers
                                int qrtr = (int)now % 1000;
                                if (qrtr >= 500)
                                {
                                    tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("water_0");
                                }
                                else
                                {
                                    tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("water_1");
                                }
                            }
                            else
                            {
                                if (GameSceneController.Instance.WasAtLeastOneRoomVisited(tile.Rooms))
                                {
                                    tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite(tile.Type.ToString().ToLower());
                                    if (tile.Type == Tile.TerrainType.floor_0
                                        || tile.Type == Tile.TerrainType.floor_1
                                        || tile.Type == Tile.TerrainType.floor_2
                                        || tile.Type == Tile.TerrainType.tombstone
                                        || tile.Type == Tile.TerrainType.corpse
                                        || tile.Type == Tile.TerrainType.pit)
                                    {
                                        tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
                                        tileObject.layer = LayerMask.NameToLayer("Floor");
                                    }
                                    else if (tile.Type == Tile.TerrainType.wall_0
                                       || tile.Type == Tile.TerrainType.wall_1)
                                    {
                                        tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Units";
                                        tileObject.layer = LayerMask.NameToLayer("BlockingLayer");
                                        tileObject.GetComponent<Blocker>().Type = Blocker.WALL;
                                    }
                                }
                                else
                                {
                                    tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("void");
                                }
                            }
                        }
                    }
                    catch (KeyNotFoundException knfe)
                    {
                        print(knfe.ToString());
                        print(sb.ToString() + "::" + tileObjects.ContainsKey(sb.ToString()));
                    }
                    sb.Length = 0;
                }
            }
            sb.ReturnToPool();
        }
        /// <summary>
        /// Gets the tile at a specific location.  If no tile exists, null is returned.
        /// </summary>
        /// <param name="x">the x-coordinate</param>
        /// <param name="y">the y-coordinate</param>
        /// <returns><see cref="WoFMTile"/></returns>
        public WoFMTile GetTileAt(int x, int y)
        {
            return (WoFMTile)world.GetTileAt(x, y);
        }
        /// <summary>
        /// Gets the tile at a specific location.  If no tile exists, null is returned.
        /// </summary>
        /// <param name="v">the coordinates</param>
        /// <returns><see cref="WoFMTile"/></returns>
        public WoFMTile GetTileAt(Vector2 v)
        {
            return GetTileAt((int)v.x, (int)v.y);
        }
        /// <summary>
        /// Initializes the board.
        /// </summary>
        private void InitMap()
        {
            mapData = GameController.Instance.LoadMap();
            int maxX = 0, maxY = 0;
            for (int i = mapData.Count - 1; i >= 0; i--)
            {
                maxX = Math.Max(maxX, (int)mapData[i].coordinates.x);
                maxY = Math.Max(maxY, (int)mapData[i].coordinates.y);
            }
            maxX++;
            maxY++;
            // create world
            world = new WoFMTileWorld(maxX, maxY);
            print("world is " + maxX + "," + maxY);
            TileViewportController.Instance.MaxY = world.Height;
            TileViewportController.Instance.MaxX = world.Width;
            // create object to hold game tiles
            tileHolder = new GameObject("Board").transform;
            // create map to hold references to all game tiles
            tileObjects = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
            //****************************
            // CREATE GameObjects TILES
            //****************************
            // viewport dimensions should be 46x22.
            // game board takes up 75% screen width, 50% screen height, which is
            // 48x24 units (set in orthographic camera properties)
            TileViewportController.Instance.ViewportTileDimensions = new Vector2(48, 24);
            viewportDimensions = TileViewportController.Instance.RequiredTileDimensions;
            print("viewportDimensions::" + viewportDimensions);
            // y-offset is 24 units up, because game board is displayed in top half of screen
            float xOffset = 1f, yOffset = 25f;
            // loop through viewport tiles, creating GameObjects to hold sprites
            // for displaying the game board
            for (int x = (int)viewportDimensions.x - 1; x >= 0; x--)
            {
                for (int y = (int)viewportDimensions.y - 1; y >= 0; y--)
                {
                    GameObject tileObject = new GameObject
                    {
                        name = "Tile_" + x + "_" + y
                    };
                    /********************************************************
                     * SETUP BLOCKER COMPONENT
                    /*******************************************************/
                    // every tile is a potential blocker
                    Blocker b = tileObject.AddComponent<Blocker>();
                    b.Type = Blocker.VOID;
                    /********************************************************
                     * SETUP BOX_COLLIDER_2D COMPONENT
                    /*******************************************************/
                    BoxCollider2D bc = tileObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
                    bc.size = new Vector2(1f, 1f);
                    /********************************************************
                     * SETUP SPRITE_RENDERER COMPONENT
                    /*******************************************************/
                    // every tile needs a SpriteRenderer
                    tileObject.AddComponent<SpriteRenderer>();
                    // set sprite initially to void (white)
                    tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("void_white");
                    // set the sprite's sorting layer to floor
                    tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
                    /********************************************************
                     * FINISH SETUP
                    /*******************************************************/
                    // add tile to list of tiles
                    tileObjects.Add(tileObject.name, tileObject);
                    // position tile 
                    tileObject.transform.position = new Vector3(x + xOffset, y + yOffset, 0);
                    // set new tile as child of tile holder
                    tileObject.transform.SetParent(tileHolder);
                }
            }
            // SET TILE DATA
            for (int i = mapData.Count - 1; i >= 0; i--)
            {
                MapData data = mapData[i];
                WoFMTile tile = (WoFMTile)world.GetTileAt((int)data.coordinates.x, (int)data.coordinates.y);
                if (string.Equals(data.type, "cave_wall", StringComparison.OrdinalIgnoreCase))
                {
                    tile.Type = Tile.TerrainType.wall_0;
                }
                else if (string.Equals(data.type, "pit", StringComparison.OrdinalIgnoreCase))
                {
                    tile.Type = Tile.TerrainType.pit;
                }
                else if (string.Equals(data.type, "floor", StringComparison.OrdinalIgnoreCase))
                {
                    switch (Diceroller.Instance.RollXdY(3, 6))
                    {
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            tile.Type = Tile.TerrainType.floor_2;
                            break;
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            tile.Type = Tile.TerrainType.floor_1;
                            break;
                        default:
                            tile.Type = Tile.TerrainType.floor_0;
                            break;
                    }
                }
                tile.Rooms = data.roomId;
            }
            InitGraph();
        }
        /// <summary>
        /// Spawns an audible sound to which NPC IOs can react.
        /// </summary>
        /// <param name="tileCoords">the world position where the sound happens</param>
        /// <param name="srcIo">the source IO</param>
        public void SpawnAudibleSound(Vector2 pos, WoFMInteractiveObject srcIo)
        {
            WoFMTile sourceTile = GetTileAt(pos);
            // go through mobs.  If they are in the same room, send a hear event
            foreach (Transform child in GameController.Instance.mobHolder)
            {
                WoFMInteractiveObject childIo = child.gameObject.GetComponent<WoFMInteractiveObject>();
                if (childIo.HasIOFlag(IoGlobals.IO_03_NPC)
                    && childIo.RefId != srcIo.RefId
                    && childIo.NpcData.Life > 0)
                {
                    WoFMTile ioTile = GetTileAt(childIo.LastPositionHeld);
                    WoFMTile noiseTile = GetTileAt(pos);
                    if (ArrayUtilities.Instance.ArrayContainsOneElement(ioTile.Rooms, noiseTile.Rooms))
                    {
                        Script.Instance.EventSender = srcIo;
                        Script.Instance.SendIOScriptEvent(childIo, ScriptConsts.SM_046_HEAR, null, null);
                    }
                }
            }
        }
        #endregion
        #region Graph METHODS
        /// <summary>
        /// Gets the path from the source location to the destination.
        /// </summary>
        /// <param name="source">the source coordinates</param>
        /// <param name="destination">the destination coordinates</param>
        /// <returns></returns>
        public WeightedGraphEdge[] GetLandPath(Vector2 source, Vector2 destination)
        {
            int srcNode = -1, destNode = -1;
            for (int i = graphNodes.Length - 1; i >= 0; i--)
            {
                if (graphNodes[i].Location == source)
                {
                    srcNode = graphNodes[i].Index;
                }
                if (graphNodes[i].Location == destination)
                {
                    destNode = graphNodes[i].Index;
                }
                if (srcNode >= 0 && destNode >= 0)
                {
                    break;
                }
            }
            return GetLandPath(srcNode, destNode);
        }
        /// <summary>
        /// Gets the path from the source cell to a destination.
        /// </summary>
        /// <param name="source">source the source location</param>
        /// <param name="destination">the destination</param>
        /// <returns><see cref="WeightedGraphEdge"/>[]</returns>
        public WeightedGraphEdge[] GetLandPath(int source, int destination)
        {
            // perform a dijkstra search
            DijkstraUndirectedSearch dijkstra = new DijkstraUndirectedSearch(dungeonGraph, source);
            WeightedGraphEdge[] path = dijkstra.pathTo(destination);
            // re-order the path. sometimes the path edges do not match up (edge 1 will go from node 2->3, while edge 2 will go from node 5->3.  edge 2 should go from 3->5)
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (i == path.Length - 1)
                {
                    //print("reversing first path " + EdgeToCoordinatesString(path[i]));
                    // first node. check to see if source is from
                    if (path[i].To == source)
                    {
                        path[i] = new WeightedGraphEdge(source, path[i].From, path[i].Cost);
                    }
                }
                else if (i + 1 == 0)
                {
                    // last node. check to see if source is from
                    if (path[i].From == destination)
                    {
                        path[i] = new WeightedGraphEdge(path[i].To, destination, path[i].Cost);
                    }
                }
                else
                {
                    // middle node.  check to see if out of order
                    if (path[i].From != path[i + 1].To
                        || path[i].To == path[i + 1].To)
                    {
                        // reverse the order
                        path[i] = new WeightedGraphEdge(path[i].To, path[i].From, path[i].Cost);
                    }
                }
            }
            dijkstra = null;
            return path;
        }
        public string EdgeToCoordinatesString(WeightedGraphEdge edge)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(GetNodeCoordinatesFromId(edge.From));
            sb.Append(" - ");
            sb.Append(GetNodeCoordinatesFromId(edge.To));
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public Vector2 GetNodeCoordinatesFromId(int nodeId)
        {
            Vector2 v = new Vector2(-1, -1);
            for (int i = graphNodes.Length - 1; i >= 0; i--)
            {
                if (graphNodes[i].Index == nodeId)
                {
                    v = graphNodes[i].Location;
                    break;
                }
            }
            return v;
        }
        /// <summary>
        /// the list of <see cref="PhysicalGraphNode"/>s in the graph.
        /// </summary>
        private PhysicalGraphNode[] graphNodes;
        /// <summary>
        /// Initializes the dungeon graph.
        /// </summary>
        private void InitGraph()
        {
            dungeonGraph = new EdgeWeightedUndirectedGraph(0);
            graphNodes = new PhysicalGraphNode[0];
            for (int i = mapData.Count - 1; i >= 0; i--)
            {
                MapData d = mapData[i];
                PhysicalGraphNode node = new PhysicalGraphNode(i, d.coordinates);
                graphNodes = ArrayUtilities.Instance.ExtendArray(node, graphNodes);
                dungeonGraph.AddVertex(node);
            }
            // create edges between all cells
            for (int i = graphNodes.Length - 1; i >= 0; i--)
            {
                PhysicalGraphNode node = graphNodes[i];
                Tile orig = world.GetTileAt(node.Location);
                if (orig.Type != Tile.TerrainType.floor_0
                    && orig.Type != Tile.TerrainType.floor_1
                    && orig.Type != Tile.TerrainType.floor_2
                    && orig.Type != Tile.TerrainType.tombstone
                    && orig.Type != Tile.TerrainType.corpse)
                {
                    // no need to create edge for walls or void spaces
                    continue;
                }
                // connect north
                int otherX = (int)node.Location.x, otherY = (int)node.Location.y + 1;
                Tile other = world.GetTileAt(otherX, otherY);
                if (other != null
                    && (other.Type == Tile.TerrainType.floor_0
                    || other.Type == Tile.TerrainType.floor_1
                    || other.Type == Tile.TerrainType.floor_2))
                {
                    dungeonGraph.AddEdge(node.Index, GetNode(new Vector2(otherX, otherY)).Index);
                    //print("edge created: " + node.Location + "-" + GetNode(new Vector2(otherX, otherY)).Location);
                }
                // connect east
                otherX = (int)node.Location.x + 1;
                otherY = (int)node.Location.y;
                other = world.GetTileAt(otherX, otherY);
                if (other != null
                    && (other.Type == Tile.TerrainType.floor_0
                    || other.Type == Tile.TerrainType.floor_1
                    || other.Type == Tile.TerrainType.floor_2
                    || other.Type == Tile.TerrainType.tombstone
                    || other.Type == Tile.TerrainType.corpse))
                {
                    dungeonGraph.AddEdge(node.Index, GetNode(new Vector2(otherX, otherY)).Index);
                    //print("edge created: " + node.Location + "-" + GetNode(new Vector2(otherX, otherY)).Location);
                }
                // connect south
                otherX = (int)node.Location.x;
                otherY = (int)node.Location.y - 1;
                other = world.GetTileAt(otherX, otherY);
                if (other != null
                    && (other.Type == Tile.TerrainType.floor_0
                    || other.Type == Tile.TerrainType.floor_1
                    || other.Type == Tile.TerrainType.floor_2
                    || other.Type == Tile.TerrainType.tombstone
                    || other.Type == Tile.TerrainType.corpse))
                {
                    dungeonGraph.AddEdge(node.Index, GetNode(new Vector2(otherX, otherY)).Index);
                    //print("edge created: " + node.Location + "-" + GetNode(new Vector2(otherX, otherY)).Location);
                }
                // connect west
                otherX = (int)node.Location.x - 1;
                otherY = (int)node.Location.y;
                other = world.GetTileAt(otherX, otherY);
                if (other != null
                    && (other.Type == Tile.TerrainType.floor_0
                    || other.Type == Tile.TerrainType.floor_1
                    || other.Type == Tile.TerrainType.floor_2
                    || other.Type == Tile.TerrainType.tombstone
                    || other.Type == Tile.TerrainType.corpse))
                {
                    dungeonGraph.AddEdge(node.Index, GetNode(new Vector2(otherX, otherY)).Index);
                    //print("edge created: " + node.Location + "-" + GetNode(new Vector2(otherX, otherY)).Location);
                }
            }
        }
        private PhysicalGraphNode GetNode(Vector2 v)
        {
            PhysicalGraphNode node = null;
            for (int i = graphNodes.Length - 1; i >= 0; i--)
            {
                if (graphNodes[i].Location == v)
                {
                    node = graphNodes[i];
                    break;
                }
            }
            return node;
        }
        #endregion
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // print("++++++++++++++++++++++WorldController awake");
            shadowcaster = new ShadowcastFOV();
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            // print("********************WorldController start");
            // init singletons
            // MouseListener ml = MouseListener.Instance;
            TileViewportController vc = TileViewportController.Instance;
            // IS THE MAP LOADED?
            InitMap();
            // reposition camera to 0,0
            TileViewportController.Instance.PositionViewport(new Vector2(0, 0));
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // print("-------------------------WorldController update");          
            DisplayMap();
            /*
            float xOffset = 638, yOffset = 1341;
            TileViewportController.Instance.GetWorldCoordinatesForTile(new Vector2(641 - xOffset, yOffset - 1340));
            */
        }
        #endregion
    }
}

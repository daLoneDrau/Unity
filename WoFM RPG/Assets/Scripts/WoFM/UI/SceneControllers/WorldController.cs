using RPGBase.Pooled;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class WorldController : Singleton<WorldController>
    {
        /// <summary>
        /// The game world.
        /// </summary>
        private TileWorld world;
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
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            // init singletons
            // MouseListener ml = MouseListener.Instance;
            ViewportController vc = ViewportController.Instance;
            // IS THE MAP LOADED?
            InitMap();
            // reposition camera to 0,0
            ViewportController.Instance.PositionViewport(new Vector2(0, 0));
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            DisplayMap();
        }
        #endregion
        private List<MapData> mapData;
        /// <summary>
        /// Displays the game board.
        /// </summary>
        public void DisplayMap()
        {
            // get current time for animations
            float now = (Time.time - RPGTime.Instance.TimePaused) * 1000f;
            // get the viewport's range.
            Vector2 v = ViewportController.Instance.ViewportPosition;
            ViewportController.Instance.CenterOnPoint(new Vector2(0, 0));
            print("viewport at " + v);
            int minx = Mathf.FloorToInt(v.x);
            int miny = Mathf.FloorToInt(v.y);
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            // iterate through tiles in viewport's range to set the onscreen sprites
            // get the fractional part of the viewport's position. if viewport is at 0.5,10.3, then dx=-.5, dy=-.3
            float dx = v.x - (float)Math.Truncate(v.x);
            float dy = v.y - (float)Math.Truncate(v.y);
            dx *= -1;
            dy *= -1;
            int xOffset = 1, yOffset = 25;
            for (int x = minx, lx = minx + (int)viewportDimensions.x; x < lx; x++)
            {
                for (int y = miny, ly = miny + (int)viewportDimensions.y; y < ly; y++)
                {
                    sb.Append("Tile_");
                    sb.Append(x - minx);
                    sb.Append("_");
                    sb.Append(y - miny);
                    try
                    {
                        GameObject tileObject = tileObjects[sb.ToString()];
                        // adjust tile position based on viewport
                        tileObject.transform.position = new Vector3(x - minx + dx + xOffset, y - miny + dy + yOffset, 0);
                        Tile tile = world.GetTileAt(x, y);
                        if (tile != null)
                        {
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
                                tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite(tile.Type.ToString().ToLower());
                                print("tile " + sb.ToString() + " set to " + tile.Type.ToString());
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
            world = new TileWorld(maxX, maxY);
            print("world is " + maxX + "," + maxY);
            ViewportController.Instance.MaxY = world.Height;
            ViewportController.Instance.MaxX = world.Width;
            // create object to hold game tiles
            tileHolder = new GameObject("Board").transform;
            // create map to hold references to all game tiles
            tileObjects = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
            //****************************
            // CREATE GameObjects TILES
            //****************************
            // viewport dimensions should be 46x22.
            // game board takes up 75% screen width, 50% screen height, which is
            // 48x24 units (set in orthographic camera properties). an additional 2 units taken off for a decorative border
            ViewportController.Instance.ViewportTileDimensions = new Vector2(46, 22);
            viewportDimensions = ViewportController.Instance.RequiredTileDimensions;
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
                    tileObjects.Add(tileObject.name, tileObject);
                    tileObject.transform.position = new Vector3(x + xOffset, y + yOffset, 0);
                    tileObject.AddComponent<SpriteRenderer>();
                    // set sprite initially to void
                    tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("void_white");
                    tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
                    // set new tile as child of tile holder
                    tileObject.transform.SetParent(tileHolder);
                }
            }
            // SET TILE DATA
            for (int i = mapData.Count - 1; i >= 0; i--)
            {
                MapData data = mapData[i];
                Tile tile = world.GetTileAt((int)data.coordinates.x, (int)data.coordinates.y);
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
            }
        }
    }
}

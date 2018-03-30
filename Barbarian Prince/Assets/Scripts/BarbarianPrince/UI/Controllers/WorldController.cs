using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.BarbarianPrince.Graph;
using Assets.Scripts.BarbarianPrince.Singletons;
using Assets.Scripts.BarbarianPrince.UI.Controllers;
using Assets.Scripts.RPGBase.Graph;
using Assets.Scripts.RPGBase.Singletons;
using Assets.Scripts.UI;
using RPGBase.Pooled;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WorldController : Singleton<WorldController>
{
    private const int N_EDGE = 1;
    private const int NNE_EDGE = 2;
    private const int SSE_EDGE = 4;
    private const int S_EDGE = 8;
    private const int SSW_EDGE = 16;
    private const int NNW_EDGE = 32;
    private World world;
    /// <summary>
    /// Transform to hold all our game objects, so they don't clog up the hierarchy.
    /// </summary>
    private Transform tileHolder;
    /// <summary>
    /// The map of tile objects used to display the game board
    /// </summary>
    private Dictionary<string, GameObject> tileObjects;
    private GameObject heroTile;
    private GameObject possibleMove;
    // Use this for initialization
    private Hex[] hexList;
    private RiverCrossing[] crossings;
    private int[][] roads;
    private bool doonce = false;
    /// <summary>
    /// the dimensions for the number of tiles that can fit in the viewport.
    /// </summary>
    private Vector2 viewportDimensions;
    void Awake()
    {
        GameController.Instance.StartLoad(GameController.STATE_START_MENU);
        // init singletons
        MouseListener ml = MouseListener.Instance;
        ViewportController vc = ViewportController.Instance;
        // load resources
        LoadResources();
        // load hex tiles
        StartCoroutine(BPServiceClient.Instance.GetAllHexes(value => hexList = value));
        // load river crossings
        StartCoroutine(BPServiceClient.Instance.GetAllRiverCrossings(value => crossings = value));
        // load roads
        StartCoroutine(BPServiceClient.Instance.GetAllRoads(value => roads = value));
        // reposition camera to 0,0
        ViewportController.Instance.PositionViewport(new Vector2(0, 0));
    }
    /// <summary>
    /// Initializes the board.
    /// </summary>
    private void InitMap()
    {
        HexMap.Instance.Hexes = hexList;
        HexMap.Instance.RiverCrossings = crossings;
        HexMap.Instance.Roads = roads;
        HexMap.Instance.Load();
        // create world
        world = new World();
        ViewportController.Instance.MaxY = world.Height;
        ViewportController.Instance.MaxX = world.Width;
        // create object to hold game tiles
        tileHolder = new GameObject("Board").transform;
        // create map to hold references to all game tiles
        tileObjects = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
        // create game tiles
        viewportDimensions = ViewportController.Instance.RequiredTileDimensions;
        for (int x = (int)viewportDimensions.x - 1; x >= 0; x--)
        {
            for (int y = (int)viewportDimensions.y - 1; y >= 0; y--)
            {
                Tile tileData = world.GetTileAt(x, y);
                GameObject tileObject = new GameObject
                {
                    name = "Tile_" + x + "_" + y
                };
                tileObjects.Add(tileObject.name, tileObject);
                tileObject.transform.position = new Vector3(x, y, 0);
                tileObject.AddComponent<SpriteRenderer>();
                // set sprite initially to void
                tileObject.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("void");
                tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
                // set new tile as child of tile holder
                tileObject.transform.SetParent(tileHolder);
            }
        }
        heroTile = new GameObject
        {
            name = "Hero"
        };
        heroTile.transform.position = new Vector3(-1, 0, 0);
        heroTile.AddComponent<SpriteRenderer>();
        heroTile.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("Hero");
        heroTile.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
        heroTile.transform.SetParent(tileHolder);
        /*
        print("hexes loaded " + hexList.Length);
        print("world tiles dimensions - " + world.Width + "x" + world.Height);
        print("viewport dimensions - " + viewportDimensions.x + "x" + viewportDimensions.y);
        */
        SetTileTypes();
        doonce = true;
        GameController.Instance.StopLoad();
    }
    private Vector2 GetHexTilePosition(Vector2 hexPosition)
    {
        // find hex's bottom-left corner
        // hexes are 4 rows high
        int miny = 23 - (int)hexPosition.y;
        miny *= 4;
        if (hexPosition.x % 2 == 1)
        {
            miny += 2;
        }
        int minx = ((int)hexPosition.x - 1) * 5;
        return new Vector2(minx, miny);
    }
    /// <summary>
    /// Centers the viewport on a specific hex.
    /// </summary>
    /// <param name="position">the hex</param>
    public void CenterOnHex(Vector2 position)
    {
        Vector2 hexBottomLeft = GetHexTilePosition(position);
        float midx = (float)hexBottomLeft.x+ 2.5f;
        float midy = (float)hexBottomLeft.y+ 1.5f;
        print("hex " + position + " goes from " + hexBottomLeft.x + "," + hexBottomLeft.y + " to " + (hexBottomLeft.x+5) + "," + (hexBottomLeft.y+3) + " mid at " + midx + "," + midy);
        ViewportController.Instance.CenterOnPoint(new Vector2(midx,midy));
    }
    public void DrawHero()
    {
        float now = (Time.time - RPGTime.Instance.TimePaused) * 1000f;
        // get the viewport's range.
        Vector2 v = ViewportController.Instance.ViewportPosition;
        int minx = Mathf.FloorToInt(v.x);
        int miny = Mathf.FloorToInt(v.y);
        float dx = v.x - (float)Math.Truncate(v.x);
        float dy = v.y - (float)Math.Truncate(v.y);
        dx *= -1;
        dy *= -1;
        BPInteractiveObject io = ((BPInteractive)BPInteractive.Instance).GetPlayerIO();
        // player is in hex. find out if it's in view
        Vector2 playerTilePosition = GetHexTilePosition(io.Position);
        playerTilePosition += new Vector2(4, 1);
        if (playerTilePosition.x >=minx
            && playerTilePosition.x <= minx + viewportDimensions.x
            && playerTilePosition.y >= miny
            && playerTilePosition.y <= miny + viewportDimensions.y)
        {
            // player is visible
            // adjust tile position based on viewport
            heroTile.transform.position = new Vector3(playerTilePosition.x - minx + dx, playerTilePosition.y - miny + dy, 0);
        }
        else
        {
            heroTile.transform.position = new Vector3(-1, 0, 0);
        }
    }
    /// <summary>
    /// Displays the game board.
    /// </summary>
    public void DisplayMap()
    {
        float now = (Time.time-RPGTime.Instance.TimePaused) * 1000f;
        // get the viewport's range.
        Vector2 v = ViewportController.Instance.ViewportPosition;
        int minx = Mathf.FloorToInt(v.x);
        int miny = Mathf.FloorToInt(v.y);
        PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
        // iterate through tiles in viewport's range to set the onscreen sprites
        float dx = v.x - (float)Math.Truncate(v.x);
        float dy = v.y - (float)Math.Truncate(v.y);
        dx *= -1;
        dy *= -1;
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
                    tileObject.transform.position = new Vector3(x - minx + dx, y - miny + dy, 0);
                    Tile tile = world.GetTileAt(x, y);
                    if (tile != null)
                    {
                        // set the tile sprite based on underlying data
                        if (tile.Type == Tile.TerrainType.River_a || tile.Type == Tile.TerrainType.River_b || tile.Type == Tile.TerrainType.River_c || tile.Type == Tile.TerrainType.River_d)
                        {
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
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (hexList != null
                && crossings != null
                && roads != null
                && !doonce)
        {
            InitMap();
        }
    }
    /// <summary>
    /// Loads XML resources.
    /// </summary>
    private void LoadResources()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("config");
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);
        XmlNode root = xmldoc.SelectSingleNode("endpoints");
        //print(root.SelectSingleNode("bp_endpoint").InnerText);
        BPServiceClient.Instance.Endpoint = root.SelectSingleNode("bp_endpoint").InnerText;
        //print(BPServiceClient.Instance.Endpoint);
    }
    private void SetTileSprites(Hex hex, string terrain, int minx, int miny, int sameEdge, int dsrtedge, int voidedge, int roadEdge, int rvrEdge, bool fourPattern)
    {
        PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
        // HEX LAYOUT
        //   abab
        //  cdcdcd/bababa
        //  ababab
        //   baba/cdcd
        // DO EDGES FIRST
        //******************
        //*** NORTH - abab
        // 1,3
        if ((rvrEdge & NNW_EDGE) == NNW_EDGE
        || ((voidedge & NNW_EDGE) == NNW_EDGE && (rvrEdge & N_EDGE) == N_EDGE))
        {
            world.GetTileAt(minx + 1, miny + 3).Type = Tile.TerrainType.River_a;
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_a");
            world.GetTileAt(minx + 1, miny + 3).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;
        }
        // 2,3 - 3,3
        if ((rvrEdge & N_EDGE) == N_EDGE)
        {
            world.GetTileAt(minx + 2, miny + 3).Type = Tile.TerrainType.River_b;
            if ((roadEdge & N_EDGE) == N_EDGE)
            {
                world.GetTileAt(minx + 3, miny + 3).Type = Tile.TerrainType.Road_a;
            }
            else
            {
                world.GetTileAt(minx + 3, miny + 3).Type = Tile.TerrainType.River_a;
            }
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_b");
            world.GetTileAt(minx + 2, miny + 3).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;

            if ((roadEdge & N_EDGE) == N_EDGE)
            {
                world.GetTileAt(minx + 3, miny + 3).Type = Tile.TerrainType.Road_a;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_a");
                world.GetTileAt(minx + 3, miny + 3).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        // 4,3
        if ((rvrEdge & NNE_EDGE) == NNE_EDGE
            || ((voidedge & NNE_EDGE) == NNE_EDGE && (rvrEdge & N_EDGE) == N_EDGE))
        {
            world.GetTileAt(minx + 4, miny + 3).Type = Tile.TerrainType.River_b;
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_b");
            world.GetTileAt(minx + 4, miny + 3).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;
        }
        //*** NORTH-NORTHEAST - a/d
        // 5,2
        if ((roadEdge & NNE_EDGE) == NNE_EDGE)
        {
            world.GetTileAt(minx + 5, miny + 2).Type = Tile.TerrainType.Road_a;
        }
        else
        {
            if ((rvrEdge & NNE_EDGE) == NNE_EDGE)
            {
                world.GetTileAt(minx + 5, miny + 2).Type = Tile.TerrainType.River_d;
            }
            else
            {
                if (fourPattern)
                {
                    sb.Append(terrain);
                    sb.Append("_d");
                    world.GetTileAt(minx + 5, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                    sb.Length = 0;
                }
                else
                {
                    sb.Append(terrain);
                    sb.Append("_a");
                    world.GetTileAt(minx + 5, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                    sb.Length = 0;
                }
            }
        }
        //*** SOUTH-SOUTHEAST
        // 5,1 - b
        if ((roadEdge & SSE_EDGE) == SSE_EDGE)
        {
            world.GetTileAt(minx + 5, miny + 1).Type = Tile.TerrainType.Road_b;
        }
        else
        {
            if ((rvrEdge & SSE_EDGE) == SSE_EDGE)
            {
                world.GetTileAt(minx + 5, miny + 1).Type = Tile.TerrainType.River_b;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_b");
                world.GetTileAt(minx + 5, miny + 1).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        //*** SOUTH baba OR cdcd
        // 1,0
        if ((rvrEdge & SSW_EDGE) == SSW_EDGE
        || ((voidedge & SSW_EDGE) == SSW_EDGE && (rvrEdge & S_EDGE) == S_EDGE))
        {
            world.GetTileAt(minx + 1, miny).Type = Tile.TerrainType.River_c;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_c");
                world.GetTileAt(minx + 1, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_b");
                world.GetTileAt(minx + 1, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        // 2,0 - 3,0
        if ((rvrEdge & S_EDGE) == S_EDGE)
        {
            world.GetTileAt(minx + 2, miny).Type = Tile.TerrainType.River_d;
            if ((roadEdge & S_EDGE) == S_EDGE)
            {
                world.GetTileAt(minx + 3, miny).Type = Tile.TerrainType.Road_b;
            }
            else
            {
                world.GetTileAt(minx + 3, miny).Type = Tile.TerrainType.River_c;
            }
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_d");
                world.GetTileAt(minx + 2, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
                if ((roadEdge & S_EDGE) == S_EDGE)
                {
                    world.GetTileAt(minx + 3, miny).Type = Tile.TerrainType.Road_b;
                }
                else
                {
                    sb.Append(terrain);
                    sb.Append("_c");
                    world.GetTileAt(minx + 3, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                    sb.Length = 0;
                }
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_a");
                world.GetTileAt(minx + 2, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
                if ((roadEdge & S_EDGE) == S_EDGE)
                {
                    world.GetTileAt(minx + 3, miny).Type = Tile.TerrainType.Road_b;
                }
                else
                {
                    sb.Append(terrain);
                    sb.Append("_b");
                    world.GetTileAt(minx + 3, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                    sb.Length = 0;
                }
            }
        }
        // 4,0
        if ((rvrEdge & SSE_EDGE) == SSE_EDGE
            || ((voidedge & SSE_EDGE) == SSE_EDGE && (rvrEdge & S_EDGE) == S_EDGE))
        {
            world.GetTileAt(minx + 4, miny).Type = Tile.TerrainType.River_d;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_d");
                world.GetTileAt(minx + 4, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_a");
                world.GetTileAt(minx + 4, miny).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        //*** SOUTH-SOUTHWEST
        // 0,1 - a
        if ((roadEdge & SSW_EDGE) == SSW_EDGE)
        {
            world.GetTileAt(minx, miny + 1).Type = Tile.TerrainType.Road_a;
        }
        else
        {
            if ((rvrEdge & SSW_EDGE) == SSW_EDGE)
            {
                world.GetTileAt(minx, miny + 1).Type = Tile.TerrainType.River_a;
            }
            else
            {
                if ((roadEdge & SSW_EDGE) == SSW_EDGE)
                {
                    world.GetTileAt(minx, miny + 1).Type = Tile.TerrainType.Road_a;
                }
                else
                {
                    sb.Append(terrain);
                    sb.Append("_a");
                    world.GetTileAt(minx, miny + 1).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                    sb.Length = 0;
                }
            }
        }
        //*** NORTH-NORTHWEST - b OR c
        // 0,2
        if ((roadEdge & NNW_EDGE) == NNW_EDGE)
        {
            world.GetTileAt(minx, miny + 2).Type = Tile.TerrainType.Road_b;
        }
        else if ((rvrEdge & NNW_EDGE) == NNW_EDGE)
        {
            world.GetTileAt(minx, miny + 2).Type = Tile.TerrainType.River_c;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_c");
                world.GetTileAt(minx, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_b");
                world.GetTileAt(minx, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        //***************
        //*** DO INTERIOR
        // 1,2 a OR d
        if ((roadEdge & NNW_EDGE) == NNW_EDGE)
        {
            world.GetTileAt(minx + 1, miny + 2).Type = Tile.TerrainType.Road_a;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_d");
                world.GetTileAt(minx + 1, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_a");
                world.GetTileAt(minx + 1, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        // 1,1 b
        if ((roadEdge & SSW_EDGE) == SSW_EDGE)
        {
            world.GetTileAt(minx + 1, miny + 1).Type = Tile.TerrainType.Road_b;
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_b");
            world.GetTileAt(minx + 1, miny + 1).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;
        }
        // 2,2 b OR c
        if ((roadEdge & NNW_EDGE) == NNW_EDGE)
        {
            world.GetTileAt(minx + 2, miny + 2).Type = Tile.TerrainType.Road_b;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_c");
                world.GetTileAt(minx + 2, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_b");
                world.GetTileAt(minx + 2, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        // 2,1 a
        if ((roadEdge & SSW_EDGE) == SSW_EDGE)
        {
            world.GetTileAt(minx + 2, miny + 1).Type = Tile.TerrainType.Road_a;
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_a");
            world.GetTileAt(minx + 2, miny + 1).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;
        }
        // 3,2 a OR d
        if ((roadEdge & N_EDGE) == N_EDGE || (roadEdge & NNE_EDGE) == NNE_EDGE || (roadEdge & NNW_EDGE) == NNW_EDGE)
        {
            world.GetTileAt(minx + 3, miny + 2).Type = Tile.TerrainType.Road_a;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_d");
                world.GetTileAt(minx + 3, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_a");
                world.GetTileAt(minx + 3, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        // 3,1 b
        if ((roadEdge & SSW_EDGE) == SSW_EDGE || (roadEdge & S_EDGE) == S_EDGE || (roadEdge & SSE_EDGE) == SSE_EDGE)
        {
            world.GetTileAt(minx + 3, miny + 1).Type = Tile.TerrainType.Road_b;
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_b");
            world.GetTileAt(minx + 3, miny + 1).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;
        }
        // 4,2 b OR c
        if ((roadEdge & NNE_EDGE) == NNE_EDGE)
        {
            world.GetTileAt(minx + 4, miny + 2).Type = Tile.TerrainType.Road_b;
        }
        else
        {
            if (fourPattern)
            {
                sb.Append(terrain);
                sb.Append("_c");
                world.GetTileAt(minx + 4, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
            else
            {
                sb.Append(terrain);
                sb.Append("_b");
                world.GetTileAt(minx + 4, miny + 2).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
                sb.Length = 0;
            }
        }
        // 4,1 a
        if ((roadEdge & SSE_EDGE) == SSE_EDGE)
        {
            world.GetTileAt(minx + 4, miny + 1).Type = Tile.TerrainType.Road_a;
        }
        else
        {
            sb.Append(terrain);
            sb.Append("_a");
            world.GetTileAt(minx + 4, miny + 1).Type = (Tile.TerrainType)Enum.Parse(typeof(Tile.TerrainType), sb.ToString());
            sb.Length = 0;
        }
        sb.ReturnToPool();
        if (hex.HasFeatures())
        {
            if (hex.HasFeature(HexFeature.TOWN))
            {
                // 2,1 - 3,1
                world.GetTileAt(minx + 2, miny + 1).Type = Tile.TerrainType.Town_a;
                world.GetTileAt(minx + 3, miny + 1).Type = Tile.TerrainType.Town_b;
            }
            if (hex.HasFeature(HexFeature.OASIS))
            {
                // 2,2
                world.GetTileAt(minx + 2, miny + 2).Type = Tile.TerrainType.River_c;
            }
            if (hex.HasFeature(HexFeature.RUINS))
            {
                // 2,1 - 3,1
                world.GetTileAt(minx + 2, miny + 1).Type = Tile.TerrainType.Ruins_a;
                world.GetTileAt(minx + 3, miny + 1).Type = Tile.TerrainType.Ruins_b;
            }
            if (hex.HasFeature(HexFeature.CASTLE))
            {
                // 2,1 - 3,1
                world.GetTileAt(minx + 2, miny + 1).Type = Tile.TerrainType.Castle_a;
                world.GetTileAt(minx + 3, miny + 1).Type = Tile.TerrainType.Castle_b;
            }
        }
    }
    private void SetHexTiles(Hex hex, int minx, int miny, int maxx, int maxy)
    {
        int sameEdge = 0, dsrtedge = 0, voidedge = 0, roadEdge = 0, rvrEdge = 0;
        // COUNT EDGES FIRST
        Hex other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_N);
        if (other != null)
        {
            if (other.Type == hex.Type)
            {
                sameEdge += N_EDGE;
            }
            else if (hex.Type != HexType.DESERT && other.Type == HexType.DESERT)
            {
                dsrtedge += N_EDGE;
            }
            if (HexMap.Instance.HasRiverCrossingTo(hex, other))
            {
                rvrEdge += N_EDGE;
                sameEdge &= ~N_EDGE;
            }
            if (HexMap.Instance.HasRoadEdge(hex, other))
            {
                roadEdge += N_EDGE;
            }
        }
        else
        {
            voidedge += N_EDGE;
        }
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_NNE);
        if (other != null)
        {
            if (other.Type == hex.Type)
            {
                sameEdge += NNE_EDGE;
            }
            else if (hex.Type != HexType.DESERT && other.Type == HexType.DESERT)
            {
                dsrtedge += NNE_EDGE;
            }
            if (HexMap.Instance.HasRiverCrossingTo(hex, other))
            {
                rvrEdge += NNE_EDGE;
                sameEdge &= ~NNE_EDGE;
            }
            if (HexMap.Instance.HasRoadEdge(hex, other))
            {
                roadEdge += NNE_EDGE;
            }
        }
        else
        {
            voidedge += NNE_EDGE;
        }
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_SSE);
        if (other != null)
        {
            if (other.Type == hex.Type)
            {
                sameEdge += SSE_EDGE;
            }
            else if (hex.Type != HexType.DESERT && other.Type == HexType.DESERT)
            {
                dsrtedge += SSE_EDGE;
            }
            if (HexMap.Instance.HasRiverCrossingTo(hex, other))
            {
                rvrEdge += SSE_EDGE;
                sameEdge &= ~SSE_EDGE;
            }
            if (HexMap.Instance.HasRoadEdge(hex, other))
            {
                roadEdge += SSE_EDGE;
            }
        }
        else
        {
            voidedge += SSE_EDGE;
        }
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_S);
        if (other != null)
        {
            if (other.Type == hex.Type)
            {
                sameEdge += S_EDGE;
            }
            else if (hex.Type != HexType.DESERT && other.Type == HexType.DESERT)
            {
                dsrtedge += S_EDGE;
            }
            if (HexMap.Instance.HasRiverCrossingTo(hex, other))
            {
                rvrEdge += S_EDGE;
                sameEdge &= ~S_EDGE;
            }
            if (HexMap.Instance.HasRoadEdge(hex, other))
            {
                roadEdge += S_EDGE;
            }
        }
        else
        {
            voidedge += S_EDGE;
        }
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_SSW);
        if (other != null)
        {
            if (other.Type == hex.Type)
            {
                sameEdge += SSW_EDGE;
            }
            else if (hex.Type != HexType.DESERT && other.Type == HexType.DESERT)
            {
                dsrtedge += SSW_EDGE;
            }
            if (HexMap.Instance.HasRiverCrossingTo(hex, other))
            {
                rvrEdge += SSW_EDGE;
                sameEdge &= ~SSW_EDGE;
            }
            if (HexMap.Instance.HasRoadEdge(hex, other))
            {
                roadEdge += SSW_EDGE;
            }
        }
        else
        {
            voidedge += SSW_EDGE;
        }
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_NNW);
        if (other != null)
        {
            if (other.Type == hex.Type)
            {
                sameEdge += NNW_EDGE;
            }
            else if (hex.Type != HexType.DESERT && other.Type == HexType.DESERT)
            {
                dsrtedge += NNW_EDGE;
            }
            if (HexMap.Instance.HasRiverCrossingTo(hex, other))
            {
                rvrEdge += NNW_EDGE;
                sameEdge &= ~NNW_EDGE;
            }
            if (HexMap.Instance.HasRoadEdge(hex, other))
            {
                roadEdge += NNW_EDGE;
            }
        }
        else
        {
            voidedge += NNW_EDGE;
        }
        switch (hex.Type.Type)
        {
            case HexType.COUNTRY_VAL:
                SetTileSprites(hex, "Grass", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, false);
                break;
            case HexType.DESERT_VAL:
                SetTileSprites(hex, "Desert", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.FARM_VAL:
                SetTileSprites(hex, "Farm", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, false);
                break;
            case HexType.FOREST_VAL:
                SetTileSprites(hex, "Forest", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.HILL_VAL:
                SetTileSprites(hex, "Hill", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.MOUNTAIN_VAL:
                SetTileSprites(hex, "Mountain", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.SWAMP_VAL:
                SetTileSprites(hex, "Swamp", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, false);
                break;
        }
    }
    private void SetTileTypes()
    {
        print("SetTileTypes");
        for (int i = HexMap.Instance.MaxHexId; i >= 0; i--)
        {
            Hex hex = HexMap.Instance.GetHexById(i);
            if (hex != null)
            {
                // find hex's bottom-left corner
                // hexes are 4 rows high
                int miny = 23 - (int)hex.Location.y;
                miny *= 4;
                if (hex.Location.x % 2 == 1)
                {
                    miny += 2;
                }
                int minx = ((int)hex.Location.x - 1) * 5;
                SetHexTiles(hex, minx, miny, minx + 5, miny + 3);
            }
        }
    }
    public bool RequiresMarker(Vector3 pos)
    {
        bool does = false;
        Tile tileData = world.GetTileAtWorldCoordinates(pos);
        if (tileData != null
            && tileData.Type != Tile.TerrainType.Void)
        {
            does = true;
        }
        return does;
    }
    public Tile GetTileAtWorldCoordinates(Vector3 pos)
    {
        return world.GetTileAtWorldCoordinates(pos);
    }
    public Vector3 GetTileCoordinatesForWorldCoordinates(Vector3 pos)
    {
        return world.GetTileCoordinatesForWorldCoordinates(pos);
    }
}

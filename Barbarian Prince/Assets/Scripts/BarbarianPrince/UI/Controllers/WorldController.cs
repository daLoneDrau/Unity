using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.BarbarianPrince.Graph;
using Assets.Scripts.BarbarianPrince.Singletons;
using Assets.Scripts.BarbarianPrince.UI.Controllers;
using Assets.Scripts.RPGBase.Graph;
using Assets.Scripts.UI;
using RPGBase.Pooled;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private World world;
    /// <summary>
    /// Transform to hold all our game objects, so they don't clog up the hierarchy.
    /// </summary>
    private Transform tileHolder;
    private Dictionary<string, GameObject> tileObjects;
    // Use this for initialization
    private Hex[] hexList;
    private bool doonce = false;
    /// <summary>
    /// the dimensions for the number of tiles that can fit in the viewport.
    /// </summary>
    private Vector2 viewportDimensions;
    void Awake()
    {
        MouseListener ml = MouseListener.Instance;
        ViewportController vc = ViewportController.Instance;
        LoadResources();
        // load hex tiles
        StartCoroutine(BPServiceClient.Instance.GetAllHexes(value => hexList = value));
        // reposition camera to 0,0
        ViewportController.Instance.PositionViewport(new Vector2(0, 0));
    }
    private void InitMap()
    {
        print("hexes loaded " + hexList.Length);
        doonce = true;
        HexMap.Instance.Hexes = hexList;
        HexMap.Instance.Load();
        // create world
        world = new World();
        ViewportController.Instance.MaxY = world.Height;
        ViewportController.Instance.MaxX = world.Width;
        // create object to hold game tiles
        tileHolder = new GameObject("Board").transform;
        // create map to hold references to all game tiles
        tileObjects = new Dictionary<string, GameObject>();
        viewportDimensions = ViewportController.Instance.RequiredTileDimensions;
        print("need tiles for " + viewportDimensions);
        // create game tiles
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
                tileObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteMap>().GetSprite("void");
                tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
                // set new tile as child of tile holder
                tileObject.transform.SetParent(tileHolder);
            }
        }
        print("About to set tiles");
        SetTileTypes();
    }
    private void DisplayMap()
    {
        // need to go through tiles and display correct ones.

        // get the viewport's range.
        Vector2 v = ViewportController.Instance.ViewportPosition;
        int minx = Mathf.FloorToInt(v.x);
        int miny = Mathf.FloorToInt(v.y);
        PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
        SpriteMap sm = gameObject.GetComponent<SpriteMap>();
        for (int x = minx, lx = minx + (int)viewportDimensions.x; x < lx; x++)
        {
            for (int y = minx, ly = miny + (int)viewportDimensions.y; y < ly; y++)
            {
                Tile tile = world.GetTileAt(x, y);
                if (tile != null)
                {
                    sb.Append("Tile_");
                    sb.Append(x - minx);
                    sb.Append("_");
                    sb.Append(y - miny);
                    GameObject go = tileObjects[sb.ToString()];
                    sb.Length = 0;

                    go.GetComponent<SpriteRenderer>().sprite = sm.GetSprite(tile.Type.ToString().ToLower());
                }
            }
        }
        sb.ReturnToPool();
    }
    // Update is called once per frame
    void Update()
    {
        if (hexList != null
                && !doonce)
        {
            InitMap();
        }
        else
        {
            //DisplayMap();
        }
    }
    private void LoadResources()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("config");
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);
        XmlNode root = xmldoc.SelectSingleNode("endpoints");
        print(root.SelectSingleNode("bp_endpoint").InnerText);
        BPServiceClient.Instance.Endpoint = root.SelectSingleNode("bp_endpoint").InnerText;
        print(BPServiceClient.Instance.Endpoint);
    }
    private void SetCountryTiles(Hex hex, int minx, int miny, int maxx, int maxy)
    {
        // DO EDGES FIRST
        // N
        Hex other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_N);
        Debug.Log(other);
        // TODO - check for river crossing
        // north side is void/country
        Tile tileData = world.GetTileAt(minx + 2, maxy);
        tileData.Type = Tile.TerrainType.Grass_0;
        tileData = world.GetTileAt(minx + 3, maxy);
        tileData.Type = Tile.TerrainType.Grass_0;
        // TO DO - check for river crossing

        // NNE
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_NNE);
        if (other == null)
        {
            // NNE side is void
            tileData = world.GetTileAt(maxx - 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_void_3;
            tileData = world.GetTileAt(maxx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_void_3;
        }
        else
        {
            tileData = world.GetTileAt(maxx - 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(maxx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // SSE
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_SSE);
        if (other == null)
        {
            // SSE side is void
            tileData = world.GetTileAt(maxx - 1, miny);
            tileData.Type = Tile.TerrainType.Grass_void_6;
            tileData = world.GetTileAt(maxx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_void_6;
        }
        else
        {
            tileData = world.GetTileAt(maxx - 1, miny);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(maxx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // S
        // north side is void/country
        tileData = world.GetTileAt(minx + 2, miny);
        tileData.Type = Tile.TerrainType.Grass_0;
        tileData = world.GetTileAt(minx + 3, miny);
        tileData.Type = Tile.TerrainType.Grass_0;
        // TO DO - check for river crossing
        // SSW
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_SSW);
        if (other == null)
        {
            // SSW side is void
            tileData = world.GetTileAt(minx + 1, miny);
            tileData.Type = Tile.TerrainType.Grass_void_12;
            tileData = world.GetTileAt(minx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_void_12;
        }
        else
        {
            tileData = world.GetTileAt(minx + 1, miny);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // NNW
        other = HexMap.Instance.GetNeighborHex(hex.Index, HexCoordinateSystem.DIRECTION_NNW);
        if (other == null)
        {
            // NNW side is void
            tileData = world.GetTileAt(minx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_void_9;
            tileData = world.GetTileAt(minx + 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_void_9;
        }
        else
        {
            tileData = world.GetTileAt(minx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx + 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // DO MIDDLE
        if (hex.HasFeature(HexFeature.TOWN))
        {
            tileData = world.GetTileAt(minx + 1, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx + 1, miny + 2);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx + 4, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx + 4, miny + 2);
            tileData.Type = Tile.TerrainType.Grass_0;
            // town sprite
            tileData = world.GetTileAt(minx + 2, miny + 1);
            tileData.Type = Tile.TerrainType.Town_3;
            tileData = world.GetTileAt(minx + 2, miny + 2);
            tileData.Type = Tile.TerrainType.Town_0;
            tileData = world.GetTileAt(minx + 3, miny + 1);
            tileData.Type = Tile.TerrainType.Town_2;
            tileData = world.GetTileAt(minx + 3, miny + 2);
            tileData.Type = Tile.TerrainType.Town_1;
        }
        else
        {
            for (int x = minx + 1; x < maxx; x++)
            {
                for (int y = miny + 1; y < maxy; y++)
                {
                    Tile td = world.GetTileAt(x, y);
                    td.Type = Tile.TerrainType.Grass_0;
                }
            }
        }
    }
    private const int N_EDGE = 1;
    private const int NNE_EDGE = 2;
    private const int SSE_EDGE = 4;
    private const int S_EDGE = 8;
    private const int SSW_EDGE = 16;
    private const int NNW_EDGE = 32;
    private void SetCountryTile(Hex hex, string terrain, int minx, int miny, int sameEdge, int dsrtedge, int voidedge, int roadEdge, int rvrEdge, bool fourPattern)
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
                SetCountryTile(hex, "Grass", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, false);
                break;
            case HexType.DESERT_VAL:
                SetCountryTile(hex, "Desert", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.FARM_VAL:
                SetCountryTile(hex, "Farm", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, false);
                break;
            case HexType.FOREST_VAL:
                SetCountryTile(hex, "Forest", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.HILL_VAL:
                SetCountryTile(hex, "Hill", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.MOUNTAIN_VAL:
                SetCountryTile(hex, "Mountain", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, true);
                break;
            case HexType.SWAMP_VAL:
                SetCountryTile(hex, "Swamp", minx, miny, sameEdge, dsrtedge, voidedge, roadEdge, rvrEdge, false);
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
                /*
                int minx = 9999999, miny = 9999999, maxx = -1, maxy = -1;
                // find all tiles that belong in the hex
                for (int x = world.Width - 1; x >= 0; x--)
                {
                    for (int y = world.Height - 1; y >= 0; y--)
                    {

                        Tile tileData = world.GetTileAt(x, y);
                        Hex other = world.GetHexForTileCoordinates(x, y);
                        if (other != null && other.Equals(hex))
                        {
                            minx = Mathf.Min(x, minx);
                            miny = Mathf.Min(y, miny);
                            maxx = Mathf.Max(x, maxx);
                            maxy = Mathf.Max(y, maxy);
                        }
                    }
                }
                */
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
    private void OnTileChanged(Tile tile, GameObject obj)
    {
        SpriteMap sm = gameObject.GetComponent<SpriteMap>();
        obj.GetComponent<SpriteRenderer>().sprite = sm.GetSprite(tile.Type.ToString().ToLower());
        obj.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
    }
    private float randomizeTimer = 2f;
}

using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.RPGBase.Graph;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
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
    void Awake()
    {
        // create world
        world = new World();
        // create object to hold game tiles
        tileHolder = new GameObject("Board").transform;
        // create map to hold references to all game tiles
        tileObjects = new Dictionary<string, GameObject>();
        // create game tiles
        for (int x = world.Width - 1; x >= 0; x--)
        {
            for (int y = world.Height - 1; y >= 0; y--)
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
                tileData.AddTypeListener((tile) => { OnTileChanged(tile, tileObject); });
            }
        }
        // set the world
        SetTileTypes();
    }
    private void SetCountryTiles(BPHexagon hex, int minx, int miny, int maxx, int maxy)
    {
        // DO EDGES FIRST
        // N
        BPHexagon other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_N));
        // TODO - check for river crossing
        // north side is void/country
        Tile tileData = world.GetTileAt(minx + 2, maxy);
        tileData.Type = Tile.TerrainType.Grass_0;
        tileData = world.GetTileAt(minx + 3, maxy);
        tileData.Type = Tile.TerrainType.Grass_0;
        // TO DO - check for river crossing

        // NNE
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_NNE));
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
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_SSE));
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
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_SSW));
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
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_NNW));
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
        switch (hex.Feature)
        {
            case BPHexagon.FeatureType.None:
                for (int x = minx + 1; x < maxx; x++)
                {
                    for (int y = miny + 1; y < maxy; y++)
                    {
                        Tile td = world.GetTileAt(x, y);
                        td.Type = Tile.TerrainType.Grass_0;
                    }
                }
                break;
            case BPHexagon.FeatureType.Town:
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
                break;
        }
    }
    private const int N_EDGE = 1;
    private const int NNE_EDGE = 2;
    private const int SSE_EDGE = 4;
    private const int S_EDGE = 8;
    private const int SSW_EDGE = 16;
    private const int NNW_EDGE = 32;
    private void SetMountainTiles(BPHexagon hex, int minx, int miny, int maxx, int maxy)
    {
        int mtnedge = 0, dsrtedge = 0, voidedge = 0;
        // COUNT EDGES FIRST
        BPHexagon other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_N));
        if (other != null)
        {
            if (other.Type == BPHexagon.HexType.Mountain)
            {
                mtnedge += N_EDGE;
            }
            else if (other.Type == BPHexagon.HexType.Desert)
            {
                dsrtedge += N_EDGE;
            }
        }
        else
        {
            voidedge += N_EDGE;
        }
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_NNE));
        if (other != null)
        {
            if (other.Type == BPHexagon.HexType.Mountain)
            {
                mtnedge += NNE_EDGE;
            }
            else if (other.Type == BPHexagon.HexType.Desert)
            {
                dsrtedge += NNE_EDGE;
            }
        }
        else
        {
            voidedge += NNE_EDGE;
        }
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_SSE));
        if (other != null)
        {
            if (other.Type == BPHexagon.HexType.Mountain)
            {
                mtnedge += SSE_EDGE;
            }
            else if (other.Type == BPHexagon.HexType.Desert)
            {
                dsrtedge += SSE_EDGE;
            }
        }
        else
        {
            voidedge += SSE_EDGE;
        }
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_S));
        if (other != null)
        {
            if (other.Type == BPHexagon.HexType.Mountain)
            {
                mtnedge += S_EDGE;
            }
            else if (other.Type == BPHexagon.HexType.Desert)
            {
                dsrtedge += S_EDGE;
            }
        }
        else
        {
            voidedge += S_EDGE;
        }
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_SSW));
        if (other != null)
        {
            if (other.Type == BPHexagon.HexType.Mountain)
            {
                mtnedge += SSW_EDGE;
            }
            else if (other.Type == BPHexagon.HexType.Desert)
            {
                dsrtedge += SSW_EDGE;
            }
        }
        else
        {
            voidedge += SSW_EDGE;
        }
        other = (BPHexagon)world.GetHex(world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_NNW));
        if (other != null)
        {
            if (other.Type == BPHexagon.HexType.Mountain)
            {
                mtnedge += NNW_EDGE;
            }
            else if (other.Type == BPHexagon.HexType.Desert)
            {
                dsrtedge += NNW_EDGE;
            }
        }
        else
        {
            voidedge += NNW_EDGE;
        }
        //************************
        // TOP ROW
        //************************
        if ((mtnedge & N_EDGE) == N_EDGE)
        {
            // NORTH SIDE IS MOUNTAIN
            if ((mtnedge & NNE_EDGE) == NNE_EDGE)
            {
                // NORTH-NORTHEAST NEIGHBOR IS MOUNTAIN
                if ((mtnedge & NNW_EDGE) == NNW_EDGE)
                {
                    // NORTH-NORTHWEST NEIGHBOR IS MOUNTAIN
                    // ALL NORTH NEIGHBORS ARE MOUNTAIN - TESTED
                    for (int i = minx + 1; i < maxx; i++)
                    {
                        Tile tileData = world.GetTileAt(i, maxy);
                        tileData.Type = Tile.TerrainType.Mountain_0;
                    }
                }
                else
                {
                    //  NORTH, NORTH-NORTHEAST NEIGHBOR IS MOUNTAIN, NORTH-NORTHWEST IS NOT - TESTED
                    Tile tileData = world.GetTileAt(minx + 4, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_0;
                    tileData = world.GetTileAt(minx + 3, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_0;
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_8;
                    tileData = world.GetTileAt(minx + 1, maxy);
                    if ((voidedge & NNW_EDGE) == NNW_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_9;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                }
            }
            else
            {
                // NORTH-NORTHEAST NEIGHBOR IS NOT MOUNTAIN
                if ((mtnedge & NNW_EDGE) == NNW_EDGE)
                {
                    //  NORTH, NORTH-NORTHWEST NEIGHBOR IS MOUNTAIN, NORTH-NORTHEAST IS NOT - TESTED
                    Tile tileData = world.GetTileAt(minx + 1, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_0;
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_0;
                    tileData = world.GetTileAt(minx +3, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_2;
                    tileData = world.GetTileAt(minx + 4, maxy);
                    if ((voidedge & NNE_EDGE) == NNE_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_3;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                }
                else
                {
                    // NORTH IS MOUNTAIN, NORTH-NORTHEAST, NORTH-NORTHWEST IS NOT - TESTED
                    Tile tileData = world.GetTileAt(minx + 1, maxy);
                    if ((voidedge & NNW_EDGE) == NNW_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_9;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_8;
                    tileData = world.GetTileAt(minx + 3, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_2;
                    tileData = world.GetTileAt(minx + 4, maxy);
                    if ((voidedge & NNE_EDGE) == NNE_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_3;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                }
            }
        }
        else
        {
            // NORTH NEIGHBOR IS NOT MOUNTAIN
            if ((mtnedge & NNE_EDGE) == NNE_EDGE)
            {
                // NORTH-NORTHEAST NEIGHBOR IS MOUNTAIN
                if ((mtnedge & NNW_EDGE) == NNW_EDGE)
                {
                    // NORTH-NORTHWEST NEIGHBOR IS MOUNTAIN
                    // NORTH-NORTHEAST, NORTH-NORTHWEST IS MOUNTAIN, NORTH IS NOT - TESTED
                    Tile tileData = world.GetTileAt(minx + 1, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_3;
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 3, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 4, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_9;
                }
                else
                {
                    // NORTH-NORTHWEST NEIGHBOR IS NOT MOUNTAIN
                    // NORTH-NORTHEAST IS MOUNTAIN, NORTH, NORTH-NORTHWEST IS NOT - TESTED
                    Tile tileData = world.GetTileAt(minx + 1, maxy);
                    if ((voidedge & NNW_EDGE) == NNW_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_9;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 3, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 4, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_9;
                }
            }
            else
            {
                // NORTH-NORTHEAST NEIGHBOR IS NOT MOUNTAIN
                if ((mtnedge & NNW_EDGE) == NNW_EDGE)
                {
                    // NORTH-NORTHWEST NEIGHBOR IS MOUNTAIN
                    // NORTH-NORTHWEST IS MOUNTAIN, NORTH, NORTH-NORTHEAST IS NOT - TESTED
                    Tile tileData = world.GetTileAt(minx + 1, maxy);
                    tileData.Type = Tile.TerrainType.Mountain_3;
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 3, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 4, maxy);
                    if ((voidedge & NNE_EDGE) == NNE_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_3;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                }
                else
                {
                    // NORTH-NORTHWEST NEIGHBOR IS NOT MOUNTAIN
                    // NO NEIGHBORS ARE MOUNTAIN - TESTED
                    Tile tileData = world.GetTileAt(minx + 1, maxy);
                    if ((voidedge & NNW_EDGE) == NNW_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_9;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                    tileData = world.GetTileAt(minx + 2, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 3, maxy);
                    tileData.Type = Tile.TerrainType.Grass_0;
                    tileData = world.GetTileAt(minx + 4, maxy);
                    if ((voidedge & NNE_EDGE) == NNE_EDGE)
                    {
                        tileData.Type = Tile.TerrainType.Grass_void_3;
                    }
                    else
                    {
                        tileData.Type = Tile.TerrainType.Grass_0;
                    }
                }
            }
        }
        /*

        // NNE
        nc = world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_NNE);
        other = (BPHexagon)world.GetHex(nc);
        if (other == null)
        {
            // NNE side is void
            Tile tileData = world.GetTileAt(maxx - 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_void_3;
            tileData = world.GetTileAt(maxx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_void_3;
        }
        else if (other.Type == BPHexagon.HexType.Countryside)
        {
            Tile tileData = world.GetTileAt(maxx - 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(maxx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // SSE
        nc = world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_SSE);
        other = (BPHexagon)world.GetHex(nc);
        if (other == null)
        {
            // SSE side is void
            Tile tileData = world.GetTileAt(maxx - 1, miny);
            tileData.Type = Tile.TerrainType.Grass_void_6;
            tileData = world.GetTileAt(maxx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_void_6;
        }
        else if (other.Type == BPHexagon.HexType.Countryside)
        {
            Tile tileData = world.GetTileAt(maxx - 1, miny);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(maxx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // S
        nc = world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_S);
        other = (BPHexagon)world.GetHex(nc);
        if (other == null
                || other.Type == BPHexagon.HexType.Countryside)
        {
            // north side is void/country
            Tile tileData = world.GetTileAt(minx + 2, miny);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx + 3, miny);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // SSW
        nc = world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_SSW);
        other = (BPHexagon)world.GetHex(nc);
        if (other == null)
        {
            // SSW side is void
            Tile tileData = world.GetTileAt(minx + 1, miny);
            tileData.Type = Tile.TerrainType.Grass_void_12;
            tileData = world.GetTileAt(minx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_void_12;
        }
        else if (other.Type == BPHexagon.HexType.Countryside)
        {
            Tile tileData = world.GetTileAt(minx + 1, miny);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx, miny + 1);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // NNW
        nc = world.GetNeighborCoordinates(hex, HexCoordinateSystem.DIRECTION_NNW);
        other = (BPHexagon)world.GetHex(nc);
        if (other == null)
        {
            // NNW side is void
            Tile tileData = world.GetTileAt(minx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_void_9;
            tileData = world.GetTileAt(minx + 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_void_9;
        }
        else if (other.Type == BPHexagon.HexType.Countryside)
        {
            Tile tileData = world.GetTileAt(minx, maxy - 1);
            tileData.Type = Tile.TerrainType.Grass_0;
            tileData = world.GetTileAt(minx + 1, maxy);
            tileData.Type = Tile.TerrainType.Grass_0;
        }
        // TO DO - check for river crossing
        // DO MIDDLE
        switch (hex.Feature)
        {
            case BPHexagon.FeatureType.None:
                for (int x = minx + 1; x < maxx; x++)
                {
                    for (int y = miny + 1; y < maxy; y++)
                    {
                        Tile td = world.GetTileAt(x, y);
                        td.Type = Tile.TerrainType.Grass_0;
                    }
                }
                break;
            case BPHexagon.FeatureType.Town:
                Tile tileData = world.GetTileAt(minx + 1, miny + 1);
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
                break;
        }
        */
    }
    private void SetTileTypes()
    {
        Hexagon[] hexes = world.GetHexes();
        for (int i = hexes.Length - 1; i >= 0; i--)
        {
            BPHexagon hex = (BPHexagon)hexes[i];
            Debug.Log("setting tile types for " + hex.GetVector());
            int minx = 9999999, miny = 9999999, maxx = -1, maxy = -1;
            // find all tiles that belong in the hex
            for (int x = world.Width - 1; x >= 0; x--)
            {
                for (int y = world.Height - 1; y >= 0; y--)
                {
                    Tile tileData = world.GetTileAt(x, y);
                    BPHexagon other = world.GetHexForTileCoordinates(x, y);
                    if (other != null && other.Equals(hex))
                    {
                        minx = Mathf.Min(x, minx);
                        miny = Mathf.Min(y, miny);
                        maxx = Mathf.Max(x, maxx);
                        maxy = Mathf.Max(y, maxy);
                    }
                }
            }
            switch (hex.Type)
            {
                case BPHexagon.HexType.Countryside:
                    SetCountryTiles(hex, minx, miny, maxx, maxy);
                    break;
                case BPHexagon.HexType.Mountain:
                    SetMountainTiles(hex, minx, miny, maxx, maxy);
                    break;
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
        /*
        switch (tile.Type)
        {
            case Tile.TerrainType.Grass_0:
                break;
        }
        if (tile.Type == Tile.TerrainType.Grass_0)
        {
            obj.GetComponent<SpriteRenderer>().sprite = sm.GetSprite("grass_0");
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        }
        else if (tile.Type == Tile.TerrainType.Void)
        {
            obj.GetComponent<SpriteRenderer>().sprite = sm.GetSprite("void");
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        }
        else
        {
            Debug.LogError("Invalid floor type - " + tile.Type);
        }
        */
    }
    private float randomizeTimer = 2f;
    // Update is called once per frame
    void Update()
    {
        randomizeTimer -= Time.deltaTime;
        if (randomizeTimer < 0)
        {
            randomizeTimer = 2f;
        }
    }
}

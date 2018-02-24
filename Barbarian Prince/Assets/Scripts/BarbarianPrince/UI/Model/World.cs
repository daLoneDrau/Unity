using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.RPGBase.Graph;
using Assets.Scripts.BarbarianPrince.Flyweights;
using System.Xml;

public class World
{
    /// <summary>
    /// the hex coordinate system for the game map.
    /// </summary>
    private HexCoordinateSystem hexMap;
    Tile[,] tiles;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public World(int w = 100, int h = 100)
    {
        hexMap = new HexCoordinateSystem(HexCoordinateSystem.EVEN_Q);
        LoadHexTiles();
        int minx = (int)hexMap.GetMapRange()[0].x, miny = (int)hexMap.GetMapRange()[0].y;
        int maxx = (int)hexMap.GetMapRange()[1].x, maxy = (int)hexMap.GetMapRange()[1].y;
        Width = maxx - minx + 1;
        Width *= 5;
        Width++;
        Height = h;
        Height = maxy - miny + 1;
        Height *= 4;
        Height += 2;
        tiles = new Tile[Width, Height];
        for (int x = Width - 1; x >= 0; x--)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                tiles[x, y] = new Tile(this);
            }
        }
    }
    public BPHexagon GetHexForTileCoordinates(int x, int y)
    {
        // minimum column range
        int col = (int)hexMap.GetMapRange()[0].x;
        int row = 0;
        // col 1 covers 0-5, col 2 covers 5-10, col 3 covers 11-15, etc...
        col += x / 5;
        if (x % 5 == 0)
        {
            // x is an edge
            if ((x & 1) == 1)
            {
                col--;
                if (y % 4 == 1 || y % 4 == 2)
                {
                    col++;
                }
            }
            else
            {
                if (y % 4 == 1 || y % 4 == 2)
                {
                    col--;
                }
            }
        }
        if ((col & 1) == 0)
        {
            // even column
            // row is found by dividing y-coordinate by 4
            row = y / 4;
            // then offsetting by the map height, since map y-axis is descending
            row = (int)hexMap.GetMapRange()[1].y - row;
        }
        else
        {
            // odd column
            row = (y + 2) / 4;
            row--;
            row = (int)hexMap.GetMapRange()[1].y - row;
        }
        return (BPHexagon)hexMap.GetHexagon(col, row);
    }
    /// <summary>
    /// Gets the list of <see cref="Hexagon"/>s in the coordinate system.
    /// </summary>
    /// <returns><see cref="Hexagon"/>[]</returns>
    public Hexagon[] GetHexes()
    {
        return (Hexagon[])hexMap.Hexes;
    }
    public Vector3 GetNeighborCoordinates(Hexagon hex, int direction)
    {
        return hexMap.GetNeighborCoordinates(hex, direction);
    }
    public Hexagon GetHex(Vector3 coordinates)
    {
        return hexMap.GetHexagon(coordinates);
    }
    /// <summary>
    /// Loads all hex tiles for the game map.
    /// </summary>
    private void LoadHexTiles()
    {
        Debug.Log("LoadHexTiles");
        TextAsset textAsset = (TextAsset)Resources.Load("config");
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);
        XmlNode root = xmldoc.SelectSingleNode("data");
        XmlNode map = root.SelectSingleNode("map");
        XmlNodeList hexes = map.SelectNodes("hex");
        for (int i = hexes.Count - 1; i >= 0; i--)
        {
            XmlNode hexData = hexes.Item(i);
            BPHexagon.HexType ht = (BPHexagon.HexType)Enum.Parse(typeof(BPHexagon.HexType), hexData.SelectSingleNode("type").InnerText, true);
            BPHexagon hex = new BPHexagon(ht);
            if (hexData.SelectSingleNode("feature") != null)
            {
                XmlNode featureData = hexData.SelectSingleNode("feature");
                hex.Feature = (BPHexagon.FeatureType)Enum.Parse(typeof(BPHexagon.FeatureType), featureData.SelectSingleNode("type").InnerText, true);
                if (featureData.SelectSingleNode("name") != null)
                {
                    hex.Name = featureData.SelectSingleNode("name").InnerText;
                }
            }
            hex.SetCoordinates(hexMap.GetCubeCoordinates(Int32.Parse(hexData.SelectSingleNode("x").InnerText), Int32.Parse(hexData.SelectSingleNode("y").InnerText)));
            hexMap.AddHexagon(hex);
        }
    }
    public Tile GetTileAtWorldCoordinates(Vector3 pos)
    {
        Tile t = null;
        for (int x = Width - 1; x >= 0; x--)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                Rect r = new Rect((float)x - 0.5f, (float)y - 0.5f, 1, 1);
                if (r.Contains(pos))
                {
                    t = tiles[x, y];
                    break;
                }
            }
        }
        return t;
    }
    public Vector3 GetTileCoordinatesForWorldCoordinates(Vector3 pos)
    {
        Vector3 v = new Vector3(0, 0, 0);
        for (int x = Width - 1; x >= 0; x--)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                Rect r = new Rect((float)x - 0.5f, (float)y - 0.5f, 1, 1);
                if (r.Contains(pos))
                {
                    v.Set(x, y, 0);
                    break;
                }
            }
        }
        return v;
    }
    public Tile GetTileAt(int x, int y)
    {
        Tile t = null;
        if (x >= 0
            && x < Width
            && y >= 0
            && y < Height)
        {
            if (tiles[x, y] == null)
            {
                tiles[x, y] = new Tile(this);
            }
            t = tiles[x, y];
        }
        return t;
    }
}

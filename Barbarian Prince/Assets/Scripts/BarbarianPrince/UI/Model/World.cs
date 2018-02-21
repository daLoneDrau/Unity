using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.RPGBase.Graph;
using Assets.Scripts.BarbarianPrince.Flyweights;

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
        Debug.Log(Width + "," + Height);
        tiles = new Tile[Width, Height];
        for (int x = Width - 1; x >= 0; x--)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                tiles[x, y] = new Tile(this);
                GetHexForTileCoordinates(x, y);
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
        Debug.Log("tile_" + x + "_" + y + " is in hex " + col + "," + row);
        return (BPHexagon)hexMap.GetHexagon(x, y);
    }
    private void LoadHexTiles()
    {
        Debug.Log("LoadHexTiles");
        // create 1,1
        BPHexagon hex = new BPHexagon(BPHexagon.HexType.Country);
        hex.SetCoordinates(hexMap.GetCubeCoordinates(1, 1));
        hexMap.AddHexagon(hex);
        // create 1,2
        BPHexagon hex2 = new BPHexagon(BPHexagon.HexType.Country);
        hex2.SetCoordinates(hexMap.GetCubeCoordinates(1, 2));
        hexMap.AddHexagon(hex2);
        // create 2,1
        BPHexagon hex3 = new BPHexagon(BPHexagon.HexType.Country);
        hex3.SetCoordinates(hexMap.GetCubeCoordinates(2, 1));
        hexMap.AddHexagon(hex3);
        // create 2,2
        BPHexagon hex4 = new BPHexagon(BPHexagon.HexType.Country);
        hex4.SetCoordinates(hexMap.GetCubeCoordinates(2, 2));
        hexMap.AddHexagon(hex4);
        Debug.Log(hexMap.GetMapRange()[0]);
        Debug.Log(hexMap.GetMapRange()[1]);
        //

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

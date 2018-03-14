using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.RPGBase.Graph;
using Assets.Scripts.BarbarianPrince.Flyweights;
using System.Xml;
using Assets.Scripts.BarbarianPrince.Graph;

public class World
{
    Tile[,] tiles;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public World(int w = 100, int h = 100)
    {
        Vector2[] range = HexMap.Instance.GetMapRange();
        int minx = (int)range[0].x, miny = (int)range[0].y;
        int maxx = (int)range[1].x, maxy = (int)range[1].y;
        Debug.Log("range:" + minx + "," + miny + "-" + maxx + "," + maxy);
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
    public Hex GetHexForTileCoordinates(int x, int y)
    {
        // minimum column range
        int col = (int)HexMap.Instance.GetMapRange()[0].x;
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
            row = (int)HexMap.Instance.GetMapRange()[1].y - row;
        }
        else
        {
            // odd column
            row = (y + 2) / 4;
            row--;
            row = (int)HexMap.Instance.GetMapRange()[1].y - row;
        }
        return (Hex)HexMap.Instance.GetHex(col, row);
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

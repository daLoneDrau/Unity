using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class World
{
    Tile[,] tiles;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public World(int w = 100, int h = 100)
    {
        Width = w;
        Height = h;
        tiles = new Tile[Width, Height];
        for (int x = Width - 1; x >= 0; x--)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                tiles[x, y] = new Tile(this);
            }
        }
    }
    public void RandomizeTiles()
    {
        Console.WriteLine("RandomizeTiles");
        for (int x = Width - 1; x >= 0; x--)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    tiles[x, y].Type = Tile.TerrainType.Empty;
                }
                else
                {
                    tiles[x, y].Type = Tile.TerrainType.Floor;
                }
            }
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
                if(r.Contains(pos))
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
        Vector3 v = new Vector3(0,0,0);
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

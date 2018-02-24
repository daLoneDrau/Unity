using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Action<Tile> typeListener;
    public enum TerrainType { Void, Grass_0, Grass_void_3, Grass_void_6, Grass_void_9, Grass_void_12, Town_0, Town_1, Town_2, Town_3, Mountain_0, Mountain_1, Mountain_2, Mountain_3, Mountain_4, Mountain_6, Mountain_8, Mountain_9, Mountain_12 };
    private TerrainType type;
    public TerrainType Type
    {
        get { return type; }
        set
        {
            if (type != value)
            {
                type = value;
                if (typeListener != null)
                {
                    typeListener(this);
                }
            }
        }
    }
    private World world;
    public Tile(World w)
    {
        world = w;
        Type = TerrainType.Void;
    }
    public void AddTypeListener(Action<Tile> callback)
    {
        typeListener += callback;
    }
}

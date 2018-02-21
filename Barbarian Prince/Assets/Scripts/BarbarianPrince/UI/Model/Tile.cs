using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Action<Tile> typeListener;
    public enum TerrainType { Void, Grass };
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

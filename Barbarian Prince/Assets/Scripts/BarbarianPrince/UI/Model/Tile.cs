using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Action<Tile> typeListener;
    public enum TerrainType
    {
        Void,
        Desert_0,
        Desert_void_3, Desert_void_6, Desert_void_9, Desert_void_12,
        Forest_0, Forest_1, Forest_2, Forest_3, Forest_4, Forest_6, Forest_8, Forest_9, Forest_12,
        Forest_void_3, Forest_void_6, Forest_void_9, Forest_void_12,
        Grass_0,
        Grass_void_3, Grass_void_6, Grass_void_9, Grass_void_12,
        Oasis_0, Oasis_1, Oasis_2, Oasis_3,
        Town_0, Town_1, Town_2, Town_3,
        Hill_0, Hill_1, Hill_2, Hill_3, Hill_4, Hill_6, Hill_8, Hill_9, Hill_12,
        Hill_void_3, Hill_void_6, Hill_void_9, Hill_void_12,
        Mountain_0, Mountain_1, Mountain_2, Mountain_3, Mountain_4, Mountain_6, Mountain_8, Mountain_9, Mountain_12,
        Mountain_void_3, Mountain_void_6, Mountain_void_9, Mountain_void_12,
        River_1, River_2, River_3, River_4, River_6, River_8, River_9, River_12,
        Castle_a, Castle_b,
        Desert_a, Desert_b, Desert_c, Desert_d,
        Farm_a, Farm_b,
        Forest_a, Forest_b, Forest_c, Forest_d,
        Grass_a, Grass_b,
        Hill_a, Hill_b, Hill_c, Hill_d,
        Mountain_a, Mountain_b, Mountain_c, Mountain_d,
        River_a, River_b, River_c, River_d,
        Road_a, Road_b,
        Ruins_a, Ruins_b,
        Shrine_a, Shrine_b,
        Swamp_a, Swamp_b,
        Town_a, Town_b
    };
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

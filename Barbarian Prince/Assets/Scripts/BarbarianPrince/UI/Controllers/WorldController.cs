using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
    private World world;
    [SerializeField]
    private Sprite floorSPrite;
    /// <summary>
    /// Transform to hold all our game objects, so they don't clog up the hierarchy.
    /// </summary>
    private Transform tileHolder;
    // Use this for initialization
    void Awake () {
        world = new World();
        tileHolder = new GameObject("Board").transform;

        for (int x = world.Width - 1; x >= 0; x--)
        {
            for (int y = world.Height - 1; y >= 0; y--)
            {
                Tile tileData = world.GetTileAt(x, y);
                GameObject tileObject = new GameObject
                {
                    name = "Tile_" + x + "_" + y
                };
                tileObject.transform.position = new Vector3(x, y, 0);
                tileObject.AddComponent<SpriteRenderer>();
                tileObject.transform.SetParent(tileHolder); // set new tile as child of tile holder
                tileData.AddTypeListener((tile) => { OnTileChanged(tile, tileObject); });
            }
        }
        world.RandomizeTiles();
    }
    public bool RequiresMarker(Vector3 pos)
    {
        bool does = false;
        Tile tileData = world.GetTileAtWorldCoordinates(pos);
        if (tileData != null
            && tileData.Type != Tile.TerrainType.Empty)
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
        if (tile.Type == Tile.TerrainType.Floor)
        {
            obj.GetComponent<SpriteRenderer>().sprite = floorSPrite;
        }
        else if (tile.Type == Tile.TerrainType.Empty)
        {
            obj.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("Invalid floor type - " + tile.Type);
        }
    }
    private float randomizeTimer = 2f;
	// Update is called once per frame
	void Update () {
        randomizeTimer -= Time.deltaTime;
        if (randomizeTimer < 0)
        {
            randomizeTimer = 2f;
        }
    }
}

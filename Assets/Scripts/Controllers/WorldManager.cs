using UnityEngine;
using System.Collections;
using BitStrap;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    public int Width = 50;
    [SerializeField]
    public int Height = 50;
    [SerializeField]
    int seed = 0;
    [SerializeField]
    int PERLIN_SCALE = 3;
    [SerializeField]
    float SNOW_LIMIT = 0.8f;
    [SerializeField]
    float ROCK_LIMIT = 0.7f;
    [SerializeField]
    float GRASS_LIMIT = 0.4f;

    public static WorldManager Instance { get; protected set; }

    private World World { get; set; }

    void OnEnable()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be more than 1 WorldManager.");
        }
        Instance = this;

        CreateWorld(seed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Creates a new world. If the seed is 0 the world will be empty
    /// </summary>
    /// <param name="seed"></param>
    void CreateWorld(int seed)
    {
        if (World == null)
            World = new World(Width, Height);

        if (seed == 0)
            return;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float perlinResult = Mathf.PerlinNoise(((float)(x + seed) / (Width / PERLIN_SCALE)), ((float)(y + seed) / (Height / PERLIN_SCALE)));

                if (perlinResult >= SNOW_LIMIT && perlinResult <= 1)
                    World.GetTileAt(x, y).Type = TileTypes.Snow;
                else if (perlinResult >= ROCK_LIMIT && perlinResult < SNOW_LIMIT)
                    World.GetTileAt(x, y).Type = TileTypes.Rock;
                else if (perlinResult >= GRASS_LIMIT && perlinResult < ROCK_LIMIT)
                {
                    World.GetTileAt(x, y).Type = TileTypes.Grass;
                    //if (rnd.Next(0, 101) > 60)//% chance to have trees
                    //    AddBuilding(x, y, Tile.BuildingType.Trees);
                }
                else
                    World.GetTileAt(x, y).Type = TileTypes.Water;
            }
        }
    }

    [Button]
    void ReloadWorld()
    {
        Instance.CreateWorld(seed);
        Debug.Log("Reloaded World");
    }

    public Tile GetTileAtWorldCoord(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x + 0.5f);
        int y = Mathf.FloorToInt(position.y + 0.5f);

        return World.GetTileAt(x, y);
    }

    public Tile GetTileAtWorldCoord(int x, int y)
    {
        return GetTileAtWorldCoord(new Vector2(x, y));
    }
}

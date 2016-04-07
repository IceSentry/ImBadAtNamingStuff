using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    float noiseScale;

    [SerializeField]
    int octaves;
    [SerializeField]
    [Range(0, 1)]
    float persistance;
    [SerializeField]
    float lacunarity;

    [SerializeField]
    int seed;
    [SerializeField]
    bool randomizeSeed = false;

    [SerializeField]
    Vector2 offset;

    [SerializeField]
    TerrainType[] regions;

    /// <summary>
    /// Creates a new world. If the seed is 0 the world will be empty.
    /// </summary>
    /// <returns>Instance of the new world</returns>
    public World GenerateWorld(World world, int width, int height)
    {
        if (world == null)
            world = new World(width, height);

        if (seed == 0)
            return world;
        else if (randomizeSeed == true)
            seed = Random.Range(1, int.MaxValue);

        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].Height)
                    {
                        world.GetTileAt(x, y).Type = regions[i].Type;
                        break;
                    }
                }
            }
        }

        return world;
    }

    void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}

[System.Serializable]
public struct TerrainType
{
    public float Height;
    public TileTypes Type;
}

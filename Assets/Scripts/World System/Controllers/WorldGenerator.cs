using UnityEngine;

public class WorldGenerator : MonoBehaviour {
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
    Region[] regions;

    /// <summary>
    /// Creates a new world. If the seed is 0 the world will be empty.
    /// </summary>
    /// <returns>Instance of the new world</returns>
    public World GenerateWorld(World world, int width, int height) {
        if (world == null)
            world = new World(width, height);

        if (seed == 0)
            return world;
        else if (randomizeSeed == true)
            seed = UnityEngine.Random.Range(1, int.MaxValue);

        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float currentHeight = noiseMap[x, y];
                foreach (Region region in regions) {
                    if (currentHeight >= region.Height) {
                        world.GetTileAt(x, y).Type = region.Type;
                    }
                }
            }
        }

        return world;
    }

    void OnValidate() {
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}

[System.Serializable]
public struct Region {
    public float Height;
    public TileTypes Type;
}

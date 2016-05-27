using BitStrap;
using UnityEngine;

public class WorldGenerator : Singleton<WorldGenerator> {
#pragma warning disable 0649

    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private int octaves;

    [SerializeField]
    [Range(0, 1)]
    private float persistance;

    [SerializeField]
    private float lacunarity;

    [SerializeField]
    private int seed;

    [SerializeField]
    private bool randomizeSeed = false;

    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private Region[] regions;

#pragma warning restore 0649
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

        Debug.Log("<color=green>World Generated</color>");

        return world;
    }

    private void OnValidate() {
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
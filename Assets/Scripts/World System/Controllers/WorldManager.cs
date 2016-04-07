using BitStrap;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    [SerializeField]
    public int Width = 50;
    [SerializeField]
    public int Height = 50;


    public static WorldManager Instance { get; protected set; }

    private World World { get; set; }

    WorldGenerator worldGenerator;

    void OnEnable() {
        if (Instance != null) {
            Debug.LogError("There should never be more than 1 WorldManager.");
        }
        Instance = this;

        worldGenerator = FindObjectOfType<WorldGenerator>();

        World = worldGenerator.GenerateWorld(World, Width, Height);
    }

    [Button]
    void ReloadWorld() {
        World = worldGenerator.GenerateWorld(World, Width, Height);
        FindObjectOfType<WorldView>().ReloadTexture();
        Debug.Log("Reloaded World");
    }

    public Tile GetTileAtWorldCoord(Vector2 position) {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);

        return World.GetTileAt(x, y);
    }

    public Tile GetTileAtWorldCoord(int x, int y) {
        return GetTileAtWorldCoord(new Vector2(x, y));
    }
}

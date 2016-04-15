using BitStrap;
using UnityEngine;

[ExecuteInEditMode]
public class WorldManager : Singleton<WorldManager> {
    [SerializeField]
    public int Width = 50;
    [SerializeField]
    public int Height = 50;

    private World World;

    public bool IsReloadingWorld = false;


    void OnEnable() {
        World = WorldGenerator.Instance.GenerateWorld(World, Width, Height);
    }

    [Button]
    void ReloadWorld() {
        IsReloadingWorld = true;
        World = WorldGenerator.Instance.GenerateWorld(World, Width, Height);
        WorldView.Instance.ReloadTexture();
        Debug.Log("<color=green>World Reloaded</color>");
        IsReloadingWorld = false;
    }

    public Tile GetTileAtWorldCoord(Vector2 position) {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        return World.GetTileAt(x, y);
    }

    public Tile GetTileAtWorldCoord(int x, int y) {
        return GetTileAtWorldCoord(new Vector2(x, y));
    }

    public Tile GetTileAt(int x, int y) {
        return World.GetTileAt(x, y);
    }
}

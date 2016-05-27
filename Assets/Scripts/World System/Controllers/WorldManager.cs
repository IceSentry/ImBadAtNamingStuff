using BitStrap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO implement a chunk system
//[ExecuteInEditMode]
public class WorldManager : Singleton<WorldManager> {

    [HideInInspector]
    public int Width = 0;

    public int WidthMultiplier = 1;

    [HideInInspector]
    public int Height = 0;

    public int HeightMultiplier = 1;

    private World World;

    public bool IsReloadingWorld = false;

    //private List<ChunkView> chunks = new List<ChunkView>();

    public int chunkYSize = 64;
    public int chunkXSize = 64;
    public GameObject ChunkViewPrefab;

    private void OnEnable() {
        Width = chunkXSize * WidthMultiplier;
        Height = chunkYSize * HeightMultiplier;

        World = WorldGenerator.Instance.GenerateWorld(World, Width, Height);
        for (int x = 0; x < World.Width / chunkXSize; x++) {
            for (int y = 0; y < World.Height / chunkYSize; y++) {
                GameObject chunk = Instantiate(ChunkViewPrefab, new Vector2(x * chunkXSize, y * chunkYSize), Quaternion.identity) as GameObject;
                chunk.transform.SetParent(transform);
                chunk.GetComponent<ChunkView>().Height = chunkYSize;
                chunk.GetComponent<ChunkView>().Width = chunkXSize;
                chunk.GetComponent<ChunkView>().ChunkXIndex = x;
                chunk.GetComponent<ChunkView>().ChunkYIndex = y;
                chunk.GetComponent<ChunkView>().Show();
            }
        }
    }

    [Button]
    private void ReloadWorld() {
        IsReloadingWorld = true;
        World = WorldGenerator.Instance.GenerateWorld(World, Width, Height);
        FindObjectsOfType<ChunkView>().ToList().ForEach(x => x.ReloadTexture());
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
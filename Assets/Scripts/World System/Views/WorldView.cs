using BitStrap;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldView : Singleton<WorldView> {
    private WorldManager worldManager;
    private MeshFilter meshFilter;

    public bool MeshIsGenerated = false;

    //Remember to change this if tilemap changes
    private const int TEXTURE_WIDTH = 6;

    private const int TEXTURE_HEIGHT = 2;
    private const float TEXTURE_TILE_WIDTH = 1f / TEXTURE_WIDTH;
    private const float TEXTURE_TILE_HEIGHT = 1f / TEXTURE_HEIGHT;

    private void Start() {
        meshFilter = GetComponent<MeshFilter>();
        worldManager = WorldManager.Instance;
        GenerateMesh();

        for (int x = 0; x < worldManager.Width; x++) {
            for (int y = 0; y < worldManager.Height; y++) {
                Tile t = worldManager.GetTileAtWorldCoord(x, y);
                t.TypeChanged += OnTypeChanged;
            }
        }
    }

    //TODO implement a chunk system
    public void GenerateMesh() {
        int width = worldManager.Width;
        int height = worldManager.Height;

        MeshData meshData = new MeshData(width, height);

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int verticesIndex = vertices.Count;

                vertices.Add(new Vector3(x, y, 0));
                vertices.Add(new Vector3(x, y + 1, 0));
                vertices.Add(new Vector3(x + 1, y, 0));
                vertices.Add(new Vector3(x + 1, y + 1, 0));

                triangles.Add(verticesIndex);
                triangles.Add(verticesIndex + 1);
                triangles.Add(verticesIndex + 2);

                triangles.Add(verticesIndex + 1);
                triangles.Add(verticesIndex + 2);
                triangles.Add(verticesIndex + 3);
            }
        }

        meshData.vertices = vertices.ToArray();
        meshData.triangles = triangles.ToArray();
        meshData.uv = GetUV();

        meshFilter.sharedMesh = meshData.CreateMesh();
        MeshIsGenerated = true;
        Debug.Log("<color=green>Mesh Generated</color>");
    }

    //TODO: Assign uv based on tile type
    private Vector2[] GetUV() {
        int width = worldManager.Width;
        int height = worldManager.Height;

        List<Vector2> uv = new List<Vector2>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector2 textureIndex = GetTextureIndex(x, y);
                uv.Add(new Vector2(textureIndex.x * TEXTURE_TILE_WIDTH + TEXTURE_TILE_WIDTH, textureIndex.y * TEXTURE_TILE_HEIGHT + TEXTURE_TILE_HEIGHT));
                uv.Add(new Vector2(textureIndex.x * TEXTURE_TILE_WIDTH + TEXTURE_TILE_WIDTH, textureIndex.y * TEXTURE_TILE_HEIGHT));
                uv.Add(new Vector2(textureIndex.x * TEXTURE_TILE_WIDTH, textureIndex.y * TEXTURE_TILE_HEIGHT + TEXTURE_TILE_HEIGHT));
                uv.Add(new Vector2(textureIndex.x * TEXTURE_TILE_WIDTH, textureIndex.y * TEXTURE_TILE_HEIGHT));
            }
        }

        return uv.ToArray();
    }

    //TODO make it less dependant on the value of the enum
    private Vector2 GetTextureIndex(int x, int y) {
        if (worldManager.GetTileAt(x, y) == null)
            return Vector2.zero;

        TileTypes type = worldManager.GetTileAt(x, y).Type;
        float _x = 0;
        float _y = 0;

        switch (type) {
            case TileTypes.Water:
                _x = 0 % TEXTURE_WIDTH;
                _y = 0;
                break;

            case TileTypes.Grass:
                _x = 1 % TEXTURE_WIDTH;
                _y = 0;
                break;

            case TileTypes.Rock:
                _x = 2 % TEXTURE_WIDTH;
                _y = 0;
                break;

            case TileTypes.Dirt:
                _x = 3 % TEXTURE_WIDTH;
                _y = 0;
                break;

            case TileTypes.Snow:
                _x = 4 % TEXTURE_WIDTH;
                _y = 0;
                break;

            default:
                break;
        }

        Vector2 index = new Vector2(_x, _y);
        return index;
    }

    private void UpdateUVs() {
        Vector2[] uv = meshFilter.sharedMesh.uv;

        uv = GetUV();

        meshFilter.sharedMesh.uv = uv;

        meshFilter.sharedMesh.Optimize();
        meshFilter.sharedMesh.RecalculateNormals();

        Debug.Log("<color=green>UV updated</color>");
    }

    public void OnTypeChanged(object source, EventArgs e) {
        UpdateUVs();
    }

    public void ReloadTexture() {
        UpdateUVs();
    }
}
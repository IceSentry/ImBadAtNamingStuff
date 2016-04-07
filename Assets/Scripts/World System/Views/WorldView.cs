using System;
using UnityEngine;

/// <summary>
/// Based on Sebastian Lague tutorial series on Procedural generation.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldView : MonoBehaviour {
    WorldManager worldManager;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    Vector3[] vertices;

    [SerializeField]
    int tileResolution;
    [SerializeField]
    float tileSize = 1.0f;

    public Texture2D TileMap;

    void Start() {
        meshFilter = FindObjectOfType<MeshFilter>();
        meshRenderer = FindObjectOfType<MeshRenderer>();
        worldManager = WorldManager.Instance;
        DrawMesh(GenerateMesh(), GenerateTexture());

        for (int x = 0; x < worldManager.Width; x++) {
            for (int y = 0; y < worldManager.Height; y++) {
                Tile t = worldManager.GetTileAtWorldCoord(x, y);
                t.TypeChanged += OnTypeChanged;
            }
        }

        //transform.position = new Vector2(worldManager.Width / 2 - 0.5f, worldManager.Height / 2 - 0.5f);
    }

    //TODO: Add a requestMeshData method and add chunk support
    MeshData GenerateMesh() {
        int width = worldManager.Width;
        int height = worldManager.Height;

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                meshData.vertices[vertexIndex] = new Vector2(x * tileSize, y * tileSize);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }

        return meshData;
    }

    Texture2D GenerateTexture() {
        int tilesPerRow = TileMap.width / tileResolution;
        int tilesRow = TileMap.height / tileResolution;

        int textureWidth = worldManager.Width * tileResolution;
        int textureHeight = worldManager.Height * tileResolution;

        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        //texture.SetPixels

        for (int x = 0; x < worldManager.Width; x++) {
            for (int y = 0; y < worldManager.Height; y++) {
                Color[] tile = new Color[tileResolution * tileResolution];
                switch (worldManager.GetTileAtWorldCoord(x, y).Type) {
                    case TileTypes.Water:
                        tile = TileMap.GetPixels(tileResolution * 2, tileResolution, tileResolution, tileResolution);
                        break;
                    case TileTypes.Grass:
                        tile = TileMap.GetPixels(0, tileResolution, tileResolution, tileResolution);
                        break;
                    case TileTypes.Rock:
                        tile = TileMap.GetPixels(tileResolution, tileResolution, tileResolution, tileResolution);
                        break;
                    case TileTypes.Dirt:
                        tile = TileMap.GetPixels(tileResolution * 4, tileResolution, tileResolution, tileResolution);
                        break;
                    case TileTypes.Snow:
                        tile = TileMap.GetPixels(tileResolution * 3, tileResolution, tileResolution, tileResolution);
                        break;
                    default:
                        break;
                }
                texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, tile);
            }
        }

        texture.Apply();
        return texture;
    }

    Color[] GenerateColors() {
        Color[] colors = new Color[worldManager.Width * worldManager.Height];



        return colors;
    }

    void DrawMesh(MeshData meshData, Texture2D texture) {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    public void OnTypeChanged(object source, EventArgs e) {
        //meshRenderer.sharedMaterial.mainTexture = GenerateTexture();
    }

    public void ReloadTexture() {
        meshRenderer.sharedMaterial.mainTexture = GenerateTexture();
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight) {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c) {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}

using System;
using UnityEngine;

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
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
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
    void GenerateMesh() {
        int width = worldManager.Width + 1;
        int height = worldManager.Height + 1;

        MeshData meshData = new MeshData(width, height);

        int vertexIndex = 0;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                meshData.vertices[vertexIndex] = new Vector2(x * tileSize, y * tileSize);

                if (x < width - 1 && y < height - 1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        //meshData.uv = AssignUV();

        meshFilter.sharedMesh = meshData.CreateMesh();
    }

    #region slow way of drawing the mesh
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
    #endregion

    //TODO: Assign uv based on tile type
    Vector2[] AssignUV() {
        throw new NotImplementedException();
    }

    void UpdateUVs() {
        Vector2[] uv = meshFilter.sharedMesh.uv;

        uv = AssignUV();

        meshFilter.sharedMesh.uv = uv;

        meshFilter.sharedMesh.Optimize();
        meshFilter.sharedMesh.RecalculateNormals();
    }

    public void OnTypeChanged(object source, EventArgs e) {
        UpdateUVs();
    }

    public void ReloadTexture() {
        UpdateUVs();
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight) {
        vertices = new Vector3[meshWidth * meshHeight];
        uv = new Vector2[meshWidth * meshHeight];
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
        mesh.uv = uv;
        mesh.Optimize();
        mesh.RecalculateNormals();
        return mesh;
    }
}

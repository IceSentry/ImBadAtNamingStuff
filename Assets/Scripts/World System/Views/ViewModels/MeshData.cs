using UnityEngine;

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight) {
        vertices = new Vector3[meshWidth * meshHeight * 4];
        uv = new Vector2[meshWidth * meshHeight * 4];
        triangles = new int[meshWidth * meshHeight * 6];
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
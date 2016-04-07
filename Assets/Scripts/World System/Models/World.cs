using UnityEngine;
using System.Collections;
using System;

public class World
{
    public Tile[,] Tiles { get; set; }

    public int Width;
    public int Height;

    public World(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[Width, Height];
        InstantitateTiles();
    }

    void InstantitateTiles()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tiles[x, y] = new Tile(TileTypes.Water, new Vector2(x, y));
            }
        }
    }

    internal Tile GetTileAt(int x, int y)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            //Debug.LogError(string.Format("Tile ({0},{1}) is out of range.", x, y));
            return null;
        }
        return Tiles[x, y];
    }
}

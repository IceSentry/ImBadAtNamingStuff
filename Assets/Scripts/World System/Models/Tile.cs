using System;
using UnityEngine;

public enum TileTypes { Water, Grass, Rock, Dirt, Snow }

public class Tile {

    public delegate void TypeChangedEventHandler(object source, EventArgs args);

    public event TypeChangedEventHandler TypeChanged;

    private TileTypes type;

    public TileTypes Type {
        get { return type; }
        set {
            TileTypes oldType = type;
            type = value;
            if (TypeChanged != null && type != oldType)
                OnTypeChanged();
        }
    }

    public Vector2 Position;

    private World world;

    public World World {
        get { return world; }
        set { world = value; }
    }

    public Tile(TileTypes type, Vector2 position, World world) {
        Type = type;
        Position = position;
        World = world;
    }

    public Tile[] GetNeighbours(bool diagonalOkay = false) {
        int arraySize = diagonalOkay ? 8 : 4;
        Tile[] neighbours = new Tile[arraySize];

        neighbours[0] = world.GetTileAt((int)Position.x + 1, (int)Position.y);
        neighbours[1] = world.GetTileAt((int)Position.x - 1, (int)Position.y);
        neighbours[2] = world.GetTileAt((int)Position.x, (int)Position.y + 1);
        neighbours[3] = world.GetTileAt((int)Position.x, (int)Position.y - 1);

        if (diagonalOkay) {
            neighbours[4] = world.GetTileAt((int)Position.x + 1, (int)Position.y + 1);
            neighbours[5] = world.GetTileAt((int)Position.x - 1, (int)Position.y - 1);
            neighbours[6] = world.GetTileAt((int)Position.x - 1, (int)Position.y + 1);
            neighbours[7] = world.GetTileAt((int)Position.x + 1, (int)Position.y - 1);
        }

        return neighbours;
    }

    protected virtual void OnTypeChanged() {
        if (TypeChanged != null && !WorldManager.Instance.IsReloadingWorld)
            TypeChanged(this, EventArgs.Empty);
    }
}
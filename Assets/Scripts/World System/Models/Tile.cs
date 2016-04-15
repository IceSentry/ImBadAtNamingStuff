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

    public Tile(TileTypes type, Vector2 position) {
        Type = type;
        Position = position;
    }

    protected virtual void OnTypeChanged() {
        if (TypeChanged != null && !WorldManager.Instance.IsReloadingWorld)
            TypeChanged(this, EventArgs.Empty);
    }
}
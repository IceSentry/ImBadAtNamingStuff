using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TileSpriteController : MonoBehaviour
{
    WorldManager worldManager;

    Dictionary<Tile, GameObject> tileGameObjectMap;

    Dictionary<string, Sprite> tileSprites;

    void Start()
    {
        LoadSprites();
        worldManager = WorldManager.Instance;
        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        for (int x = 0; x < worldManager.Width; x++)
        {
            for (int y = 0; y < worldManager.Height; y++)
            {
                Tile t = worldManager.GetTileAtWorldCoord(x, y);
                CreateTileGameObject(t);
                t.TypeChanged += OnTypeChanged;
            }
        }
    }

    private void LoadSprites()
    {
        tileSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/tiles/");

        foreach (Sprite s in sprites)
        {
            //Debug.Log(s);
            tileSprites[s.name] = s;
        }
    }

    void CreateTileGameObject(Tile tile)
    {
        GameObject gameObject = new GameObject();

        gameObject.name = "Tile " + tile.Position;
        gameObject.transform.position = tile.Position;
        gameObject.transform.SetParent(transform, true);

        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = GetSprite(tile.Type);
        sr.sortingLayerName = "Tiles";

        tileGameObjectMap.Add(tile, gameObject);
    }

    public void OnTypeChanged(object source, EventArgs e)
    {
        Tile tile = (Tile)source;
        GameObject gameObject = tileGameObjectMap[tile];
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = GetSprite(tile.Type);
    }

    private Sprite GetSprite(TileTypes type)
    {
        return tileSprites[type.ToString()];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScript : MonoBehaviour
{
    Dictionary<string, List<Tile>> tileDictionary;
    List<Tilemap> tilemaps;
    // Start is called before the first frame update
    void Start()
    {
        tileDictionary = new Dictionary<string, List<Tile>>();
        tilemaps = new List<Tilemap>();
        var totalTilemaps = this.gameObject.transform.childCount;
        for(var i = 0;i<totalTilemaps;i++)
        {
            var child = this.gameObject.transform.GetChild(i).GetComponent<Tilemap>();
            
            tilemaps.Add(child);
        }
        Debug.Log(string.Format("Completed. Added {0} tilemaps",tilemaps.Count));

        UpdateTileDictionary();
        SetTilesDynamically();
    }

    // Update is called once per frame
    void Update()
    {
        tilemaps[0].RefreshAllTiles();
        tilemaps[1].RefreshAllTiles();
    }

    void UpdateTileDictionary()
    {
        foreach (var tilemap in tilemaps)
        {
            Debug.Log(string.Format("Adding tiles for {0}", tilemap.name));
            tileDictionary.Add(tilemap.name, new List<Tile>());
            for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
            {
                for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
                {
                    var vector = new Vector3Int(x, y, 0);
                    var tile = tilemap.GetTile(vector) as Tile;
                    if (tile != null)
                    {
                        tileDictionary[tilemap.name].Add(tile);
                        Debug.Log(string.Format("Added tile for tilemap {0} at {1}x:{2}y using sprite {3}",tilemap.name, x,y,tile.sprite));
                    }
                }
            }
            Debug.Log(string.Format("Addied {0} tiles for {1}",tileDictionary[tilemap.name].Count, tilemap.name));
        }
    }
    void SetTilesDynamically()
    {
        var tilemapToUse = tilemaps[0];
        Debug.Log(string.Format("Setting tiles dynamically for tilemap {0}", tilemapToUse.name));
        var vector = tilemapToUse.WorldToCell(transform.position);

        Debug.Log(string.Format("Found current position {0}x:{1}y:{2}z", vector.x, vector.y, vector.z));

        vector.x += 5;
        vector.y += 5;

        Debug.Log(string.Format("adjusting to new position {0}x:{1}y:{2}z", vector.x, vector.y, vector.z));

        tilemapToUse.SetTile(vector, Tile.Instantiate(((Tile)tilemapToUse.GetTile(new Vector3Int(-3,3,0)))));

    }
}

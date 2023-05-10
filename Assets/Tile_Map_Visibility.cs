using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Tile_Map_Visibility : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject player;
    [SerializeField] TileBase mountainTile;

    // Start is called before the first frame update
    void Start()
    {
        Tilemap originalTiles = new Tilemap();
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                originalTiles.SetTile(pos, tile);
            }
        }

        updateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        updateVisibility();
    }

    private void updateVisibility(){
        // Clear the tilemap and reset it to the original tiles
        tilemap.ClearAllTiles();
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                tilemap.SetTile(pos, tile);
            }
        }

        // Hide tiles based on mountain tiles
        Vector3Int playerPos = tilemap.WorldToCell(player.transform.position);
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null && tile != mountainTile)
            {
                // Check if there is a mountain tile between player and current tile
                bool visible = true;
                // check if there is a mountain tile between player and current tile using Bresenham's line algorithm
                foreach (Vector3Int mountainPos in GetPositionsBetween(playerPos, pos))
                {
                    TileBase mountainTilecheck = tilemap.GetTile(mountainPos);
                    if (mountainTile != null && mountainTilecheck == mountainTile)
                    {
                        visible = false;
                        break;
                    }
                }

                // Hide tile if there is a mountain tile between player and current tile
                if (!visible)
                {
                    tilemap.SetTileFlags(pos, TileFlags.None);
                    tilemap.SetColor(pos, new Color(1f, 1f, 1f, 0.5f));
                }
    }
}
    }

    private List<Vector3Int> GetPositionsBetween(Vector3Int a, Vector3Int b)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        int x = a.x;
        int y = a.y;
        int dx = b.x - a.x;
        int dy = b.y - a.y;
        int xIncrement = dx > 0 ? 1 : -1;
        int yIncrement = dy > 0 ? 1 : -1;
        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);
        if (longest < shortest)
        {
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);
            xIncrement = dx > 0 ? 1 : -1;
            yIncrement = dy > 0 ? 1 : -1;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            positions.Add(new Vector3Int(x, y, 0));
            numerator += shortest;
            if (numerator >= longest)
            {
                numerator -= longest;
                x += xIncrement;
                y += yIncrement;
            }
            else
            {
                x += xIncrement;
            }
        }
        return positions;
    }
}
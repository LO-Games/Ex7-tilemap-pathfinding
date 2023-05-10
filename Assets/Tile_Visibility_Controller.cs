using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile_Visibility_Controller : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Transform playerTransform;
    [SerializeField] TileBase mountainTile;

    private Dictionary<Vector3Int, Tile> originalTiles = new Dictionary<Vector3Int, Tile>();

    private void Start()
    {
        // Store the original tiles of the tilemap
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                originalTiles.Add(pos, tile);
            }
        }
    }

    private void Update()
    {
        // Clear the tilemap and reset it to the original tiles
        tilemap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, Tile> pair in originalTiles)
        {
            tilemap.SetTile(pair.Key, pair.Value);
        }

        // Hide tiles based on mountain tiles
        Vector3Int playerPos = tilemap.WorldToCell(playerTransform.position);
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null && tile != mountainTile)
            {
                // Check if there is a mountain tile between player and current tile
                bool visible = true;
                foreach (Vector3Int mountainPos in GetPositionsBetween(playerPos, pos))
                {
                    Tile mountainTile = tilemap.GetTile<Tile>(mountainPos);
                    if (mountainTile != null && mountainTile == mountainTile)
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

    private List<Vector3Int> GetPositionsBetween(Vector3Int startPos, Vector3Int endPos)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        int dx = Mathf.Abs(endPos.x - startPos.x);
        int dy = Mathf.Abs(endPos.y - startPos.y);
        int sx = startPos.x < endPos.x ? 1 : -1;
        int sy = startPos.y < endPos.y ? 1 : -1;
        int err = dx - dy;

        while (startPos != endPos)
        {
            positions.Add(startPos);
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                startPos.x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                startPos.y += sy;
            }
        }

        positions.Add(startPos);
        return positions;
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUpdater : MonoBehaviour
{
    public Tilemap tilemap;         // reference to the Tilemap component
    public Tile mountainTile;      // reference to the mountain tile

    private Vector3Int prevPlayerPos = new Vector3Int();   // previous player position

    void Update()
    {
        // get player current tile coordinates
        Vector3Int playerPos = tilemap.WorldToCell(transform.position);

        // check if the player has moved
        if (playerPos != prevPlayerPos)
        {
            // loop through each tile in the tilemap
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                // check if the tile is within range of the player
                if (Mathf.Abs(pos.x - playerPos.x) <= 2 && Mathf.Abs(pos.y - playerPos.y) <= 2)
                {
                    // check if there is a mountain tile between the player and this tile
                    bool mountainFound = false;

                    // raycast from player to tile to check for mountain tiles
                    Vector3Int rayDirection = pos - playerPos;
                    Vector2Int rayDirection2D = new Vector2Int(rayDirection.x, rayDirection.y);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection2D, rayDirection2D.magnitude);

                    foreach (var hit in hits)
                    {
                        TileBase tile = tilemap.GetTile(tilemap.WorldToCell(hit.point));
                        if (tile == mountainTile)
                        {
                            mountainFound = true;
                            break;
                        }
                    }

                    // set the tile color based on the mountain tile condition
                    if (mountainFound)
                    {
                        tilemap.SetTileFlags(pos, TileFlags.None);
                        tilemap.SetColor(pos, Color.black);
                    }
                    else
                    {
                        tilemap.SetTileFlags(pos, TileFlags.LockColor);
                        tilemap.SetColor(pos, Color.white);
                    }
                }
            }

            // update previous player position
            prevPlayerPos = playerPos;
        }
    }
}
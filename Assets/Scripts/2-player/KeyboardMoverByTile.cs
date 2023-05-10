using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile : KeyboardMover
{
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] private bool goat_key = false;
    [SerializeField] private TileBase mountain_tile = null;
    [SerializeField] private TileBase[] water_tile = null;

    [SerializeField] private TileBase goat_tile = null;
    [SerializeField] private bool ship_key = false;
    [SerializeField] private TileBase ship_tile = null;
    [SerializeField] private bool hammer_key = false;
    [SerializeField] private TileBase hammer_tile = null;
    [SerializeField] private TileBase normal_tile = null;
    TargetMover tm = null;
    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Start()
    {
        tm = GetComponent<TargetMover>();
    }

    void Update()
    {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        if (allowedTiles.Contain(tileOnNewPosition))
        {
            transform.position = newPosition;
            if (tileOnNewPosition == goat_tile)
            {
                goat_key = true;
                allowedTiles.Add(mountain_tile);
                tm.update_allowed_tiles(allowedTiles);
                tilemap.SetTile(tilemap.WorldToCell(newPosition), normal_tile);
            }
            if (tileOnNewPosition == ship_tile)
            {
                ship_key = true;
                foreach (TileBase tile in water_tile)
                {
                    allowedTiles.Add(tile);
                }
                tm.update_allowed_tiles(allowedTiles);
                tilemap.SetTile(tilemap.WorldToCell(newPosition), normal_tile);
            }
            if (tileOnNewPosition == hammer_tile)
            {
                hammer_key = true;
                allowedTiles.Add(mountain_tile);
                tm.update_allowed_tiles(allowedTiles);
                tilemap.SetTile(tilemap.WorldToCell(newPosition), normal_tile);
            }
            if (tileOnNewPosition == mountain_tile && hammer_key)
            {
                tilemap.SetTile(tilemap.WorldToCell(newPosition), normal_tile);
            }

        }
        else
        {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
    }
}

